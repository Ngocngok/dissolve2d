using System;
using System.Collections;
using System.Linq;
using GoogleMobileAds.Api;
using UnityEngine;

public class AppOpenAdManager
{
    private static AppOpenAdManager instance;

    private AppOpenAd ad;

    private DateTime loadTime;

    private bool isShowingAd = false;

    private bool showFirstOpen = false;

    public static bool ConfigOpenApp = true;
    public static bool ConfigResumeApp = true;

    public static int WaterfallTierCount = 4;
    public static bool TestFillAOA = false;

    public static bool ResumeFromAds = false;

    public static int TryGetAOATime = -1;

    public static AppOpenAdManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new AppOpenAdManager();
            }

            return instance;
        }
    }

    private bool IsAdAvailable => ad != null && (System.DateTime.UtcNow - loadTime).TotalHours < 4;

    private int tierIndex = 1;

    public void LoadAd()
    {
        if (!GameManager.EnableAds)
            return;

        LoadAOA();
    }

    public void LoadAOA()
    {
        string id = "";
        if (tierIndex > GameManager.Instance.GameSetting.AOAListIds.Count)
            id = GameManager.Instance.GameSetting.AOAListIds.Last();
        else
            id = GameManager.Instance.GameSetting.AOAListIds[tierIndex - 1];

        Debug.Log("Start request Open App Ads Tier " + tierIndex);

        AdRequest request = new AdRequest.Builder().Build();

        AppOpenAd.LoadAd(id, ScreenOrientation.Portrait, request, ((appOpenAd, error) =>
        {
            if (error != null)
            {
                // Handle the error.
                Debug.LogFormat("Failed to load the ad. (reason: {0}), tier {1}", error.LoadAdError.GetMessage(), tierIndex);
                tierIndex++;
                if (tierIndex <= (TestFillAOA ? WaterfallTierCount + 1 : WaterfallTierCount))
                    LoadAOA();
                else
                {
                    tierIndex = 1;
                    AppOpenAdLauncher.Instance.TryGetAOA();
                    showFirstOpen = true;
                }

                return;
            }

            // App open ad is loaded.
            ad = appOpenAd;
            tierIndex = 1;
            loadTime = DateTime.UtcNow;
            if (!showFirstOpen && ConfigOpenApp)
            {
                ShowAdIfAvailable();
                showFirstOpen = true;
            }
        }));
    }

    public void ShowAdIfAvailable()
    {
        if (!GameManager.EnableAds)
            return;

        if (!IsAdAvailable || isShowingAd)
        {
            return;
        }

        ad.OnAdDidDismissFullScreenContent += HandleAdDidDismissFullScreenContent;
        ad.OnAdFailedToPresentFullScreenContent += HandleAdFailedToPresentFullScreenContent;
        ad.OnAdDidPresentFullScreenContent += HandleAdDidPresentFullScreenContent;
        ad.OnAdDidRecordImpression += HandleAdDidRecordImpression;
        ad.OnPaidEvent += HandlePaidEvent;

        ad.Show();
    }

    private void HandleAdDidDismissFullScreenContent(object sender, EventArgs args)
    {
        Debug.Log("Closed app open ad");
        // Set the ad to null to indicate that AppOpenAdManager no longer has another ad to show.
        ad = null;
        isShowingAd = false;
        LoadAd();
    }

    private void HandleAdFailedToPresentFullScreenContent(object sender, AdErrorEventArgs args)
    {
        Debug.LogFormat("Failed to present the ad (reason: {0})", args.AdError.GetMessage());
        // Set the ad to null to indicate that AppOpenAdManager no longer has another ad to show.
        ad = null;
        LoadAd();
    }

    private void HandleAdDidPresentFullScreenContent(object sender, EventArgs args)
    {
        Debug.Log("Displayed app open ad");
        isShowingAd = true;
    }

    private void HandleAdDidRecordImpression(object sender, EventArgs args)
    {
        Debug.Log("Recorded ad impression");
    }

    private void HandlePaidEvent(object sender, AdValueEventArgs args)
    {
        Debug.LogFormat("Received paid event. (currency: {0}, value: {1}",
            args.AdValue.CurrencyCode, args.AdValue.Value);
    }
}