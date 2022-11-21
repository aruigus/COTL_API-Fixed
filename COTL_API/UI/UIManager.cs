﻿using Object = UnityEngine.Object;
using COTL_API.CustomSettings;
using Lamb.UI.SettingsMenu;
using Lamb.UI.Settings;
using UnityEngine.UI;
using UnityEngine;
using HarmonyLib;
using Lamb.UI;
using TMPro;

namespace COTL_API.UI;

[HarmonyPatch]
public class UIManager
{
    [HarmonyPatch(typeof(UISettingsMenuController), nameof(UISettingsMenuController.OnShowStarted))]
    [HarmonyPostfix]
    public static void UISettingsMenuController_OnShowStarted(UISettingsMenuController __instance)
    {
        if (SettingsUtils.SliderTemplate == null)
        {
            SettingsUtils.SliderTemplate = __instance._gameSettings.GetComponentInChildren<ScrollRect>().content
                .GetChild(1).gameObject;
            SettingsUtils.ToggleTemplate = __instance._gameSettings.GetComponentInChildren<ScrollRect>().content
                .GetChild(2).gameObject;
            SettingsUtils.HorizontalSelectorTemplate = __instance._gameSettings.GetComponentInChildren<ScrollRect>()
                .content.GetChild(3).gameObject;
            SettingsUtils.HeaderTemplate = __instance._graphicsSettings.GetComponentInChildren<ScrollRect>().content
                .GetChild(0).gameObject;
        }

        var originalGraphicsSettings = __instance._graphicsSettings;
        var stnb = __instance.transform.GetComponentInChildren<SettingsTabNavigatorBase>();
        var hlg = stnb.GetComponentInChildren<HorizontalLayoutGroup>();
        var graphicsSettingsTab = hlg.transform.GetChild(2);
        var newSettings = Object.Instantiate(graphicsSettingsTab, hlg.transform);
        newSettings.SetSiblingIndex(hlg.transform.childCount - 2);
        newSettings.name = "Mod Settings Button";
        var text = newSettings.GetComponentInChildren<TMP_Text>();
        text.text = "Mods";
        var content = __instance.GetComponentInChildren<GameSettings>().transform.parent.gameObject;
        var graphicsSettings = content.transform.GetChild(1);
        var newGraphicsSettings = Object.Instantiate(graphicsSettings, content.transform);
        newGraphicsSettings.name = "Mod Settings Content";
        newGraphicsSettings.gameObject.SetActive(true);
        var copy = newGraphicsSettings.GetComponentInChildren<GraphicsSettings>();
        var tab = newSettings.GetComponent<SettingsTab>();
        tab._menu = copy;
        copy._defaultSelectable = graphicsSettingsTab.GetComponentInChildren<Selectable>();
        var onShow = originalGraphicsSettings.OnShow;
        var onHide = originalGraphicsSettings.OnHide;
        Delegate[] onShowDelegates = onShow.GetInvocationList();
        Delegate[] onHideDelegates = onHide.GetInvocationList();
        var showDelegate = (Action)onHideDelegates[1];
        var hideDelegate = (Action)onShowDelegates[1];
        copy.OnShow += showDelegate;
        copy.OnHide += hideDelegate;
        originalGraphicsSettings.OnShow -= showDelegate;
        originalGraphicsSettings.OnHide -= hideDelegate;

        __instance.transform.GetComponentInChildren<SettingsTabNavigatorBase>()._tabs = __instance.transform
            .GetComponentInChildren<SettingsTabNavigatorBase>()._tabs.Append(tab).ToArray();
        var button = newSettings.GetComponentInChildren<MMButton>();
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => stnb.TransitionTo(tab));
    }

    [HarmonyPatch(typeof(GraphicsSettings), nameof(GraphicsSettings.OnShowStarted))]
    [HarmonyPrefix]
    public static void UISettingsMenuController_OnShowStarted(GraphicsSettings __instance)
    {
        if (__instance.name == "Mod Settings Content")
        {
            __instance._targetFpsSelectable.HorizontalSelector._canvasGroup = __instance._canvasGroup;
        }
    }

    [HarmonyPatch(typeof(GraphicsSettings), nameof(GraphicsSettings.Start))]
    [HarmonyPrefix]
    public static bool GraphicsSettings_Start(GraphicsSettings __instance)
    {
        if (__instance.name != "Mod Settings Content") return true;

        Transform scrollContent = __instance._scrollRect.content;
        foreach (Transform child in scrollContent)
        {
            Object.Destroy(child.gameObject);
        }

        string? currentCategory = null;
        foreach (var element in CustomSettingsManager.SettingsElements.OrderBy(x => x.Category)
                     .ThenBy(x => x.Text))
        {
            if (element.Category != currentCategory)
            {
                currentCategory = element.Category;
                SettingsUtils.AddHeader(scrollContent, currentCategory);
            }

            switch (element)
            {
                case Dropdown dropdown:
                    SettingsUtils.AddHorizontalSelector(scrollContent, dropdown.Text, dropdown.Options, -1,
                        dropdown.OnValueChanged, dropdown.Value);
                    break;
                case Slider slider:
                {
                    void OnValueChanged(float i)
                    {
                        slider.OnValueChanged?.Invoke(i);
                    }

                    SettingsUtils.AddSlider(scrollContent, slider.Text, slider.Value, slider.Min, slider.Max,
                        slider.Increment, slider.DisplayFormat, OnValueChanged);
                    break;
                }
                case Toggle toggle:
                    SettingsUtils.AddToggle(scrollContent, toggle.Text, toggle.Value, toggle.OnValueChanged);
                    break;
            }
        }

        return false;
    }
}