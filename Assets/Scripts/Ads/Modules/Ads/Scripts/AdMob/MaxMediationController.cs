using RocketTeam.Sdk.Services.Ads;
using System;
using System.Collections;
using System.Collections.Generic;
using AppsFlyerSDK;
using UnityEngine;

public partial class MaxMediationController : MonoBehaviour
{
    public AdManager adsManager;

    private const string MaxSdkKey = "7PspscCcbGd6ohttmPcZTwGmZCihCW-Jwr7nSJN2a_9Mg0ERPs0tmGdKTK1gs__nr6XHQvK0vTNaTb1uR1mCIN";

    private string InterstitialAdUnitId;
    private string RewardedAdUnitId;
    private string BannerAdUnitId;

    private const string RewardedInterstitialAdUnitId = "ENTER_REWARD_INTER_AD_UNIT_ID_HERE";
    private const string MRecAdUnitId = "";


    private bool isBannerShowing;
    private bool isMRecShowing;

    private int interstitialRetryAttempt;
    private int rewardedRetryAttempt;
    private int rewardedInterstitialRetryAttempt;

    public TypeAdsMax TypeAdsUse;

    public void Init()
    {
        InterstitialAdUnitId = GameManager.Instance.GameSetting.InterAd;
        RewardedAdUnitId = GameManager.Instance.GameSetting.RewardedAd;
        BannerAdUnitId = GameManager.Instance.GameSetting.BannerAd;

        MaxSdkCallbacks.OnSdkInitializedEvent += sdkConfiguration =>
        {
            RegisterRevenuePaidCallback();

            // AppLovin SDK is initialized, configure and start loading ads.
            Debug.Log("MAX SDK Initialized");
            if (TypeAdsUse.HasFlag(TypeAdsMax.Inter))
                InitializeInterstitialAds();

            if (TypeAdsUse.HasFlag(TypeAdsMax.Reward))
                InitializeRewardedAds();

            if (TypeAdsUse.HasFlag(TypeAdsMax.Inter_Reward))
                InitializeRewardedInterstitialAds();

            if (TypeAdsUse.HasFlag(TypeAdsMax.Banner))
                InitializeBannerAds();

            if (TypeAdsUse.HasFlag(TypeAdsMax.MRec))
                InitializeMRecAds();

            //MaxSdk.ShowMediationDebugger();
        };
        MaxSdk.SetUserId(AppsFlyer.getAppsFlyerId());
        MaxSdk.SetSdkKey(MaxSdkKey);
        MaxSdk.InitializeSdk();
    }

    #region Interstitial Ad Methods

    private void InitializeInterstitialAds()
    {
        // Attach callbacks
        MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += OnInterstitialLoadedEvent;
        MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += OnInterstitialFailedEvent;
        MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent += OnInterstitialFailedToDisplayEvent;
        MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += OnInterstitialDismissedEvent;
        MaxSdkCallbacks.Interstitial.OnAdClickedEvent += OnInterstitialClickedEvent;
        MaxSdkCallbacks.Interstitial.OnAdDisplayedEvent += OnInterstitialDisplayedEvent;
        // Load the first interstitial
        LoadInterstitial();
    }

    public void LoadInterstitial()
    {
        //if (MaxSdk.IsInterstitialReady(InterstitialAdUnitId))
        //{
        //    return;
        //}

        MaxSdk.LoadInterstitial(InterstitialAdUnitId);
    }

    public void ShowInterstitial(string placement)
    {
        if (MaxSdk.IsInterstitialReady(InterstitialAdUnitId))
        {
            MaxSdk.ShowInterstitial(InterstitialAdUnitId, placement);
        }
    }

    public bool IsLoadInterstitial()
    {
        return MaxSdk.IsInterstitialReady(InterstitialAdUnitId);
    }

