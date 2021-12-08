using System;
using GoogleMobileAds.Api;

public class AppOpenAdLauncher : Singleton<AppOpenAdLauncher>
{
    public void Init()
    {
        MobileAds.Initialize(status => { AppOpenAdManager.Instance.LoadAd(); });
    }

    private void OnApplicationPause(bool pause)
    {
        if (!pause && AppOpenAdManager.ConfigResumeApp && !AppOpenAdManager.ResumeFromAds)
        {
            AppOpenAdManager.Instance.ShowAdIfAvailable();
        }
    }

    public void TryGetAOA()
    {
        Invoke(nameof(GetAOA), AppOpenAdManager.TryGetAOAInterver);
    }

    void GetAOA()
    {
        AppOpenAdManager.Instance.LoadAd();
    }
}