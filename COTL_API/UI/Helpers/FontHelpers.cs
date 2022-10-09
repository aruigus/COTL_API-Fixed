﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using UnityEngine.TextCore;
using UnityEngine;
using Lamb.UI;
using TMPro;

namespace COTL_API.UI.Helpers;
public static class FontHelpers
{
    internal static TMP_FontAsset _startMenu, _pauseMenu;
    // Getters
    public static TMP_FontAsset PauseMenu => _pauseMenu;
    public static TMP_FontAsset StartMenu => _startMenu;
    public static TMP_FontAsset GetAnyFont => PauseMenu ?? StartMenu;

}