    private void OnInterstitialLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Reset retry attempt
        interstitialRetryAttempt = 0;
    }

    private void OnInterstitialFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo error)
    {
        // Interstitial ad failed to load. We recommend retrying with exponentially higher delays up to a maximum delay (in this case 64 seconds).
        interstitialRetryAttempt++;
        double retryDelay = Math.Pow(2, Math.Min(6, interstitialRetryAttempt));

        Invoke("LoadInterstitial", (float) retryDelay);
    }

    private void OnInterstitialFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
    {
        // Interstitial ad failed to display. We recommend loading the next ad
        //DebugCustom.Log("Interstitial failed to display with error code: " + errorInfo.Message);
        LoadInterstitial();
    }

    private void OnInterstitialDismissedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Interstitial ad is hidden. Pre-load the next ad
        //DebugCustom.Log("Interstitial dismissed");
        LoadInterstitial();
        ResetAds();
    }

    private void OnInterstitialClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Reset retry attempt
        interstitialRetryAttempt = 0;
        AppsFlyer.sendEvent("event_interstitial_ad_clicked", new Dictionary<string, string>() {{"interstitial_ad_clicked", adInfo.Placement}});
    }

    private void OnInterstitialDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Reset retry attempt
        //interstitialRetryAttempt = 0;  
        Debug.Log("InterstitialDisplayedEvent");
        AppsFlyer.sendEvent("event_interstitial_ad_impression", new Dictionary<string, string>() {{"event_interstitial_ad_impression", adInfo.Placement}});
    }

    #endregion

    #region Rewarded Ad Methods

    private void InitializeRewardedAds()
    {
        // Attach callbacks
        MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += OnRewardedAdLoadedEvent;
        MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += OnRewardedAdFailedEvent;
        MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += OnRewardedAdFailedToDisplayEvent;
        MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent += OnRewardedAdDisplayedEvent;
        MaxSdkCallbacks.Rewarded.OnAdClickedEvent += OnRewardedAdClickedEvent;
        MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += OnRewardedAdDismissedEvent;
        MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += OnRewardedAdReceivedRewardEvent;

        // Load the first RewardedAd
        LoadRewardedAd();
    }

    public void LoadRewardedAd()
    {
        MaxSdk.LoadRewardedAd(RewardedAdUnitId);
    }

    public bool IsLoadRewardedAd()
    {
        return MaxSdk.IsRewardedAdReady(RewardedAdUnitId);
    }

    public void ShowRewardedAd(string placeId)
    {
        if (MaxSdk.IsRewardedAdReady(RewardedAdUnitId))
        {
            MaxSdk.ShowRewardedAd(RewardedAdUnitId, placeId);
        }
    }


    private void OnRewardedAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Rewarded ad is ready to be shown. MaxSdk.IsRewardedAdReady(rewardedAdUnitId) will now return 'true'
        Debug.Log("Rewarded ad loaded");

        // Reset retry attempt
        rewardedRetryAttempt = 0;
    }

    private void OnRewardedAdFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        // Rewarded ad failed to load. We recommend retrying with exponentially higher delays up to a maximum delay (in this case 64 seconds).
        rewardedRetryAttempt++;
        double retryDelay = Math.Pow(2, Math.Min(6, rewardedRetryAttempt));

        Debug.Log("Rewarded ad failed to load with error code: " + errorInfo.Code);

        Invoke("LoadRewardedAd", (float) retryDelay);
    }

    private void OnRewardedAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
    {
        // Rewarded ad failed to display. We recommend loading the next ad
        Debug.Log("Rewarded ad failed to display with error code: " + errorInfo.Code);
        LoadRewardedAd();
    }

    private void OnRewardedAdDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        Debug.Log("Rewarded ad displayed");
    }

    private void OnRewardedAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        //DebugCustom.Log("Rewarded ad clicked");

        AppsFlyerSDK.AppsFlyer.sendEvent("event_video_reward_clicked", new Dictionary<string, string>() {{"clicked_video", adInfo.Placement}});
    }

    private void OnRewardedAdDismissedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Rewarded ad is hidden. Pre-load the next ad
        Debug.Log("Rewarded ad dismissed");
        LoadRewardedAd();
        ResetAds();
    }

    private void OnRewardedAdReceivedRewardEvent(string adUnitId, MaxSdkBase.Reward reward, MaxSdkBase.AdInfo adInfo)
    {
        // Rewarded ad was displayed and user should receive the reward
        //DebugCustom.Log("Rewarded ad received reward");
        if (adsManager.onGetReward != null)
        {
            adsManager.onGetReward(1);
        }

        //GameManager.Instance.Profile.SetNumberPlay(-1);
    }

    #endregion

    #region Rewarded Interstitial Ad Methods

    private void InitializeRewardedInterstitialAds()
    {
        // Attach callbacks
        //MaxSdkCallbacks.OnRewardedInterstitialAdLoadedEvent += OnRewardedInterstitialAdLoadedEvent;
        //MaxSdkCallbacks.OnRewardedInterstitialAdLoadFailedEvent += OnRewardedInterstitialAdFailedEvent;
        //MaxSdkCallbacks.OnRewardedInterstitialAdFailedToDisplayEvent += OnRewardedInterstitialAdFailedToDisplayEvent;
        //MaxSdkCallbacks.OnRewardedInterstitialAdDisplayedEvent += OnRewardedInterstitialAdDisplayedEvent;
        //MaxSdkCallbacks.OnRewardedInterstitialAdClickedEvent += OnRewardedInterstitialAdClickedEvent;
        //MaxSdkCallbacks.OnRewardedInterstitialAdHiddenEvent += OnRewardedInterstitialAdDismissedEvent;
        //MaxSdkCallbacks.OnRewardedInterstitialAdReceivedRewardEvent += OnRewardedInterstitialAdReceivedRewardEvent;

        // Load the first RewardedInterstitialAd
        LoadRewardedInterstitialAd();
    }

    public void LoadRewardedInterstitialAd()
    {
        if (MaxSdk.IsRewardedInterstitialAdReady(RewardedInterstitialAdUnitId))
        {
            return;
        }

        MaxSdk.LoadRewardedInterstitialAd(RewardedInterstitialAdUnitId);
    }

    public bool IsRewardedInterstitialAdReady()
    {
        return MaxSdk.IsRewardedInterstitialAdReady(RewardedInterstitialAdUnitId);
    }

    public void ShowRewardedInterstitialAd(string placeId)
    {
        if (MaxSdk.IsRewardedInterstitialAdReady(RewardedInterstitialAdUnitId))
        {
            MaxSdk.ShowRewardedInterstitialAd(RewardedInterstitialAdUnitId, placeId);
        }
    }

    private void OnRewardedInterstitialAdLoadedEvent(string adUnitId)
    {
        // Rewarded interstitial ad is ready to be shown. MaxSdk.IsRewardedInterstitialAdReady(rewardedInterstitialAdUnitId) will now return 'true'
        Debug.Log("Rewarded interstitial ad loaded");

        // Reset retry attempt
        rewardedInterstitialRetryAttempt = 0;
    }

    private void OnRewardedInterstitialAdFailedEvent(string adUnitId, int errorCode)
    {
        // Rewarded interstitial ad failed to load. We recommend retrying with exponentially higher delays up to a maximum delay (in this case 64 seconds).
        rewardedInterstitialRetryAttempt++;
        double retryDelay = Math.Pow(2, Math.Min(6, rewardedInterstitialRetryAttempt));
        Debug.Log("Rewarded interstitial ad failed to load with error code: " + errorCode);

        Invoke("LoadRewardedInterstitialAd", (float) retryDelay);
    }

    private void OnRewardedInterstitialAdFailedToDisplayEvent(string adUnitId, int errorCode)
    {
        // Rewarded interstitial ad failed to display. We recommend loading the next ad
        Debug.Log("Rewarded interstitial ad failed to display with error code: " + errorCode);
        LoadRewardedInterstitialAd();
    }

    private void OnRewardedInterstitialAdDisplayedEvent(string adUnitId)
    {
        Debug.Log("Rewarded interstitial ad displayed");
    }

    private void OnRewardedInterstitialAdClickedEvent(string adUnitId)
    {
        Debug.Log("Rewarded interstitial ad clicked");
    }

    private void OnRewardedInterstitialAdDismissedEvent(string adUnitId)
    {
        // Rewarded interstitial ad is hidden. Pre-load the next ad
        Debug.Log("Rewarded interstitial ad dismissed");
        LoadRewardedInterstitialAd();
    }

    private void OnRewardedInterstitialAdReceivedRewardEvent(string adUnitId, MaxSdk.Reward reward)
    {
        // Rewarded interstitial ad was displayed and user should receive the reward
        Debug.Log("Rewarded interstitial ad received reward");
        if (adsManager.onGetReward != null)
        {
            adsManager.onGetReward(1);
        }
    }

    #endregion

    #region Banner Ad Methods

    private void InitializeBannerAds()
    {
        // Banners are automatically sized to 320x50 on phones and 728x90 on tablets.
        // You may use the utility method `MaxSdkUtils.isTablet()` to help with view sizing adjustments.
        MaxSdk.CreateBanner(BannerAdUnitId, MaxSdkBase.BannerPosition.BottomCenter);
        MaxSdk.SetBannerExtraParameter(BannerAdUnitId, "adaptive_banner", "true");
        // Set background or background color for banners to be fully functional.
        MaxSdk.SetBannerBackgroundColor(BannerAdUnitId, new Color(0, 0, 0, 0.01f));
    }

    private void ToggleBannerVisibility()
    {
        if (!isBannerShowing)
        {
            MaxSdk.ShowBanner(BannerAdUnitId);
        }
        else
        {
            MaxSdk.HideBanner(BannerAdUnitId);
        }

        isBannerShowing = !isBannerShowing;
    }

    public bool ShowBanner()
    {
        MaxSdk.ShowBanner(BannerAdUnitId);
        return true;
    }

    public void HideBanner()
    {
        MaxSdk.HideBanner(BannerAdUnitId);
    }

    #endregion

    #region MREC Ad Methods

    private void InitializeMRecAds()
    {
        // MRECs are automatically sized to 300x250.
        MaxSdk.CreateMRec(MRecAdUnitId, MaxSdkBase.AdViewPosition.BottomCenter);
    }

    private void ToggleMRecVisibility()
    {
        if (!isMRecShowing)
        {
            MaxSdk.ShowMRec(MRecAdUnitId);
        }
        else
        {
            MaxSdk.HideMRec(MRecAdUnitId);
        }

        isMRecShowing = !isMRecShowing;
    }

    #endregion

    public void SetWatchingAds()
    {
        AppOpenAdManager.ResumeFromAds = true;
        Debug.Log("=> BEGIN WATCHING ADS");
    }

    public void ResetAds()
    {
        AppOpenAdManager.ResumeFromAds = false;
        Debug.Log("=> STOP WATCHING ADS");
    }
}

[Flags]
public enum TypeAdsMax
{
    Inter = 1 << 0,
    Banner = 1 << 1,
    Reward = 1 << 2,
    Inter_Reward = 1 << 3,
    MRec = 1 << 4
}