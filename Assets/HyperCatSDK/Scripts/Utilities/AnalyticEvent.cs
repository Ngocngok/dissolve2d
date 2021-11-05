public class AnalyticEvent
{
    public static string WATCH_ADS = "watch_ads";
    public static string FAILED_VIDEO = "failed_video";
    public static string NO_VIDEO = "no_video";
    public static string NO_INTERNET_VIDEO = "no_internet_video";
    public static string INTERSTITIAL_VIDEO = "interstitial_video";
    public static string SHOW_IAP = "show_iap";
    public static string CLICK_INAPP = "click_inapp";
    public static string INAPP_PURCHASE = "inapp_purchase";
    public static string INAPP_FAILED = "inapp_failed";
    public static string INAPP_SUCCESSED = "inapp_successed";

    public static string M_IAP_PURCHASE_CLICK = "iap_purchase_click";
    public static string M_IAP_PURCHASE_PROCESSING = "iap_purchase_processing";
    public static string M_IAP_PURCHASE_NO_INTERNET = "iap_purchase_no_internet";
    public static string M_IAP_PURCHASE_SUCCESSED = "iap_purchase_successed";
    public static string M_IAP_PURCHASE_CANCELLED = "iap_purchase_cancelled";

    public static string CLICK_VIDEO = "click_video";
    public static string COMPLETED_VIDEO = "completed_video";
    public static string MONETIZE_REWARD_CLICK = "monetize_reward_click";
    public static string MONETIZE_NO_REWARD = "monetize_no_reward";
    public static string MONETIZE_NO_INTERSTITIAL = "monetize_no_interstitial";
    public static string MONETIZE_REWARD_FAILED = "monetize_reward_failed";
    public static string MONETIZE_REWARD_SKIPPED = "monetize_reward_skipped";
    public static string MONETIZE_INTERSTITIAL_FAILED = "monetize_interstitial_failed";
    public static string MONETIZE_REWARD_SHOW = "monetize_reward_show";
    public static string MONETIZE_REWARD_SUCCESS = "monetize_reward_success";
    public static string MONETIZE_INTERSTITIAL_SUCCESS = "monetize_interstitial_success";
    public static string IAP_PURCHASE_SUCCESS = "iap_purchase_success";
    public static string IAP_PURCHASE_CANCELLED = "iap_purchase_cancelled";
    public static string MONETIZE_REWARD_NO_INTERNET = "monetize_reward_no_internet";
    public static string MONETIZE_INTERSTITIAL_NO_INTERNET = "monetize_interstitial_no_internet";
    public static string MONETIZE_NO_REWARD_NO_INTERSTITIAL = "monetize_no_reward_no_interstitial";
    public static string MONETIZE_REWARD_CAPPED = "monetize_reward_capped";
    public static string MONETIZE_INTERSTITIAL_CAPPED = "monetize_interstitial_capped";
}

public class AnalyticParam
{
    public static string reason = "Reason";
    public static string amount = "Amount";
    public static string currency = "Currency";

    public static string level = "level";

    public static string id_pack = "id_pack";

    public static string detail = "detail";
}

public static class AnalyticEnum
{
    public enum Action
    {
        Iap,
        BuyShopPack,
        BuyShopDailyBonus,
        BuyBlackMarket,
        TradeFromDiamond
    }
}

public static class UserProperties
{
    public static string VERSION_INSTALL = "version_install";

    public static string GEM = "gem";

    public static string TIME_PLAY = "time_play";
    public static string DAY_PLAY = "day_play";

    public static string UserID = "user_profile_id";
}