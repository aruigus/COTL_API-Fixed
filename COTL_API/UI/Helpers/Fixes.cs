﻿using System.Diagnostics;
using System.Globalization;
using COTL_API.CustomStructures;
using HarmonyLib;
using MMRoomGeneration;
using UnityEngine;
using Object = UnityEngine.Object;

namespace COTL_API.UI.Helpers;

[HarmonyPatch]
public static class Fixes
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(UnityEngine.Debug), nameof(UnityEngine.Debug.LogError), typeof(object))]
    [HarmonyPatch(typeof(UnityEngine.Debug), nameof(UnityEngine.Debug.LogError), typeof(object), typeof(Object))]
    public static bool Debug_LogWarning(ref object message)
    {
        // Stops the game complaining about missing fonts for specific cultures
        if (message is not string msg) return true;

        var currentCulture = CultureInfo.CurrentCulture;
        var cultureName = currentCulture.Name;

        // Check if the user's culture is Chinese (Simplified), Chinese (Traditional), Japanese, or Korean
        var isTargetCulture = cultureName is "zh-CN" or "zh-TW" or "ja-JP" or "ko-KR";

        // Only suppress the log message if the user's culture is not one of the target cultures
        return !(!isTargetCulture && msg.Contains("Font at path"));
    }


    [HarmonyWrapSafe]
    [HarmonyFinalizer]
    [HarmonyPatch(typeof(StructureBrain), nameof(StructureBrain.ApplyConfigToData))]
    [HarmonyPatch(typeof(LocationManager), nameof(LocationManager.PlaceStructure))]
    public static Exception? Finalizer()
    {
        //stops game complaining about invalid data from known removed custom structures. Happens once per structure, and is resolved in GenerateRoom.OnDisable postfix
        return null;
    }


    // HarmonyPostfix patch for GenerateRoom.OnDisable method
    [HarmonyPostfix]
    [HarmonyPatch(typeof(GenerateRoom), nameof(GenerateRoom.OnDisable))]
    private static void GenerateRoom_OnDisable(ref GenerateRoom __instance)
    {
        // Check if the MatingTentMod is installed
        if (!MatingTentModExists())
        {
            // Log the removal of mating tents
            LogInfo("MatingTentMod not found, checking for and removing any rogue tents.");

            // Remove any rogue mating tent GameObjects
            RemoveMatingTentGameObjects();
        }

        // Log the removal of rogue custom structures and DataManager correction
        LogInfo("Checking other custom structures and correcting DataManager (SaveData)");

        // Remove any rogue custom structures from DataManager
        RemoveRogueCustomStructuresFromDataManager();
    }

    // Method to remove mating tent GameObjects
    private static void RemoveMatingTentGameObjects()
    {
        // Find all GameObjects containing "Mating" in their names
        var tentObjects = Object.FindObjectsOfType<GameObject>()
            .Where(obj => obj.name.Contains("Mating")).ToList();

        if (!tentObjects.Any()) return;

        // Destroy all found mating tent GameObjects
        foreach (var tentObject in tentObjects)
        {
            Object.DestroyImmediate(tentObject);
            LogWarning("Destroyed mating tent game object.");
        }
    }

    // Method to remove rogue custom structures from DataManager
    private static void RemoveRogueCustomStructuresFromDataManager()
    {
        var stopWatch = new Stopwatch();
        stopWatch.Start();

        // Get all fields of type List<StructuresData> in DataManager
        var listOfStructuresDataFields = AccessTools.GetDeclaredFields(typeof(DataManager)).Where(a => a.FieldType == typeof(List<StructuresData>)).ToList();
        bool dataFixed = false;
        // Iterate through the fields, removing mating tents and custom structures that do not exist in the CustomStructureList
        foreach (var field in listOfStructuresDataFields)
        {
            if (field.GetValue(DataManager.Instance) is not List<StructuresData> f) continue;

            var itemsToRemove = new List<StructuresData>();

            // Check for custom structures that need to be removed
            f.ForEach(structure =>
            {
                if (structure.PrefabPath != null && structure.PrefabPath.Contains("CustomBuildingPrefab"))
                {
                    LogInfo($"Found custom item in {field.Name}: {structure.PrefabPath}");
                    if (CustomStructureManager.CustomStructureExists(structure.PrefabPath)) return;

                    // Item needs to be removed from BaseStructures
                    LogWarning($"Found custom item in {field.Name} from a mod no longer installed!: {structure.PrefabPath}");
                    itemsToRemove.Add(structure);
                }
            });

            // Remove custom structures
            var customCount = f.RemoveAll(a => itemsToRemove.Contains(a));

            // Remove mating tents
            var tentCount = 0;
            if (!MatingTentModExists())
            {
                tentCount = f.RemoveAll(a => a == null || (a is {PrefabPath: not null} && a.PrefabPath.Contains("Mating")));
            }

            // Update the field in DataManager with the modified list
            field.SetValue(DataManager.Instance, f);

            // Log the number of removed custom structures and mating tents
            if (customCount > 0 || tentCount > 0)
            {
                dataFixed = true;
                LogInfo($"Removed {customCount} orphaned structure(s) and {tentCount} orphaned tent(s) from {field.Name}.");
            }
        }

        // Save the changes and log the time taken for the process
        stopWatch.Stop();
        if (dataFixed)
        {
            LogInfo($"Finished correcting DataManager (SaveData) in {stopWatch.ElapsedMilliseconds}ms & {stopWatch.ElapsedTicks} ticks.");
            //  SaveAndLoad.Save();
        }
        else
        {
            LogInfo($"No orphaned structure(s), so no changes made to DataManager (SaveData) in {stopWatch.ElapsedMilliseconds}ms & {stopWatch.ElapsedTicks} ticks.");
        }
    }

    // Method to check if the MatingTentMod is installed
    private static bool MatingTentModExists()
    {
        // Find the MatingTentMod plugin in the BepInEx plugin list
        var matingTentMod = BepInEx.Bootstrap.Chainloader.PluginInfos.FirstOrDefault(a => a.Value.Metadata.GUID.Contains("MatingTentMod")).Value;
        return matingTentMod != null;
    }
}