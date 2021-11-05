using RocketTeam.Sdk.Services.Ads;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoPubController : MonoBehaviour
{
//    public AdManager adsManager;
//    private bool IsInit;

//#if UNITY_IOS
//    private readonly string[] _bannerAdUnits =
//        { "0ac59b0996d947309c33f59d6676399f", "ef078b27e11c49bbb87080617a69b970", "2aae44d2ab91424d9850870af33e5af7" };

//    private readonly string[] _interstitialAdUnits =
//        { "4f117153f5c24fa6a3a92b818a5eb630", "9f2859c6726447aa9eaaa43a35ae8682" };

//    private readonly string[] _rewardedAdUnits =
//        { "8f000bd5e00246de9c789eed39ff6096", "98c29e015e7346bd9c380b1467b33850" };
//#elif UNITY_ANDROID || UNITY_EDITOR
//    //private readonly string bannerAdUnit = "3808072fa74a44e180f8964aaf5e8322";
//    //private readonly string interstitialAdUnit = "8d942ef4018b4bc68946dfb0649dc67f";
//    //private readonly string rewardedAdUnit = "6945d1f4f2574fe18ebeb2286078273c";

//    private readonly string[] _bannerAdUnits = { "3808072fa74a44e180f8964aaf5e8322" };
//    private readonly string[] _interstitialAdUnits = { "8d942ef4018b4bc68946dfb0649dc67f" };
//    private readonly string[] _rewardedAdUnits =
//        { "6945d1f4f2574fe18ebeb2286078273c"};
//#endif

//    private int interstitialRetryAttempt;
//    private int rewardedRetryAttempt;

//    private void Start()
//    {

//        MoPub.LoadBannerPluginsForAdUnits(_bannerAdUnits);
//        MoPub.LoadInterstitialPluginsForAdUnits(_interstitialAdUnits);
//        MoPub.LoadRewardedVideoPluginsForAdUnits(_rewardedAdUnits);
//    }
//    // Start is called before the first frame update
//    public void Init()
//    {
//        IsInit = true;
//        InitializeInterstitialAds();
//        InitializeBannerAds();
//        InitializeRewardedInterstitialAds();
//    }



//    #region Interstitial Ad Methods

//    private void InitializeInterstitialAds()
//    {
//        // Attach callbacks
//        MoPubManager.OnInterstitialLoadedEvent += OnInterstitialLoadedEvent;
//        MoPubManager.OnInterstitialFailedEvent += OnInterstitialFailedEvent;
//        MoPubManager.OnInterstitialDismissedEvent += OnInterstitialDismissedEvent;

//        // Load the first interstitial
//        LoadInterstitial();
//    }

//    public void LoadInterstitial()
//    {
//        MoPub.RequestInterstitialAd(_interstitialAdUnits[0]);
//    }

//    public void ShowInterstitial()
//    {
//        if (IsLoadInterstitial())
//            MoPub.ShowInterstitialAd(_interstitialAdUnits[0]);
//    }

//    public bool IsLoadInterstitial()
//    {
//        return MoPub.IsInterstitialReady(_interstitialAdUnits[0]);
//    }

//    private void OnInterstitialLoadedEvent(string adUnitId)
//    {
//        // Reset retry attempt
//        interstitialRetryAttempt = 0;
//    }

//    private void OnInterstitialFailedEvent(string adUnitId, string errorCode)
//    {
//        // Interstitial ad failed to load. We recommend retrying with exponentially higher delays up to a maximum delay (in this case 64 seconds).
//        interstitialRetryAttempt++;
//        double retryDelay = Math.Pow(2, Math.Min(6, interstitialRetryAttempt));

//        Invoke("LoadInterstitial", (float)retryDelay);
//    }

//    private void OnInterstitialDismissedEvent(string adUnitId)
//    {
//        // Interstitial ad is hidden. Pre-load the next ad
//        DebugCustom.Log("Interstitial dismissed");
//        LoadInterstitial();
//    }

//    #endregion

//    #region Banner Ad
//    private void InitializeBannerAds()
//    {
//        MoPub.RequestBanner(_bannerAdUnits[0], MoPub.AdPosition.BottomCenter);

//    }


//    public bool ShowBanner()
//    {
//        if (!IsInit)
//            return false;

//        MoPub.ShowBanner(_bannerAdUnits[0], true);
//        return true;
//    }

//    public void HideBanner()
//    {
//        MoPub.ShowBanner(_bannerAdUnits[0], false);
//    }
//    #endregion

//    #region Reward Ads
//    private void InitializeRewardedInterstitialAds()
//    {
//        MoPubManager.OnRewardedVideoLoadedEvent += OnRewardedVideoLoadedEvent;
//        MoPubManager.OnRewardedVideoFailedEvent += OnRewardedVideoFailedEvent;
//        MoPubManager.OnRewardedVideoFailedToPlayEvent += OnRewardedVideoFailedToPlayEvent;
//        MoPubManager.OnRewardedVideoClosedEvent += OnRewardedVideoClosedEvent;
//        MoPubManager.OnRewardedVideoReceivedRewardEvent += OnRewardedVideoReceivedRewardEvent;
//        LoadRewardedVideo();
//    }


//    private void LoadRewardedVideo()
//    {
//        MoPub.RequestRewardedVideo(_rewardedAdUnits[0]);
//    }

//    public void ShowRewardVideo(string placeId)
//    {
//        if (MoPub.HasRewardedVideo(_rewardedAdUnits[0]))
//        {
//            MoPub.ShowRewardedVideo(_rewardedAdUnits[0], placeId);
//        }
//    }

//    public bool IsLoadRewardedVideo()
//    {
//        return MoPub.HasRewardedVideo(_rewardedAdUnits[0]);
//    }

//    private void OnRewardedVideoLoadedEvent(string adUnitId)
//    {
//        rewardedRetryAttempt = 0;
//    }


//    private void OnRewardedVideoFailedEvent(string adUnitId, string error)
//    {
//        // AdFailed(adUnitId, "load rewarded video", error);
//        rewardedRetryAttempt++;
//        double retryDelay = Math.Pow(2, Math.Min(6, rewardedRetryAttempt));

//        Invoke("LoadRewardedVideo", (float)retryDelay);
//    }


//    private void OnRewardedVideoFailedToPlayEvent(string adUnitId, string error)
//    {
//        // AdFailed(adUnitId, "play rewarded video", error);
//        rewardedRetryAttempt++;
//        double retryDelay = Math.Pow(2, Math.Min(6, rewardedRetryAttempt));

//        Invoke("LoadRewardedVideo", (float)retryDelay);
//    }


//    private void OnRewardedVideoClosedEvent(string adUnitId)
//    {
//        // _demoGUI.AdDismissed(adUnitId);
//        LoadRewardedVideo();
//    }


//    private void OnRewardedVideoReceivedRewardEvent(string adUnitId, string label, float amount)
//    {
//        if (adsManager.onGetReward != null)
//        {
//            adsManager.onGetReward(1);
//        }

//        GameManager.Instance.Profile.SetNumberPlay(-1);
//    }

//    #endregion
}
