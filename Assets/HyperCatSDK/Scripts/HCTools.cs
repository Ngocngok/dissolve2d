#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Facebook.Unity.Settings;
using UnityEditor;
using UnityEditor.Build.Content;
using UnityEngine;

public class HCTools : Editor
{
    public static HCGameSetting GameSetting;

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
        Selection.activeObject = GetGameSetting();
        EditorApplication.ExecuteMenuItem("Window/General/Inspector");
    }

    #region Build

    [MenuItem("HyperCat Toolkit/Build Android/Validate Player Setting")]
    public static void ValidatePlayerSetting()
    {
        if (GameSetting == null)
            GameSetting = GetGameSetting();

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
        if (GameSetting == null)
            GameSetting = GetGameSetting();
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

        if (GameSetting == null)
            GameSetting = GetGameSetting();
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

    #region UI Tools
    [MenuItem("HyperCat Toolkit/UI/New Screen")]
    public static void CreateNewScreen()
    {
    }

    #endregion
}
#endif