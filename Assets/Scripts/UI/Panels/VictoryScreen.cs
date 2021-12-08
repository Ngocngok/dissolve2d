using HyperCatSdk;
using MoreMountains.NiceVibrations;
#if !PROTOTYPE
using RocketTeam.Sdk.Services.Ads;
#endif
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VictoryScreen : UIPanel
{
    public override UI_PANEL GetID()
    {
        return UI_PANEL.VictoryScreen;
    }

    public static VictoryScreen Instance;

    public static void Show()
    {
        VictoryScreen newInstance = (VictoryScreen) GUIManager.Instance.NewPanel(UI_PANEL.VictoryScreen);
        Instance = newInstance;
        newInstance.OnAppear();
    }

    [SerializeField]
    private Button claimBtn;

    [SerializeField]
    private Button claimAdsBtn;

    [SerializeField]
    private TMP_Text rewardLb;

    [SerializeField]
    private TMP_Text myGoldLb;

    [SerializeField]
    private UILabel levelLb;

    [SerializeField]
    private ParticleSystem coinFx;

    [SerializeField]
    private ParticleSystem winFx;

    public void OnAppear()
    {
        Root.enabled = true;
        if (isInited)
            return;

        base.OnAppear();

        Init();

        // LevelController.Instance.ShowFx();
        // AudioAssistant.Shot(TYPE_SOUND.VICTORY);
        timer = 3f;
    }

    private int rewardGold;
    private int currentGold;

    void Init()
    {
        currentGold = GM.Data.User.Money;

        rewardGold = 100 + 25 * GM.Data.User.Level;
        levelLb.text = "Level " + GM.Data.User.Level;

        rewardLb.text = rewardGold.ToFormatString();
        myGoldLb.text = currentGold.ToFormatString();
        gotReward = false;
        claimBtn.gameObject.SetActive(false);
        coinFx.Stop();

        HCVibrate.Haptic(HapticTypes.Success);
#if !PROTOTYPE
        AnalyticManager.LogEvent("win_level", "level", GM.Data.User.Level.ToString());
#endif
    }

    private float timer = 3f;

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0f && !gotReward)
            claimBtn.gameObject.SetActive(true);
    }

    private bool gotReward = false;

    void Claim()
    {
        AudioAssistant.Shot(TYPE_SOUND.BUTTON);
        if (gotReward)
            return;

        if (!GameManager.NetworkAvailable)
        {
            PopupNoInternet.Show();
            return;
        }

        gotReward = true;

        GM.Data.User.Level++;
        //GM.Data.User.UnlockProgress += 20;

        GM.Data.User.Money += rewardGold;
        Database.SaveData();
        myGoldLb.ChangeText(currentGold, GM.Data.User.Money, 1f);

#if !PROTOTYPE
        if (GameManager.EnableAds)
            AdManager.Instance.ShowInterstitial("ClaimNormal", 1);
#endif

        DelayBackToMain();
        HCVibrate.Haptic(HapticTypes.SoftImpact);
    }

    void ClaimAds()
    {
        AudioAssistant.Shot(TYPE_SOUND.BUTTON);
        if (gotReward)
            return;

        if (!GameManager.NetworkAvailable)
        {
            PopupNoInternet.Show();
            return;
        }

        if (!GameManager.EnableAds)
            OnCompleteAds(1);
#if !PROTOTYPE
        else
            AdManager.Instance.ShowAdsReward(OnCompleteAds, "ClaimX5");
#endif
    }

    void OnCompleteAds(int result)
    {
        gotReward = true;
        GM.Data.User.Level++;
        // GM.Data.UnlockProgress += 20;

        GM.Data.User.Money += rewardGold * 5;
        Database.SaveData();
        myGoldLb.ChangeText(currentGold, GM.Data.User.Money, 1f);
        DelayBackToMain();
        HCVibrate.Haptic(HapticTypes.SoftImpact);
    }

    bool HasRewardToUnlock()
    {
        return false;
    }

    void ProgressNext()
    {
        Root.enabled = false;
    }

    void DelayBackToMain()
    {
        coinFx.Play();
        Invoke(nameof(BackToMain), 1.5f);
    }

    void BackToMain()
    {
        coinFx.Stop();
        if (HasRewardToUnlock())
            ProgressNext();
        else
        {
            // LevelController.Instance.ResetLevel(true);
            MainScreen.Show();
        }
    }

    protected override void RegisterEvent()
    {
        base.RegisterEvent();
        claimBtn.onClick.AddListener(Claim);
        claimAdsBtn.onClick.AddListener(ClaimAds);
    }

    protected override void UnregisterEvent()
    {
        base.UnregisterEvent();
        claimBtn.onClick.RemoveListener(Claim);
        claimAdsBtn.onClick.RemoveListener(ClaimAds);
    }

    public override void OnDisappear()
    {
        coinFx.Stop();
        base.OnDisappear();
        Instance = null;
    }
}