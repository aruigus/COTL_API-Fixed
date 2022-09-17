﻿using HarmonyLib;
using Lamb.UI.FollowerInteractionWheel;
using System.Collections.Generic;

namespace COTL_API.Patches;

//This fixes custom follower commands with subcommands from raising an exception when the plugin uses IsAvailable as the original method passes null
[HarmonyPatch(typeof(UIFollowerInteractionWheelOverlayController), "MakeChoice", typeof(UIFollowerWheelInteractionItem))]
[HarmonyWrapSafe]
public static class MakeChoicePatch
{
    [HarmonyPrefix]
    public static bool Prefix(ref UIFollowerWheelInteractionItem item, ref bool __state)
    {
        if (item.CommandItem.SubCommands != null && item.CommandItem is CustomFollowerCommand.CustomFollowerCommand && item.CommandItem.SubCommands.Count > 0)

        {
            Plugin.Logger.LogMessage($"Custom command with sub commands, not letting normal method run.");
            __state = true;
            return false;
        }

        Plugin.Logger.LogMessage($"Not a custom command or doesnt have sub-commands, letting normal method run.");
        __state = false;
        return true;
    }

    [HarmonyPostfix]
    public static void Postfix(
        ref UIFollowerInteractionWheelOverlayController __instance,
        ref UIFollowerWheelInteractionItem item,
        ref Follower ____follower,
        ref Stack<List<CommandItem>> ____commandStack,
        ref Stack<FollowerCommands> ____commandHistory,
        ref List<CommandItem> ____rootCommandItems,
        ref bool __state)
    {
        if (!__state) return;
        Plugin.Logger.LogMessage($"Custom command original method skipped, this is from the postfix.");
        if (item.CommandItem.SubCommands is {Count: > 0})
        {
            if (item.CommandItem.IsAvailable(____follower))
            {
                ____commandStack.Push(____rootCommandItems);
                ____commandHistory.Push(item.CommandItem.Command);
                ____rootCommandItems = item.CommandItem.SubCommands;
                __instance.StartCoroutine(__instance.NextCategory(____rootCommandItems));
                return;
            }

            //without this the commands title and descriptions dont update if the user selects a greyed out item, instead
            //it will now just close the menu instead of raising an exception
            Plugin.Logger.LogMessage($"User pressed select on a greyed out sub command, closing menu and aborting choice.");
            __instance.OnCancelButtonInput();
            return;
        }

        if (item.FollowerCommand == FollowerCommands.AreYouSureNo)
        {
            __instance.OnCancelButtonInput();
            return;
        }

        __instance.Hide();
        ____commandHistory.Push(item.CommandItem.Command);
        var onItemChosen = __instance.OnItemChosen;
        onItemChosen?.Invoke(____commandHistory.ToArray());
    }
}