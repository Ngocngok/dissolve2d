#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Facebook.Unity.Settings;
using GoogleMobileAds.Editor;
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

    [MenuItem("HyperCat Toolkit/Build Android/Verify Player Setting")]
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
        string defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android);
        if (!defines.Contains("FIREBASE"))
            defines += ";FIREBASE";
        PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, defines);
        PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, GameSetting.PackageName);
        PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);
        PlayerSettings.Android.bundleVersionCode = GameSetting.BundleVersion;
        PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel19;
        PlayerSettings.Android.targetSdkVersion = AndroidSdkVersions.AndroidApiLevel30;
        PlayerSettings.Android.targetArchitectures = AndroidArchitecture.All;
#elif UNITY_IOS
        string defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.iOS);
        if (!defines.Contains("FIREBASE"))
            defines += ";FIREBASE";
        PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.iOS, defines);
#endif
    }

    [MenuItem("HyperCat Toolkit/Build Android/Build APK (Testing)")]
    public static void BuildAPK()
    {
        GameSetting.BuildVersion += 1;
        EditorUtility.SetDirty(GameSetting);
        AssetDatabase.SaveAssets();
        VerifyAdsIds();
        ValidatePlayerSetting();

        PlayerSettings.Android.useCustomKeystore = false;

        EditorUserBuildSettings.development = false;
        EditorUserBuildSettings.allowDebugging = false;
        EditorUserBuildSettings.androidCreateSymbolsZip = false;
        EditorUserBuildSettings.buildAppBundle = false;

        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);

        var playerPath = GameSetting.BuildPath + string.Format("{0} {1}.apk", GameSetting.GameName, PlayerSettings.bundleVersion);
        BuildPipeline.BuildPlayer(GetScenePaths(), playerPath, BuildTarget.Android, BuildOptions.None);
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
        EditorUtility.SetDirty(GameSetting);
        AssetDatabase.SaveAssets();
        VerifyAdsIds();
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

    #region Third-party SDK

    [MenuItem("HyperCat Toolkit/Third-party Sdk/Verify Ads Ids")]
    public static void VerifyAdsIds()
    {
        FacebookSettings.AppIds[0] = GameSetting.FacebookAppID;
        FacebookSettings.AppLabels[0] = GameSetting.GameName;

        AppLovinSettings.Instance.AdMobAndroidAppId = GameSetting.AdmobAndroidID;
        GoogleMobileAdsSettings.Instance.GoogleMobileAdsAndroidAppId = GameSetting.AdmobAndroidID;

        EditorUtility.SetDirty(AppLovinSettings.Instance);
        EditorUtility.SetDirty(FacebookSettings.Instance);
        AssetDatabase.SaveAssets();
    }

    #endregion

    public static void ShowInspector()
    {
        EditorApplication.ExecuteMenuItem("Window/General/Inspector");
    }
}
#endif