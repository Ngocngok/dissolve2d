#if !PROTOTYPE
using AppsFlyerSDK;
using Firebase.Analytics;
#endif
using UnityEngine;

public class AnalyticManager : Singleton<AnalyticManager>
{
#if !PROTOTYPE
    #region Setup
    public static void SetFirebaseUserProperties(string name, string property)
    {
#if FIREBASE
        if (GameServices.Instance.FirebaseInited)
        {
            FirebaseAnalytics.SetUserProperty(name, property);
        }
#endif
    }

    public static void LogEvent(string eventName, params Parameter[] parameters)
    {
#if FIREBASE
        if (GameServices.Instance.FirebaseInited)
        {
            HCDebug.Log("<color=yellow>Firebase: " + eventName + "</color>");
            FirebaseAnalytics.LogEvent(eventName, parameters);
        }
#endif
    }

    public static void LogEvent(string eventName, string paramName, string paramValue)
    {
#if FIREBASE
        if (GameServices.Instance.FirebaseInited)
        {
            HCDebug.Log("<color=yellow>Firebase: " + eventName + "</color>");
            FirebaseAnalytics.LogEvent(eventName, paramName, paramValue);
        }
#endif
    }

    public static void LogEvent(string eventName)
    {
#if FIREBASE
        if (GameServices.Instance.FirebaseInited)
        {
            HCDebug.Log("<color=yellow>Firebase: " + eventName + "</color>");
            FirebaseAnalytics.LogEvent(eventName);
        }
#endif
    }
    #endregion

    #region ADS
    public static void M_LogClickVideo(string reason, string detail)
    {
        Parameter[] parameters = new Parameter[] {new Parameter(AnalyticParam.reason, reason), new Parameter(AnalyticParam.detail, detail)};
        LogEvent(AnalyticEvent.MONETIZE_REWARD_CLICK, parameters);
    }

    public static void M_LogNoReward(string reason, string detail)
    {
        Parameter[] parameters = new Parameter[] {new Parameter(AnalyticParam.reason, reason), new Parameter(AnalyticParam.detail, detail)};
        LogEvent(AnalyticEvent.MONETIZE_NO_REWARD, parameters);
    }

    public static void M_LogNoInterstitial(string reason, string detail)
    {
        Parameter[] parameters = new Parameter[] {new Parameter(AnalyticParam.reason, reason), new Parameter(AnalyticParam.detail, detail)};
        LogEvent(AnalyticEvent.MONETIZE_NO_INTERSTITIAL, parameters);
    }

    public static void M_LogRewardFailed(string reason, string detail)
    {
        Parameter[] parameters = new Parameter[] {new Parameter(AnalyticParam.reason, reason), new Parameter(AnalyticParam.detail, detail)};
        LogEvent(AnalyticEvent.MONETIZE_REWARD_FAILED, parameters);
    }

    public static void M_LogRewardSkipped(string reason, string detail)
    {
        Parameter[] parameters = new Parameter[] {new Parameter(AnalyticParam.reason, reason), new Parameter(AnalyticParam.detail, detail)};
        LogEvent(AnalyticEvent.MONETIZE_REWARD_SKIPPED, parameters);
    }

    public static void M_LogInterstitialFailed(string reason, string detail)
    {
        Parameter[] parameters = new Parameter[] {new Parameter(AnalyticParam.reason, reason), new Parameter(AnalyticParam.detail, detail)};
        LogEvent(AnalyticEvent.MONETIZE_INTERSTITIAL_FAILED, parameters);
    }

    public static void M_LogRewardShow(string reason, string detail)
    {
        Parameter[] parameters = new Parameter[] {new Parameter(AnalyticParam.reason, reason), new Parameter(AnalyticParam.detail, detail)};
        LogEvent(AnalyticEvent.MONETIZE_REWARD_SHOW, parameters);
    }

    public static void M_LogRewardSuccess(string reason, string detail)
    {
        Parameter[] parameters = new Parameter[] {new Parameter(AnalyticParam.reason, reason), new Parameter(AnalyticParam.detail, detail)};
        LogEvent(AnalyticEvent.MONETIZE_REWARD_SUCCESS, parameters);
    }

    public static void M_LogInterstitialSuccess(string reason, string detail)
    {
        Parameter[] parameters = new Parameter[] {new Parameter(AnalyticParam.reason, reason), new Parameter(AnalyticParam.detail, detail)};
        LogEvent(AnalyticEvent.MONETIZE_INTERSTITIAL_SUCCESS, parameters);
    }

    public static void M_LogRewardNoInternet(string reason, string detail)
    {
        Parameter[] parameters = new Parameter[] {new Parameter(AnalyticParam.reason, reason), new Parameter(AnalyticParam.detail, detail)};
        LogEvent(AnalyticEvent.MONETIZE_REWARD_NO_INTERNET, parameters);
    }

    public static void M_LogInterstitialNoInternet(string reason, string detail)
    {
        Parameter[] parameters = new Parameter[] {new Parameter(AnalyticParam.reason, reason), new Parameter(AnalyticParam.detail, detail)};
        LogEvent(AnalyticEvent.MONETIZE_INTERSTITIAL_NO_INTERNET, parameters);
    }

    public static void M_LogNoRewardNoInterstitial(string reason, string detail)
    {
        Parameter[] parameters = new Parameter[] {new Parameter(AnalyticParam.reason, reason), new Parameter(AnalyticParam.detail, detail)};
        LogEvent(AnalyticEvent.MONETIZE_NO_REWARD_NO_INTERSTITIAL, parameters);
    }
    #endregion

    #region LOG TOOL
    public static void LogWatchAds(string reason, long amount)
    {
        Parameter[] parameters = new Parameter[] {new Parameter(AnalyticParam.reason, reason), new Parameter(AnalyticParam.amount, amount)};

        LogEvent(AnalyticEvent.WATCH_ADS, parameters);
    }
    #endregion

    #region APPFLYER
    public void AppflyerLogIAPAndroid(string signature, string purchaseData, string price, string currency)
    {
#if UNITY_ANDROID
        AppsFlyerAndroid.validateAndSendInAppPurchase(GameConst.PUBLIC_KEY_APPSFLYER, signature, purchaseData, price, currency, null, this);
#endif
#if UNITY_EDITOR
        Debug.Log("<color=purple>Log IAP to Appflyer: " + price + "</color>");
#endif
    }
    #endregion

#endif
}