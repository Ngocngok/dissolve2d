using System;

namespace RocketTeam.Sdk.Services.Interfaces
{
    public interface IBubbleAdManager
    {
        /// <summary>
        /// Register callback for Bubble Ad events
        /// </summary>
        void RegisterListener(Action onLoaded, Action<int, string> onFailedToLoad,
            Action onOpened, Action onClosed, Action onLeavingApplication);

        /// <summary>
        /// Load Bubble Ad data
        /// </summary>
        /// <param name="isRefresh">Ignore cache, get new data from server directly</param>
        void LoadBubbleAd(bool isRefresh = false);

        /// <summary>
        /// Show Bubble Ad
        /// </summary>
        /// <returns>Show successfully or not</returns>
        bool ShowBubbleAd();

        /// <summary>
        /// Hide Bubble Ad
        /// </summary>
        /// <returns>Hide successfully or not</returns>
        bool HideBubbleAd();

        /// <summary>
        /// Get current showing state of Bubble Ad
        /// </summary>
        /// <returns>Current showing state of More Games ads</returns>
        bool IsBubbleAdShowing();

        /// <summary>
        /// Get current loading state of Bubble Ad
        /// </summary>
        /// <returns>Current loading state of More Games ads</returns>
        bool IsBubbleAdLoading();

        /// <summary> 
        /// Notify IBubbleAdManager that the 'Back event' has occurred
        /// </summary>
        /// <returns>
        /// Return true: if IBubbleAdManager has already consumed 'Back event', you shouldn't process this event further.
        /// Return false: if IBubbleAdManager didn't comsume 'Back event', you can process this event by yourself.
        /// </returns>
        bool OnBack();
    }
}