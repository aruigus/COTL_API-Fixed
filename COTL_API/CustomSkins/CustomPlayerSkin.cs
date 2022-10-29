﻿using Spine;
using Spine.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace COTL_API.CustomSkins;

public abstract class CustomPlayerSkin : CustomSkin
{
    public void Apply()
    {
        Action a = delegate
        {
            Skin from = PlayerFarming.Instance.Spine.Skeleton.Data.FindSkin("Lamb");
            Skin to = new(Name);

            Material mat;
            SpineAtlasAsset atlas;
            List<Tuple<int, string, float, float, float, float>> overrides = SkinUtils.CreateSkinAtlas(Name, Texture,
                GenerateAtlasText(), delegate(AtlasRegion region)
                {
                    string simpleName = region.name;
                    string add = "";
                    if (simpleName.Contains("#"))
                    {
                        string[] split = simpleName.Split('#');
                        add = "#" + split[1];
                        simpleName = split[0];
                    }
                    if (from.Attachments.Any(x => x.Name == simpleName))
                    {
                        Skin.SkinEntry att = from.Attachments.First(x => x.Name == simpleName);
                        region.name = att.SlotIndex + ":" + att.Name + add;
                        return Tuple.Create(att.SlotIndex, att.Name);
                    }

                    return null;
                }, out mat, out atlas);
            Skin overrideSkin = SkinUtils.ApplyAllOverrides(from, to, overrides, mat, atlas);

            if (CustomSkinManager.PlayerSkinOverride != null)
                Plugin.Logger.LogWarning("PlayerSkinOverride already exists. Overwriting.");
            CustomSkinManager.SetPlayerSkinOverride(overrideSkin);
        };
        SkinUtils.OnFindSkin += a;
    }
}