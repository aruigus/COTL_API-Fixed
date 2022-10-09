﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;
using COTL_API.UI.Patches;
using TMPro;

namespace COTL_API.UI.Helpers;
public static class GameObjectExtensions
{
    // Extension methods for GameObjects.
    public static GameObject AttachToParent(this GameObject obj, Transform parent)
    {
        obj.transform.SetParent(parent);
        return obj;
    }

    public static GameObject ChangePosition(this GameObject obj,
        float x = 0, float y = 0, float z = 0)
    {
        obj.transform.localPosition = new Vector3(x, y, z);
        return obj;
    }

    public static GameObject AttachImage(this GameObject obj, Sprite sprite, int opacity = 100)
    {
        Image img = obj.AddComponent<Image>();
        img.sprite = sprite;
        img.SetNativeSize();
        img.preserveAspect = true;

        if(opacity < 100 && opacity >= 0)
        {
            Color color = img.color;
            color.a = (float)opacity / 100f;
            img.color = color;
        }
        return obj;
    }

    // I wanted opacity to be an int from 0-100 because I think most people are more used to terms like "100% opacity" and "50% opacity" than "1f opacity" and "0.5f opacity".
    // I can change it if necessary (or make a variation of this method that takes a float).

    public static GameObject AttachImage(this GameObject obj, string imagePath, int opacity = 100,
        FilterMode filter = FilterMode.Bilinear)
    {
        Sprite sprite = UITextureLoader.MakeSprite(imagePath, filter);

        Image img = obj.AddComponent<Image>();
        img.sprite = sprite;
        img.SetNativeSize();
        img.preserveAspect = true;

        if (opacity < 100 && opacity >= 0)
        {
            Color color = img.color;
            color.a = (float)opacity / 100f;
            img.color = color;
        }
        return obj;
    }

    public static GameObject CreateChild(this GameObject obj, string name)
    {
        Transform parent = obj.transform;
        return UIHelpers.CreateUIObject(name, parent);
    }

    public static GameObject MakeDraggable(this GameObject obj)
    {
        var script = obj.AddComponent<UIBehaviourHelpers.DraggableUIObject>();
        return obj;
    }

    public static GameObject AddText(this GameObject obj, string message, float fontSize = 10f, TextAlignmentOptions alignment = TextAlignmentOptions.Center)
    {
        TextMeshProUGUI textMesh = obj.AddComponent<TextMeshProUGUI>();
        textMesh.font = FontHelpers.GetAnyFont;
        textMesh.fontSize = fontSize;
        textMesh.text = message;
        textMesh.alignment = alignment;
        return obj;
    }

    public static GameObject EditText(this GameObject obj, string message)
    {
        TextMeshProUGUI textMesh = obj.GetComponent<TextMeshProUGUI>();
        if(textMesh == null)
        {
            Plugin.Logger.LogWarning("EditText: TextMeshProUGUI component not found.");
            return obj;
        }
        textMesh.text = message;
        return obj;
    }
}
