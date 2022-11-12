﻿using System.Linq;
using System.IO;
using FMODUnity;
using BepInEx;
using FMOD;

namespace COTL_API.Sounds;

public enum VolumeCategory
{
    Master,
    Music,
    SFX,
    VO
}

public static class SoundHelpers
{
    /// <summary>
    /// The game's Master Volume.
    /// </summary>
    public static float MasterVolume => SettingsManager.Settings.Audio.MasterVolume;

    /// <summary>
    /// The game's Music Volume.
    /// </summary>
    public static float MusicVolume => SettingsManager.Settings.Audio.MusicVolume * MasterVolume;

    /// <summary>
    /// The game's SFX Volume.
    /// </summary>
    public static float SfxVolume => SettingsManager.Settings.Audio.SFXVolume * MasterVolume;

    /// <summary>
    /// The game's VO Volume.
    /// </summary>
    public static float VoVolume => SettingsManager.Settings.Audio.VOVolume * MasterVolume;

    // Sound helpers -- all personal and not meant for users, so they're internal
    internal static Sound MakeSound(string fileName, bool loop = false)
    {
        var path = GetPath(fileName);
        if (path == null) return new Sound();

        var system = RuntimeManager.CoreSystem;

        var mode = loop ? MODE.LOOP_NORMAL : MODE.LOOP_OFF;

        var result = system.createSound(path, mode, out var sound);

        if (result == RESULT.OK) return sound;

        Plugin.Instance.Logger.LogError($"Error making sound from file {fileName}!");
        result.IfErrorPrintWith($"MakeSound() -- fileName: {fileName}");

        return new Sound(); // Return empty sound in the case of an error
    }

    internal static RESULT PlaySound(Sound sound, VolumeCategory volumeCategory = VolumeCategory.Master)
    {
        var system = RuntimeManager.CoreSystem;
        var result = system.playSound(sound, new ChannelGroup(), false, out var channel);
        channel.SyncVolume(volumeCategory);
        return result;
    }

    internal static RESULT SyncVolume(this Channel channel, VolumeCategory volumeCategory = VolumeCategory.Master)
    {
        float x;
        switch (volumeCategory)
        {
            case VolumeCategory.Music:
                x = MusicVolume;
                break;
            case VolumeCategory.SFX:
                x = SfxVolume;
                break;
            case VolumeCategory.VO:
                x = VoVolume;
                break;
            case VolumeCategory.Master:
            default:
                x = MasterVolume;
                break;
        }

        return channel.setVolume(x);
    }

    internal static void IfErrorPrintWith(this RESULT result, string where)
    {
        if (result != RESULT.OK)
        {
            Plugin.Instance.Logger.LogError($"Sound Error: {result} caught at {where}");
        }
    }


    // Find file
    internal static string GetPath(string fileName)
    {
        var files = Directory.GetFiles(Paths.PluginPath, fileName, SearchOption.AllDirectories);

        switch (files.Length)
        {
            case 0:
                Plugin.Instance.Logger.LogError($"Error: Couldn't find \"{fileName}\"");
                return null;
            case > 1:
                Plugin.Instance.Logger.LogWarning(
                    $"More than one file named \"{fileName}\" found. This may lead to weird behavior.");
                break;
        }

        return files.First();
    }
}