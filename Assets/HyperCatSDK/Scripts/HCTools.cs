﻿#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Facebook.Unity.Settings;
using UnityEditor;
using UnityEditor.Build.Content;
using UnityEngine;

public class HCTools : Editor
{
    public static HCGameSetting GameSetting
    {
        get
        {
            if (gameSetting == null)
                gameSetting = GetGameSetting();

            return gameSetting;
        }
    }

    private static HCGameSetting gameSetting;

    public static HCGameSetting GetGameSetting()
    {
        var path = "Assets/HyperCatSDK/";
        var fileEntries = Directory.GetFiles(path);
        for (int i = 0; i < fileEntries.Length; i++)
        {
            if (fileEntries[i].EndsWith(".asset"))
            {
                var item =
                    AssetDatabase.LoadAssetAtPath<HCGameSetting>(fileEntries[i].Replace("\\", "/"));
                if (item != null)
                {
                    return item;
                }
            }
        }

        return null;
    }

    [MenuItem("HyperCat Toolkit/Edit Game Setting")]
    public static void EditGameSetting()
    {
        Selection.activeObject = GameSetting;
        ShowInspector();
    }

    #region Build

    [MenuItem("HyperCat Toolkit/Build Android/Validate Player Setting")]
    public static void ValidatePlayerSetting()
    {
        PlayerSettings.companyName = "HyperCat";
        if (Application.HasProLicense())
            PlayerSettings.SplashScreen.showUnityLogo = false;
        PlayerSettings.bundleVersion = string.Format("{0}.{1}.{2}", GameSetting.GameVersion, GameSetting.BundleVersion, GameSetting.BuildVersion);

#if UNITY_ANDROID || UNITY_IOS
        PlayerSettings.defaultInterfaceOrientation = UIOrientation.Portrait;
#endif

#if UNITY_ANDROID
        PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, GameSetting.PackageName);
        PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);
        PlayerSettings.Android.bundleVersionCode = GameSetting.BundleVersion;
        PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel19;
        PlayerSettings.Android.targetSdkVersion = AndroidSdkVersions.AndroidApiLevel30;
        PlayerSettings.Android.targetArchitectures = AndroidArchitecture.All;
#endif
    }

    [MenuItem("HyperCat Toolkit/Build Android/Build APK (Testing)")]
    public static void BuildAPK()
    {
        GameSetting.BuildVersion += 1;
        ValidatePlayerSetting();

        PlayerSettings.Android.useCustomKeystore = false;

        EditorUserBuildSettings.development = false;
        EditorUserBuildSettings.allowDebugging = false;
        EditorUserBuildSettings.androidCreateSymbolsZip = false;
        EditorUserBuildSettings.buildAppBundle = false;

        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);

        var playerPath = GameSetting.BuildPath + string.Format("{0} {1}.apk", GameSetting.GameName, PlayerSettings.bundleVersion);
        // BuildPipeline.BuildPlayer(GetScenePaths(), playerPath, BuildTarget.Android, BuildOptions.None);
    }

    [MenuItem("HyperCat Toolkit/Build Android/Build AAB (Submit)")]
    public static void BuildAAB()
    {
        PlayerSettings.Android.useCustomKeystore = true;
        if (string.IsNullOrEmpty(PlayerSettings.keyaliasPass))
        {
            EditorUtility.DisplayDialog("HyperCat Warning", "Publishing Setting - Verify your keystore setting before performing a submit build!", "Yes, sir!");
            SettingsService.OpenProjectSettings("Project/Player");
            return;
        }

        GameSetting.BuildVersion += 1;
        GameSetting.BundleVersion += 1;
        ValidatePlayerSetting();

        EditorUserBuildSettings.development = false;
        EditorUserBuildSettings.allowDebugging = false;
        EditorUserBuildSettings.androidCreateSymbolsZip = false;
        EditorUserBuildSettings.buildAppBundle = true;

        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);

        var buildPath = "D:/HyperCat Build/";
        var playerPath = buildPath + string.Format("{0} {1}.aab", GameSetting.GameName, PlayerSettings.bundleVersion);
        BuildPipeline.BuildPlayer(GetScenePaths(), playerPath, BuildTarget.Android, BuildOptions.None);
    }

    private static string[] GetScenePaths()
    {
        var scenes = new string[EditorBuildSettings.scenes.Length];
        for (var i = 0; i < scenes.Length; i++)
        {
            scenes[i] = EditorBuildSettings.scenes[i].path;
        }

        return scenes;
    }

    #endregion

    #region Configs

    [MenuItem("HyperCat Toolkit/Configs/Game Config")]
    public static void ShowGameConfig()
    {
        var cfg = GetGameConfig();
        if (cfg == null)
        {
            cfg = ScriptableObject.CreateInstance<GameConfig>();
            UnityEditor.AssetDatabase.CreateAsset(cfg, "Assets/Configs/GameConfig.asset");
            UnityEditor.AssetDatabase.SaveAssets();
        }

        cfg = GetGameConfig();
        Selection.activeObject = cfg;
        ShowInspector();
    }

    public static GameConfig GetGameConfig()
    {
        var path = "Assets/Configs/";
        var fileEntries = Directory.GetFiles(path);
        for (int i = 0; i < fileEntries.Length; i++)
        {
            if (fileEntries[i].EndsWith(".asset"))
            {
                var item =
                    AssetDatabase.LoadAssetAtPath<GameConfig>(fileEntries[i].Replace("\\", "/"));
                if (item != null)
                {
                    return item;
                }
            }
        }

        return null;
    }

    [MenuItem("HyperCat Toolkit/Configs/Sound Config")]
    public static void ShowSoundConfig()
    {
        var cfg = GetSoundConfig();
        if (cfg == null)
        {
            cfg = ScriptableObject.CreateInstance<SoundConfig>();
            AssetDatabase.CreateAsset(cfg, "Assets/Configs/SoundConfig.asset");
            AssetDatabase.SaveAssets();
        }

        cfg = GetSoundConfig();
        Selection.activeObject = cfg;
        ShowInspector();
    }

    public static SoundConfig GetSoundConfig()
    {
        var path = "Assets/Configs/";
        var fileEntries = Directory.GetFiles(path);
        for (int i = 0; i < fileEntries.Length; i++)
        {
            if (fileEntries[i].EndsWith(".asset"))
            {
                var item =
                    AssetDatabase.LoadAssetAtPath<SoundConfig>(fileEntries[i].Replace("\\", "/"));
                if (item != null)
                {
                    return item;
                }
            }
        }

        return null;
    }

    #endregion

    #region UI Tools

    [MenuItem("HyperCat Toolkit/UI/New Screen")]
    public static void CreateNewScreen()
    {
    }

    #endregion

    public static void ShowInspector()
    {
        EditorApplication.ExecuteMenuItem("Window/General/Inspector");
    }
}
#endif