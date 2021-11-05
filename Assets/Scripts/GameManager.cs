﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public GameData Data;
    public bool GameInited = false;

    private int secondToRemindComeback = 0;

    public void Start()
    {
        LoadGameData();

        SetupPushNotification();

        GUIManager.Instance.Init();

        Instance.GameInited = true;

        EventGlobalManager.Instance.OnUpdateSetting.Dispatch();

        SplashManager.Instance.FinishSplash();
    }

    private void LoadGameData()
    {
        Data = Database.LoadData();
        if (Data == null)
        {
            Data = new GameData();

#if PROTOTYPE
            Data.User.PurchasedNoAds = true;
#endif
            Database.SaveData();
        }
    }

    private void SetupPushNotification()
    {
        secondToRemindComeback = 60 * ConfigManager.Instance.Game.MaxOfflineRemindMinute + 30 * 60;

        if (Data.Setting.RequestedPN)
            SetupOfflinePN();
        else
            PushNotificationManager.Instance.StartRequest();
    }

    private void UpdateGraphicSetting()
    {
        if (Data.Setting.HighPerformance == 1)
        {
            Application.targetFrameRate = 60;
            Screen.SetResolution(Screen.width, Screen.height, true);
        }
        else
        {
            Application.targetFrameRate = 30;
            Screen.SetResolution(Screen.width / 2, Screen.height / 2, true);
        }
    }

    public override void OnApplicationQuit()
    {
        Logout();
        base.OnApplicationQuit();
    }

    public void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus)
        {
            Logout();
        }
    }

    public void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            Logout();
        }
    }

    public static bool NetworkAvailable => Application.internetReachability != NetworkReachability.NotReachable;

    private void Logout()
    {
        Data.User.LastTimeLogOut = DateTime.Now;
        Database.SaveData();
    }


    private void OnEnable()
    {
        EventGlobalManager.Instance.OnUpdateSetting.AddListener(UpdateGraphicSetting);
    }

    private void OnDestroy()
    {
        if (!EventGlobalManager.Instance)
            return;

        EventGlobalManager.Instance.OnUpdateSetting.RemoveListener(UpdateGraphicSetting);
    }

    public void SetupOfflinePN()
    {
        PushNotificationManager.Instance.CancelNotification(PushNotificationType.REMIND_COMEBACK);

        PushNotificationManager.Instance.ScheduleNotification(PushNotificationType.REMIND_COMEBACK,
            "More funny challenges ahead. Play now!", secondToRemindComeback);

        Database.SaveData();
    }

    public static void DestroyChildren(Transform trans)
    {
        if (trans.childCount > 0)
        {
            for (var i = trans.childCount - 1; i > -1; i--)
            {
                var obj = trans.GetChild(i).gameObject;
                obj.SetActive(false);
                Destroy(obj);
            }
        }
    }
}