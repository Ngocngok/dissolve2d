using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class MaxMediationController : MonoBehaviour
{
    private void RegisterRevenuePaidCallback()
    {
        if (TypeAdsUse.HasFlag(TypeAdsMax.Inter))
            MaxSdkCallbacks.Interstitial.OnAdRevenuePaidEvent += OnInterstitialAdRevenuePaidEvent;
        if (TypeAdsUse.HasFlag(TypeAdsMax.Reward))
            MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += OnRewardedAdRevenuePaidEvent;
        if (TypeAdsUse.HasFlag(TypeAdsMax.Banner))
            MaxSdkCallbacks.Banner.OnAdRevenuePaidEvent += OnBannerAdRevenuePaidEvent;
    }

    private void OnInterstitialAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        double revenue = adInfo.Revenue;

        string countryCode = MaxSdk.GetSdkConfiguration().CountryCode;
        string networkName = adInfo.NetworkName;
        string adUnitIdentifier = adInfo.AdUnitIdentifier;
        string placement = adInfo.Placement;

        var data = new ImpressionData();
        data.AdFormat = "interstitial";
        data.AdUnitIdentifier = adUnitIdentifier;
        data.CountryCode = countryCode;
        data.NetworkName = networkName;
        data.Placement = placement;
        data.Revenue = revenue;

        AnalyticsRevenueAds.SendEvent(data, AdFormat.interstitial);
        Debug.Log("Inter Ad Revenue Paid");
    }

    private void OnBannerAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        double revenue = adInfo.Revenue;

        string countryCode = MaxSdk.GetSdkConfiguration().CountryCode;
        string networkName = adInfo.NetworkName;
        string adUnitIdentifier = adInfo.AdUnitIdentifier;
        string placement = adInfo.Placement;

        var data = new ImpressionData();
        data.AdFormat = "banner";
        data.AdUnitIdentifier = adUnitIdentifier;
        data.CountryCode = countryCode;
        data.NetworkName = networkName;
        data.Placement = placement;
        data.Revenue = revenue;

        AnalyticsRevenueAds.SendEvent(data, AdFormat.banner);
        Debug.Log("Banner Revenue Paid");
    }

    private void OnRewardedAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        double revenue = adInfo.Revenue;

        string countryCode = MaxSdk.GetSdkConfiguration().CountryCode;
        string networkName = adInfo.NetworkName;
        string adUnitIdentifier = adInfo.AdUnitIdentifier;
        string placement = adInfo.Placement;

        var data = new ImpressionData();
        data.AdFormat = "video_reward";
        data.AdUnitIdentifier = adUnitIdentifier;
        data.CountryCode = countryCode;
        data.NetworkName = networkName;
        data.Placement = placement;
        data.Revenue = revenue;

        AnalyticsRevenueAds.SendEvent(data, AdFormat.video_rewarded);
    }
}