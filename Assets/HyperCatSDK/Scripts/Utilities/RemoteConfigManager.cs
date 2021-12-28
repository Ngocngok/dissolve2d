using Firebase.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class RemoteConfigManager : Singleton<RemoteConfigManager>
{
    public bool RemoteConfigReady = false;

    private bool fetcheCompleted = false;

    public float min_value_revenue = 0.1f;
    public int config_max_day_send_revenue = 1;

    public bool AdsEnabled = true;

    public void StartAsync()
    {
        var defaults = new Dictionary<string, object>();
        defaults.Add("min_value_revenue", min_value_revenue);
        defaults.Add("config_max_day_send_revenue", config_max_day_send_revenue);
        defaults.Add("AppOpenAds", true);
        defaults.Add("AppResumeAds", true);
        defaults.Add("WaterfallTierCount", 4);
        defaults.Add("TestFillAOA", false);
        defaults.Add("AdsEnabled", AdsEnabled);
        defaults.Add("TryGetAOATime", 10);

        Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.SetDefaultsAsync(defaults)
            .ContinueWithOnMainThread(task =>
            {
                StartCoroutine(WaitForAsync());
                fetcheCompleted = true;
            });
    }

    IEnumerator WaitForAsync()
    {
        while (!fetcheCompleted)
            yield return null;

        FetchDataAsync();
    }

    public Task FetchDataAsync()
    {
        Task fetchTask =
            Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.FetchAsync(
                TimeSpan.Zero);
        return fetchTask.ContinueWithOnMainThread(FetchComplete);
    }

    void FetchComplete(Task fetchTask)
    {
        if (fetchTask.IsCanceled)
        {
            Debug.Log("Fetch canceled.");
        }
        else if (fetchTask.IsFaulted)
        {
            Debug.Log("Fetch encountered an error.");
        }
        else if (fetchTask.IsCompleted)
        {
            Debug.Log("Fetch completed successfully!");

            AppOpenAdManager.ConfigOpenApp = Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance
                .GetValue("AppOpenAds").BooleanValue;
            Debug.Log("AppOpenAds " + AppOpenAdManager.ConfigOpenApp);

            AppOpenAdManager.ConfigResumeApp = Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance
                .GetValue("AppResumeAds").BooleanValue;
            Debug.Log("AppResumeAds " + AppOpenAdManager.ConfigResumeApp);

            config_max_day_send_revenue = (int) Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance
                .GetValue("config_max_day_send_revenue").LongValue;
            Debug.Log("config_max_day_send_revenue " + config_max_day_send_revenue);

            min_value_revenue = (float) Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance
                .GetValue("min_value_revenue").DoubleValue;
            Debug.Log("min_value_revenue " + min_value_revenue);

            AppOpenAdManager.WaterfallTierCount = (int) Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance
                .GetValue("WaterfallTierCount").LongValue;
            Debug.Log("WaterfallTierCount " + AppOpenAdManager.WaterfallTierCount);

            AppOpenAdManager.TestFillAOA = Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance
                .GetValue("TestFillAOA").BooleanValue;
            Debug.Log("TestFillAOA " + AppOpenAdManager.TestFillAOA);

            AdsEnabled = Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance
                .GetValue("AdsEnabled").BooleanValue;
            Debug.Log("AdsEnabled " + AdsEnabled);

            AppOpenAdManager.TryGetAOATime = (int) Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance
                .GetValue("TryGetAOATime").LongValue;
            Debug.Log("TryGetAOATime " + AppOpenAdManager.TryGetAOATime);

            RemoteConfigReady = true;
        }

        var info = Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.Info;
        switch (info.LastFetchStatus)
        {
            case Firebase.RemoteConfig.LastFetchStatus.Success:
                Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.ActivateAsync()
                    .ContinueWithOnMainThread(task =>
                    {
                        Debug.Log(String.Format("Remote data loaded and ready (last fetch time {0}).",
                            info.FetchTime));
                    });

                break;
            case Firebase.RemoteConfig.LastFetchStatus.Failure:
                switch (info.LastFetchFailureReason)
                {
                    case Firebase.RemoteConfig.FetchFailureReason.Error:
                        Debug.Log("Fetch failed for unknown reason");
                        break;
                    case Firebase.RemoteConfig.FetchFailureReason.Throttled:
                        Debug.Log("Fetch throttled until " + info.ThrottledEndTime);
                        break;
                }

                break;
            case Firebase.RemoteConfig.LastFetchStatus.Pending:
                Debug.Log("Latest Fetch call still pending.");
                break;
        }
    }
}