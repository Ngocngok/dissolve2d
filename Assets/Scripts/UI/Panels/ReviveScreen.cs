#if !PROTOTYPE
using RocketTeam.Sdk.Services.Ads;
#endif
using UnityEngine;
using UnityEngine.UI;

public class ReviveScreen : UIPanel
{
    public override UI_PANEL GetID()
    {
        return UI_PANEL.ContinueScreen;
    }

    public static ReviveScreen Instance;

    [SerializeField]
    private Button ReviveBtn;

    [SerializeField]
    private Button RetryBtn;

    [SerializeField]
    private Button SkipBtn;

    [SerializeField]
    private Image timeRemainBar;

    public static void Show(bool canRevive = true)
    {
        ReviveScreen newInstance = (ReviveScreen) GUIManager.Instance.NewPanel(UI_PANEL.ContinueScreen);
        Instance = newInstance;
        newInstance.OnAppear(canRevive);
    }

    public void OnAppear(bool canRevive = true)
    {
        Root.enabled = true;
        if (isInited)
            return;

        base.OnAppear();

        Init(canRevive);
    }

    private float remain = 5f;

    void Init(bool canRevive = true)
    {
        if (canRevive)
        {
            remain = 5f;
            timeRemainBar.fillAmount = 1f;
            RetryBtn.gameObject.SetActive(false);
            ReviveBtn.gameObject.SetActive(true);
            SkipBtn.gameObject.SetActive(false);
        }
        else
        {
            remain = -1f;
            timeRemainBar.fillAmount = 0f;
            RetryBtn.gameObject.SetActive(true);
            ReviveBtn.gameObject.SetActive(false);
            SkipBtn.gameObject.SetActive(true);
        }

        // AudioAssistant.Shot(TYPE_SOUND.DEFEAT);
#if !PROTOTYPE
        AnalyticManager.LogEvent("Milk_lose_level", "level", GM.Data.User.Level.ToString());
#endif
    }

    void Update()
    {
        if (remain > 0f)
        {
            remain -= Time.deltaTime;
            timeRemainBar.fillAmount = (remain / 5f);
        }
        else
        {
            RetryBtn.gameObject.SetActive(true);
            ReviveBtn.gameObject.SetActive(false);
            SkipBtn.gameObject.SetActive(true);
        }
    }

    public void SkipLevel()
    {
        AudioAssistant.Shot(TYPE_SOUND.BUTTON);
        if (!GameManager.NetworkAvailable)
        {
            PopupNoInternet.Show();
            return;
        }

        if (!GameManager.EnableAds)
            OnCompleteAdsSkip(1);
#if !PROTOTYPE
        else
            AdManager.Instance.ShowAdsReward(OnCompleteAdsSkip, "SkipLevel");
#endif
    }

    void OnCompleteAdsSkip(int result)
    {
        GM.Data.User.Level++;

        Database.SaveData();

        Invoke(nameof(BackToMain), 0.5f);
    }

    void ProgressNext()
    {
        Root.enabled = false;
    }

    bool HasRewardToUnlock()
    {
        return true;
    }

    void BackToMain()
    {
        if (HasRewardToUnlock())
            ProgressNext();
    }

    public void Retry()
    {
        AudioAssistant.Shot(TYPE_SOUND.BUTTON);
        if (!GameManager.NetworkAvailable)
        {
            PopupNoInternet.Show();
            return;
        }

#if !PROTOTYPE
        if (GameManager.EnableAds)
            AdManager.Instance.ShowInterstitial("Retry", 1);
#endif
        MainScreen.Show();
    }

    public void Revive()
    {
        AudioAssistant.Shot(TYPE_SOUND.BUTTON);
        if (!GameManager.NetworkAvailable)
        {
            PopupNoInternet.Show();
            return;
        }

        if (!GameManager.EnableAds)
            OnCompleteAds(1);
#if !PROTOTYPE
        else
            AdManager.Instance.ShowAdsReward(OnCompleteAds, "Revive");
#endif
    }

    void OnCompleteAds(int result)
    {
        PlayScreen.Show();
    }

    protected override void RegisterEvent()
    {
        base.RegisterEvent();
        ReviveBtn.onClick.AddListener(Revive);
        RetryBtn.onClick.AddListener(Retry);
        SkipBtn.onClick.AddListener(SkipLevel);
    }

    protected override void UnregisterEvent()
    {
        base.UnregisterEvent();
        ReviveBtn.onClick.RemoveListener(Revive);
        RetryBtn.onClick.RemoveListener(Retry);
        SkipBtn.onClick.RemoveListener(SkipLevel);
    }

    public override void OnDisappear()
    {
        base.OnDisappear();
        Instance = null;
    }
}