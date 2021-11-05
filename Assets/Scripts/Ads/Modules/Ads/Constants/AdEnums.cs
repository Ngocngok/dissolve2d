using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RocketTeam.Sdk.Services.Ads
{
    public class AdEnums
    {
        public enum OS
        {
            NONE = 0,
            ANDROID = 1,
            IOS = 2
        }

        public enum AdLogStatus
        {
            VIEW_ONLY = 1,
            CLICK = 2
        }
        public enum Orientation
        {
            UNKNOWN = 0,
            PORTRAIT = 1,
            LANDSCAPE = 2
        }
        public enum ShowType
        {
            NO_AD = 0,
            INTERSTITIAL = 1,
            INTERSTITIAL_REWARD = 4,
            VIDEO_REWARD = 2,
            BANNER = 3

        }

        public enum ImageType
        {
            MORE_GAME_BACKGROUND = 0,
            MORE_GAME_ICON = 1,
            POPUP = 2,
            BUBBLE = 3
        }

        public enum ScreenRatio
        {
            UNKNOW = 0,
            RATIO_169 = 1,
            RATIO_43 = 2
        }

        public enum VideoRewardResponse
        {
            LOADED,
            FAILED_TO_LOAD,
            OPENING,
            STARTED,
            REWARDED,
            CLOSED,
            LEAVING_APP
        }

        public enum BannerResponse
        {
            LOADED,
            FAILED_TO_LOAD,
            OPENING,
            CLOSED,
            LEAVING_APP
        }
    }
}
