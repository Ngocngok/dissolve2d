using System.Collections;
using System.Collections.Generic;
#if UNITY_ANDROID
using Unity.Notifications.Android;
#endif
using System;
#if UNITY_IOS
using Unity.Notifications.iOS;
#endif
using UnityEngine;

public class PushNotificationManager : Singleton<PushNotificationManager>
{
    public void StartRequest()
    {
        GameManager.Instance.Data.Setting.RequestedPN = true;
        Database.SaveData();

        RequestAuthorization();
    }

    #region Register

    void RequestAuthorization()
    {
#if UNITY_IOS
        StartCoroutine(RequestAuthorizationForiOS());
#endif

#if UNITY_ANDROID
        RequestNotificationChannelForAndroid();
#endif

        HCDebug.Log("PSN >Request for Push Notification", HCColor.orange);
    }

#if UNITY_IOS
    IEnumerator RequestAuthorizationForiOS()
    {
        var authorizationOption = AuthorizationOption.Alert | AuthorizationOption.Badge | AuthorizationOption.Sound;
        using (var req = new AuthorizationRequest(authorizationOption, true))
        {
            while (!req.IsFinished)
            {
                yield return null;
            }

            string res = "\n RequestAuthorization:";
            res += "\n finished: " + req.IsFinished;
            res += "\n granted :  " + req.Granted;
            res += "\n error:  " + req.Error;
            res += "\n deviceToken:  " + req.DeviceToken;
            Debug.Log(res);

            if(req.Granted)
            {
                GameManager.Instance.Data.EnablePN = true;
                Database.SaveData();

                GameManager.Instance.SetupOfflinePN();
            }
        }
    }
#endif

#if UNITY_ANDROID
    void RequestNotificationChannelForAndroid()
    {
        var c = new AndroidNotificationChannel()
        {
            Id = "GameNotification", Name = "HyperCat Channel", Importance = Importance.High, Description = "Generic notifications",
        };
        AndroidNotificationCenter.RegisterNotificationChannel(c);
        GameManager.Instance.Data.Setting.EnablePN = true;
        Database.SaveData();

        GameManager.Instance.SetupOfflinePN();
    }
#endif

    #endregion

    #region Schedule A Notification

    public void ScheduleNotification(PushNotificationType type, string content, int timer)
    {
        if (!GameManager.Instance.Data.Setting.EnablePN)
            return;

        HCDebug.Log("PSN >Set Notification: " + content + " - After " + timer + " secs", HCColor.orange);

        var triggerTime = DateTime.Now.AddSeconds(timer);

#if UNITY_IOS
        var iOSTrigger = new iOSNotificationCalendarTrigger()
        {
            Hour = triggerTime.Hour, Minute = triggerTime.Minute, Second = triggerTime.Second, Repeats = false
        };

        var iOSNotification = new iOSNotification()
        {
            Identifier = type.ToString(),
            Body = content,
            Subtitle = "",
            ShowInForeground = true,
            ForegroundPresentationOption = (PresentationOption.Alert | PresentationOption.Sound),
            CategoryIdentifier = "TaskNotification",
            ThreadIdentifier = "DefaultThread",
            Trigger = iOSTrigger,
        };

        iOSNotificationCenter.ScheduleNotification(iOSNotification);
#endif

#if UNITY_ANDROID
        var androidNotification = new AndroidNotification();
        androidNotification.Text = content;
        androidNotification.FireTime = triggerTime;
        androidNotification.SmallIcon = "icon_0";
        androidNotification.LargeIcon = "icon_1";

        var newIndex = AndroidNotificationCenter.SendNotification(androidNotification, "GameNotification");
        if (!GameManager.Instance.Data.Setting.AndroidPNIndexes.ContainsKey(type))
        {
            GameManager.Instance.Data.Setting.AndroidPNIndexes.Add(type, newIndex);
            Database.SaveData();
        }
#endif
    }

    #endregion

    public void CancelNotification(PushNotificationType noti)
    {
#if UNITY_IOS
        iOSNotificationCenter.RemoveScheduledNotification(noti.ToString());
#endif
#if UNITY_ANDROID
        if (!GameManager.Instance.Data.Setting.AndroidPNIndexes.ContainsKey(noti))
            return;

        int index = GameManager.Instance.Data.Setting.AndroidPNIndexes[noti];
        AndroidNotificationCenter.CancelNotification(index);
        GameManager.Instance.Data.Setting.AndroidPNIndexes.Remove(noti);
#endif
    }
}

public enum PushNotificationType
{
    NONE,
    REMIND_COMEBACK,
    TOTAL
}