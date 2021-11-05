using System;

namespace RocketTeam.Sdk.Services.Interfaces
{
    public interface IMoreGamesManager : IG2Service
    {
        /// <summary>
        /// Register callback for More Games events
        /// </summary>
        void RegisterListener(Action onLoaded, Action<int, string> onFailedToLoad,
            Action onOpened, Action onClosed, Action onLeavingApplication, Action<int> onGetReward);

        /// <summary>
        /// Load More Games data
        /// </summary>
        /// <param name="offset">Start of More Games data that you want to get from server (for pagination purpose)</param>
        /// <param name="limit">Number of More Games data that you want to get from server (for pagination purpose)</param>
        /// <param name="isRefresh">If true, ignore cache, get new data from server directly</param>
        void LoadMoreGames(int offset = 0, int limit = 0, bool isRefresh = false);

        /// <summary>
        /// Show More Games
        /// </summary>
        /// <returns>Show successfully or not</returns>
        bool ShowMoreGames();

        /// <summary>
        /// Hide More Games
        /// </summary>
        /// <returns>Hide successfully or not</returns>
        bool HideMoreGames();

        /// <summary>
        /// Get current showing state of More Games ads
        /// </summary>
        /// <returns>Current showing state of More Games ads</returns>
        bool IsMoreGamesShowing();

        /// <summary>
        /// Get current loading state of More Games ads
        /// </summary>
        /// <returns>Current loading state of More Games ads</returns>
        bool IsMoreGamesLoading();

        /// <summary> 
        /// Notify IMoreGamesManager that the 'Back event' has occurred
        /// </summary>
        /// <returns>
        /// Return true: if IMoreGamesManager has already consumed 'Back event', you shouldn't process this event further.
        /// Return false: if IMoreGamesManager didn't comsume 'Back event', you can process this event by yourself.
        /// </returns>
        bool OnBack();
    }
}