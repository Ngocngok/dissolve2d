using System;

namespace RocketTeam.Sdk.Services.Interfaces
{
    public interface IAdsManager : IG2Service
    {
        #region Interstitial
        /// <summary>
        /// Register callback for Interstitial ads events
        /// </summary>
        void RegisterInterstitialListener(Action onOpened, Action onClosed,
            Action onClicked, Action<int> onGetReward);

        /// <summary>
        /// Load Interstitial ads
        /// </summary>
        /// <param name="adsId">Id of Interstitial ads you want to load</param>
        /// <param name="isRefresh">If true, ignore cache, get new data from server directly</param>
        void LoadInterstitial(int adsId = 1, bool isRefresh = false);

        /// <summary>
        /// Show Interstitial ads
        /// </summary>
        /// <param name="adsId">Id of Interstitial ads you want to show</param>
        /// <returns>Show successfully or not</returns>
        bool ShowInterstitial(string _placement, int adsId = 1);
        bool ShowInterstitial(string _placement, int adsId = 1, Action onOpened = null, Action onClosed = null, Action onClicked = null, Action<int> onGetReward = null);

        /// <summary>
        /// Hide Interstitial Ads
        /// </summary>
        /// <param name="adsId">Id of Interstitial ads you want to hide</param>
        /// <returns>Hide successfully or not</returns>
        bool HideInterstitial(int adsId = 1);

        /// <summary>
        /// Get current load state of Interstitial ads
        /// </summary>
        /// <param name="adsId">Id of Interstitial ads you want to check</param>
        /// <returns>Current load state of Interstitial ads</returns>
        bool IsInterstitialLoaded(int adsId = 1);

        /// <summary> 
        /// Notify IAdsManager that the 'Back event' has occurred
        /// </summary>
        /// <returns>
        /// Return true: if IAdsManager has already consumed 'Back event', you shouldn't process this event further.
        /// Return false: if IAdsManager didn't comsume 'Back event', you can process this event by yourself.
        /// </returns>
        bool OnBack();
        RewardAdStatus ShowAdsReward(Action<int> OnRewarded, string _placement, bool isAutoLog = true);
        #endregion

        #region Banner
        /// <summary>
        /// Register callback for Banner ads life cycle events
        /// </summary>
        void RegisterBannerListener(Action onOpened, Action onClosed, Action onLeavingApplication);

        /// <summary>
        /// Show Banner ads
        /// </summary>
        /// <param name="bannerId">Id of Banner ads you want to show</param>
        /// <returns>Show successfully or not</returns>
        bool ShowBanner(int bannerId = 1);

        /// <summary>
        /// Hide Banner ads
        /// </summary>
        /// <param name="bannerId">Id of Banner ads you want to hide</param>
        /// <returns>hide successfully or not</returns>
        bool HideBanner(int bannerId = 1);

        /// <summary>
        /// Get width of Banner ads
        /// </summary>
        /// <returns>Width of Banner ads in pixels</returns>
        int GetBannerWidth();

        /// <summary>
        /// Get height of Banner ads
        /// </summary>
        /// <returns>Height of Banner ads in pixels</returns>
        int GetBannerHeight();
        #endregion
    }
}