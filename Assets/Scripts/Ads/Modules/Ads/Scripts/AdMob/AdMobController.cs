using UnityEngine;
//using GoogleMobileAds.Api;
using System;
using System.Collections;
using UnityEngine.Events;
using RocketTeam.Sdk.Services.Ads;

namespace G2.Sdk.Services.Ads
{
    public class AdMobController : MonoBehaviour
    {
//        public AdManager adsManager;

//        public bool isUseBannerAd;
//        public bool isUseInterstitalAd;
//        public bool isUseRewardedAd;
//        public AdMobUnit androidAdsUnit;
//        public AdMobUnit iosAdsUnit;
//        public AdPosition bannerPosition;

//        public int bannerAdsHeght;
//        private BannerView bannerView;
//        private InterstitialAd interstitial;
//        private RewardBasedVideoAd rewardBasedVideo;
//        private bool isBannerAdLoaded;
//        private bool isInterstitialAdLoading;
//        private bool isRewardedAdsLoaded;
//        private bool isRewardedAdLoading;
//        private bool isRewardedAdHaveEventHandler;
//        private int tryLoadBannerCount;
//        private int tryLoadVideoCount;
//        private int tryLoadInterstitialCount;

//        private UnityAction<AdEnums.BannerResponse> callbackBanner;

//        private int lastRewardValue = 0;
//        private const string idOB = "ca-app-pub-6336405384015455/9076600081";
//        private const string idNonOB_Old = "ca-app-pub-6336405384015455/4163055614";
//        float timeStop=0;
//        bool isInited = false;
//        private IEnumerator Start()
//        {
//            while (true)
//            {
//                yield return new WaitForSeconds(0.1f);
//                timeStop += 0.1f;
//                if (timeStop >= 3)
//                {
//                    yield break;
//                }
//                if (isInited)
//                {
//                    yield break;
//                }
//                if (!isInited && SpaceXServices.Instance != null && SpaceXServices.Instance.isLoadRemoteConfigSucces)
//                {
//                    isInited = true;
//                    try
//                    {
//                        if (androidAdsUnit != null)
//                        {
//                            androidAdsUnit.rewardedAdUnit = RocketRemoteConfig.GetStringConfig("rewardedAdUnit_Android", "ca-app-pub-6336405384015455/4163055614");
//                        }
//                        if (iosAdsUnit != null)
//                        {
//                            iosAdsUnit.rewardedAdUnit = RocketRemoteConfig.GetStringConfig("rewardedAdUnit_Ios", "ca-app-pub-6336405384015455/2423775567");
//                        }
//                    }
//                    catch
//                    {
//                        androidAdsUnit.rewardedAdUnit = "ca-app-pub-6336405384015455/4163055614";
//                        iosAdsUnit.rewardedAdUnit = "ca-app-pub-6336405384015455/2423775567";
//                    }

//#if UNITY_ANDROID
//                    string appId = androidAdsUnit.appId;
//                    if (androidAdsUnit.rewardedAdUnit.Equals(idOB))
//                    {
//                        Analytics.SetUserProperty("USE_OB", "Yes");
//                    }
//                    else if (androidAdsUnit.rewardedAdUnit.Equals(idNonOB_Old))
//                    {
//                        Analytics.SetUserProperty("USE_OB", "Old");
//                    }
//                    else
//                    {
//                        Analytics.SetUserProperty("USE_OB", "No");
//                    }
//#elif UNITY_IPHONE
//            string appId = iosAdsUnit.appId;
//#else
//            string appId = "unexpected_platform";
//#endif

//                    // Initialize the Google Mobile Ads SDK.
//                    MobileAds.Initialize(appId);

//                    InIt();
//                    yield break;
//                }
//            }
//        }



//        #region PUBLIC METHODS

//        public void InIt()
//        {
//            if (isUseBannerAd)
//            {
//                RequestBanner();
//            }

//            if (isUseInterstitalAd)
//            {
//                RequestInterstitial();
//            }

//            if (isUseRewardedAd)
//            {
//                RequestRewardBasedVideo();
//            }
//        }

//        public void ManualResetTryLoadCount()
//        {
//            tryLoadBannerCount = 3;
//            tryLoadInterstitialCount = 3;
//            tryLoadVideoCount = 0;
//        }

//        public bool IsRewardedAdsLoaded()
//        {
//            return isRewardedAdsLoaded;
//        }

//        public bool ShowBanerAd()
//        {
//            if (isBannerAdLoaded && bannerView != null)
//            {
//                bannerView.Show();
//                return true;
//            }
//            else
//            {
//                RequestBanner();
//                return false;
//            }
//        }

//        public void HideBannerAd()
//        {
//            if (bannerView != null)
//            {
//                bannerView.Hide();
//            }

//            RequestBanner();
//        }

//        public bool IsInitInterstitial()
//        {
//            if (interstitial != null && interstitial.IsLoaded())
//            {
//                return true;
//            }
//            else
//            {
//                return false;
//            }
//        }

//        public bool ShowInterstitialAd(int rewardValue)
//        {
//            if (interstitial != null && interstitial.IsLoaded())
//            {
//                this.lastRewardValue = rewardValue;
//                interstitial.Show();
//                return true;
//            }
//            else
//            {
//                RequestInterstitial();
//                return false;
//            }
//        }

//        public bool ShowRewardedVideoAd(int rewardValue)
//        {
//            if (rewardBasedVideo != null && rewardBasedVideo.IsLoaded())
//            {
//                this.lastRewardValue = rewardValue;
//                rewardBasedVideo.Show();
//                //ShowMediationTestSuite();
//                return true;
//            }
//            else
//            {
//                tryLoadVideoCount = 0;
//                RequestRewardBasedVideo();
//                return false;
//            }
//        }

//        #endregion


//        #region REQUEST ADS METHOD

//        /*
//         * *********************************************************************************
//         * REQUEST ADS METHOD
//         * *********************************************************************************
//         */

//        private void RequestBanner()
//        {
//            if (tryLoadBannerCount > 10)
//            {
//                return;
//            }

//#if UNITY_EDITOR
//            string adUnitId = "unused";
//#elif UNITY_ANDROID
//                string adUnitId = androidAdsUnit.bannerAdUnit;
//#elif UNITY_IPHONE
//                string adUnitId = iosAdsUnit.bannerAdUnit;
//#else
//                string adUnitId = "unexpected_platform";
//#endif

//            isBannerAdLoaded = false;
//            bannerView = new BannerView(adUnitId, AdSize.SmartBanner, bannerPosition);
//            bannerView.OnAdLoaded += HandleBannerAdLoaded;
//            bannerView.OnAdFailedToLoad += HandleBannerAdFailedToLoad;
//            bannerView.OnAdLoaded += HandleBannerAdOpened;
//            bannerView.OnAdClosed += HandleBannerAdClosed;
//            bannerView.OnAdLeavingApplication += HandleBannerAdLeftApplication;
//            AdRequest request = new AdRequest.Builder().Build();
//            bannerView.LoadAd(request);

//            bannerView.Hide();
//        }

//        public int GetBannerAdsHeight()
//        {
//            return AdSize.SmartBanner.Height;
//        }

//        public void RequestInterstitial()
//        {
//            if (isInterstitialAdLoading || tryLoadInterstitialCount > 3)
//            {
//                return;
//            }

//#if UNITY_ANDROID
//            string adUnitId = androidAdsUnit.interstitialAdUnit;
//#elif UNITY_IPHONE
//        string adUnitId = iosAdsUnit.interstitialAdUnit;
//#else
//            string adUnitId = "unexpected_platform";
//#endif

//            isInterstitialAdLoading = true;
//            interstitial = new InterstitialAd(adUnitId);
//            interstitial.OnAdLoaded += HandleInterstitialLoaded;
//            interstitial.OnAdFailedToLoad += HandleInterstitialFailedToLoad;
//            interstitial.OnAdOpening += HandleInterstitialOpened;
//            interstitial.OnAdClosed += HandleInterstitialClosed;
//            interstitial.OnAdLeavingApplication += HandleInterstitialLeftApplication;
//            AdRequest request = new AdRequest.Builder().Build();
//            interstitial.LoadAd(request);
//        }

//        public void RequestRewardBasedVideo()
//        {
//            if (isRewardedAdLoading || tryLoadVideoCount > 10)
//            {
//                return;
//            }

//#if UNITY_EDITOR
//            string adUnitId = "unused";
//#elif UNITY_ANDROID
//                string adUnitId = androidAdsUnit.rewardedAdUnit;
//#elif UNITY_IPHONE
//                string adUnitId = iosAdsUnit.rewardedAdUnit;
//#else
//                string adUnitId = "unexpected_platform";
//#endif
//            isRewardedAdLoading = true;
//            isRewardedAdsLoaded = false;
//            rewardBasedVideo = RewardBasedVideoAd.Instance;

//            if (!isRewardedAdHaveEventHandler)
//            {
//                // Ad event fired when the rewarded video ad has been received.
//                rewardBasedVideo.OnAdLoaded += HandleRewardBasedVideoLoaded;
//                // has failed to load.
//                rewardBasedVideo.OnAdFailedToLoad += HandleRewardBasedVideoFailedToLoad;
//                // is opened.
//                rewardBasedVideo.OnAdOpening += HandleRewardBasedVideoOpened;
//                // has started playing.
//                rewardBasedVideo.OnAdStarted += HandleRewardBasedVideoStarted;
//                // has rewarded the user.
//                rewardBasedVideo.OnAdRewarded += HandleRewardBasedVideoRewarded;
//                // is closed.
//                rewardBasedVideo.OnAdClosed += HandleRewardBasedVideoClosed;
//                // is leaving the application.
//                rewardBasedVideo.OnAdLeavingApplication += HandleRewardBasedVideoLeftApplication;

//                isRewardedAdHaveEventHandler = true;
//            }

//            AdRequest request = new AdRequest.Builder().Build();
//            rewardBasedVideo.LoadAd(request, adUnitId);


//            //RequestMediationAds();
//        }

//        #endregion


//        #region BANNER AD EVENTS

//        /*
//         * *********************************************************************************
//         * BANNER AD
//         * *********************************************************************************
//         */

//        private void HandleBannerAdLoaded(object sender, EventArgs e)
//        {
//            tryLoadBannerCount = 0;
//            isBannerAdLoaded = true;

//            if (callbackBanner != null)
//            {
//                callbackBanner.Invoke(AdEnums.BannerResponse.LOADED);
//            }
//        }

//        private void HandleBannerAdFailedToLoad(object sender, AdFailedToLoadEventArgs e)
//        {
//            if (callbackBanner != null)
//            {
//                callbackBanner.Invoke(AdEnums.BannerResponse.FAILED_TO_LOAD);
//            }

//            tryLoadBannerCount++;
//            RequestBanner();
//        }

//        private void HandleBannerAdOpened(object sender, EventArgs e)
//        {
//            if (callbackBanner != null)
//            {
//                callbackBanner.Invoke(AdEnums.BannerResponse.OPENING);
//            }
//        }

//        private void HandleBannerAdClosed(object sender, EventArgs e)
//        {
//            RequestBanner();
//            if (callbackBanner != null)
//            {
//                callbackBanner.Invoke(AdEnums.BannerResponse.CLOSED);
//            }
//        }

//        private void HandleBannerAdLeftApplication(object sender, EventArgs e)
//        {
//            if (callbackBanner != null)
//            {
//                callbackBanner.Invoke(AdEnums.BannerResponse.LEAVING_APP);
//            }
//        }

//        #endregion


//        #region INTERSTITIAL AD EVENTS

//        /*
//         * *********************************************************************************
//         * INTERSTITIAL AD
//         * *********************************************************************************
//         */

//        private void HandleInterstitialLoaded(object sender, EventArgs e)
//        {
//            Debug.Log("Load ads success");

//            tryLoadInterstitialCount = 0;
//            isInterstitialAdLoading = false;
//            if (adsManager.onLoaded != null)
//            {
//                adsManager.onLoaded();
//            }
//        }

//        private void HandleInterstitialFailedToLoad(object sender, AdFailedToLoadEventArgs e)
//        {
//            Debug.Log("Load ads failded");

//            tryLoadInterstitialCount++;
//            isInterstitialAdLoading = false;
//            RequestInterstitial();
//            if (adsManager.onFailedToLoad != null)
//            {
//                adsManager.onFailedToLoad("Admob Failed To Load" + e.Message);
//            }
//        }

//        private void HandleInterstitialOpened(object sender, EventArgs e)
//        {
//            if (adsManager.onOpening != null)
//            {
//                adsManager.onOpening();
//            }
//        }

//        private void HandleInterstitialClosed(object sender, EventArgs e)
//        {
//            isInterstitialAdLoading = false;
//            RequestInterstitial();
//            if (adsManager.onClosed != null)
//            {
//                adsManager.onClosed();
//            }
//        }

//        private void HandleInterstitialLeftApplication(object sender, EventArgs e)
//        {
//            RequestInterstitial();
//            if (adsManager.onLeavingApplication != null)
//            {
//                adsManager.onLeavingApplication();
//            }

//            if (adsManager.onGetReward != null)
//            {
//                adsManager.onGetReward(lastRewardValue);
//            }
//        }

//        #endregion


//        #region REWARDED VIDEO AD EVENTS

//        /*
//         * *********************************************************************************
//         * REWARDED VIDEO AD
//         * *********************************************************************************
//         */

//        private void HandleRewardBasedVideoLoaded(object sender, EventArgs e)
//        {
//            tryLoadVideoCount = 0;
//            isRewardedAdLoading = false;
//            isRewardedAdsLoaded = true;
//            //Debug.Log("Load xong");
//            if (adsManager.onLoaded != null)
//            {
//                adsManager.onLoaded();
//            }
//        }

//        private void HandleRewardBasedVideoFailedToLoad(object sender, AdFailedToLoadEventArgs e)
//        {
//            tryLoadVideoCount++;
//            isRewardedAdLoading = false;
//            isRewardedAdsLoaded = false;
//            RequestRewardBasedVideo();
//            //Debug.Log("Khong load duoc");
//            if (adsManager.onFailedToLoad != null)
//            {
//                adsManager.onFailedToLoad("Admob Failed To Load" + e.Message);
//            }
//        }

//        private void HandleRewardBasedVideoOpened(object sender, EventArgs e)
//        {
//            if (adsManager.onOpening != null)
//            {
//                adsManager.onOpening();
//            }

//#if UNITY_IOS
//                Time.timeScale = 0;
//#endif
//        }

//        private void HandleRewardBasedVideoStarted(object sender, EventArgs e)
//        {
//            //Debug.Log("Started");
//        }

//        private void HandleRewardBasedVideoRewarded(object sender, Reward e)
//        {
//            StartCoroutine(RaiseReward());
//        }

//        private IEnumerator RaiseReward()
//        {
//            yield return new WaitForSeconds(0.2f);

//            if (adsManager.onLeavingApplication != null)
//            {
//                adsManager.onLeavingApplication();
//            }

//            if (adsManager.onGetReward != null)
//            {
//                adsManager.onGetReward(lastRewardValue);
//            }
//        }

//        private void HandleRewardBasedVideoClosed(object sender, EventArgs e)
//        {
//#if UNITY_IOS
//                Time.timeScale = 1;
//#endif
//            isRewardedAdLoading = false;
//            RequestRewardBasedVideo();
//            if (adsManager.onClosed != null)
//            {
//                adsManager.onClosed();
//            }
//        }

//        private void HandleRewardBasedVideoLeftApplication(object sender, EventArgs e)
//        {
//            if (adsManager.onLeavingApplication != null)
//            {
//                adsManager.onLeavingApplication();
//            }

//            //            if (adsManager.onGetReward != null)
//            //            {
//            //                adsManager.onGetReward(lastRewardValue);
//            //            }
//        }

//        #endregion
    }
}