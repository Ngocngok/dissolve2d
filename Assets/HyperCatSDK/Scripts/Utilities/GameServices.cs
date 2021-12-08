#if !PROTOTYPE
using Firebase;
using Firebase.Analytics;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using AppsFlyerSDK;
using Firebase.Crashlytics;
using UnityEditor;
using Firebase.Messaging;
using System.Threading.Tasks;
using RocketTeam.Sdk.Services.Ads;

#endif

public class GameServices : Singleton<GameServices>
{
#if !PROTOTYPE
    public FirebaseApp FirebaseApp;

    private bool firebaseInited = false;
    public bool FirebaseInited => firebaseInited && FirebaseApp != null;

    public string FirebaseMesssageToken = "";

    public FBManager fbManager;

    private void Start()
    {
        FirebaseMessaging.TokenRegistrationOnInitEnabled = true;
        InitFirebase();

        if (fbManager == null)
        {
            fbManager = new FBManager();
            fbManager.FirstInit();
        }

#if PROTOTYPE
        AppsFlyer.setIsDebug(true);
#else
        AppsFlyer.setIsDebug(false);
#endif
        AppsFlyer.initSDK(GameConst.APPFLYER_APP_KEY, "");
        AppsFlyer.startSDK();

        StartCoroutine(SyncWithGameManager());
    }

    IEnumerator SyncWithGameManager()
    {
        if (!GameManager.Instance.GameInited)
            yield return null;

        AdManager.Instance.Init();
        AppOpenAdLauncher.Instance.Init();
    }

    void InitFirebase()
    {
        FirebaseApp.CheckDependenciesAsync().ContinueWith(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                FirebaseApp = Firebase.FirebaseApp.DefaultInstance;
                SetupFirebase();
            }
            else
            {
                Debug.LogError(System.String.Format(
                    "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
            }
        });
    }

    private void OnMessageReceived(object sender, MessageReceivedEventArgs e)
    {
    }

    private void OnTokenReceived(object sender, TokenReceivedEventArgs e)
    {
        FirebaseMesssageToken = e.Token;
    }

    void SetupFirebase()
    {
        firebaseInited = true;

        RemoteConfigManager.Instance.StartAsync();

        HCDebug.Log("Firebase Inited Successfully!", HCColor.aqua);

        AnalyticManager.SetFirebaseUserProperties("last_login", DateTime.Now.DayOfYear.ToString());
        AnalyticManager.SetFirebaseUserProperties("app_version", HCTools.GameSetting.GameVersion);
        AnalyticManager.SetFirebaseUserProperties("current_level", GameManager.Instance.Data.User.Level.ToString());
        FirebaseAnalytics.SetSessionTimeoutDuration(new TimeSpan(0, 30, 0));

        FirebaseMessaging.TokenReceived += OnTokenReceived;
        FirebaseMessaging.MessageReceived += OnMessageReceived;
    }
#endif
}