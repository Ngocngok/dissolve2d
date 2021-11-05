using DG.Tweening;
using MoreMountains.NiceVibrations;
#if !PROTOTYPE
using RocketTeam.Sdk.Services.Ads;
#endif
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupUnlockProgress : UIPanel
{
    public override UI_PANEL GetID()
    {
        return UI_PANEL.PopupUnlockProgress;
    }

    public static PopupUnlockProgress Instance;

    [SerializeField]
    private Button CloseButton;

    [SerializeField]
    private Button UnlockBtn;

    [SerializeField]
    private Image skinBar;

    [SerializeField]
    private GameObject skinGrp;

    [SerializeField]
    private TMP_Text progressLb;

    [SerializeField]
    private UILabel itemNameLb;

    [SerializeField]
    private GameObject unlocked;

    [SerializeField]
    private SkinController characterSkin;

    [SerializeField]
    private GameObject bgUnlocked;

    public static void Show()
    {
        PopupUnlockProgress newInstance = (PopupUnlockProgress) GUIManager.Instance.NewPanel(UI_PANEL.PopupUnlockProgress);
        Instance = newInstance;
        newInstance.OnAppear();
    }

    public void OnAppear()
    {
        if (isInited)
            return;

        base.OnAppear();

        Init();
    }

    private SkinConfig skinConfig;
    private bool isReady = false;

    void Init()
    {
        // isReady = false;
        // bgUnlocked.SetActive(false);
        // unlocked.SetActive(false);
        // UnlockBtn.gameObject.SetActive(false);
        // CloseButton.gameObject.SetActive(false);
        // float oldP = (GM.Data.UnlockProgress - 20) / 100f;
        // float newP = GM.Data.UnlockProgress / 100f;
        //
        // skinConfig = Cfg.GetSkinConfig(GM.Data.NextUnlockSkin);
        // itemNameLb.text = skinConfig.Name;
        // characterSkin.gameObject.SetActive(false);
        // skinGrp.SetActive(true);
        //
        // skinBar.fillAmount = oldP;
        // skinBar.DOFillAmount(newP, 1f).onComplete = () =>
        // {
        //     if (GM.Data.UnlockProgress >= 100)
        //     {
        //         UnlockBtn.gameObject.SetActive(true);
        //         characterSkin.gameObject.SetActive(true);
        //         characterSkin.Init(skinConfig.ID);
        //         HCVibrate.Haptic(HapticTypes.Success);
        //         AudioAssistant.Shot(TYPE_SOUND.UNLOCK);
        //         bgUnlocked.SetActive(true);
        //         isReady = true;
        //         skinGrp.SetActive(false);
        //         GM.Data.UnlockProgress = 0;
        //         Database.SaveData();
        //     }
        //
        //     CloseButton.gameObject.SetActive(true);
        // };
        //
        // progressLb.text = GM.Data.UnlockProgress + "%";
    }

    public override void Close()
    {
        base.Close();
        // LevelController.Instance.ResetLevel(true);
        MainScreen.Show();
    }

    void UnlockNow()
    {
        AudioAssistant.Shot(TYPE_SOUND.BUTTON);
        BuySkinAds();
    }

    void BuySkinAds()
    {
//         if (GM.Data.PurchasedNoAds)
//             OnBuySkin(1);
// #if !PROTOTYPE
//         else
//             AdManager.Instance.ShowAdsReward(OnBuySkin, "BuySkinAtUnlockPopup");
// #endif
    }

    void OnBuySkin(int result)
    {
        // if (!GM.Data.SkinUnlockData.ContainsKey(skinConfig.ID))
        //     GM.Data.SkinUnlockData.Add(skinConfig.ID, skinConfig.AdsCost);
        // else
        //     GM.Data.SkinUnlockData[skinConfig.ID] += skinConfig.AdsCost;
        //
        // if (GM.Data.SkinUnlockData[skinConfig.ID] >= skinConfig.AdsCost)
        // {
        //     GM.Data.MySkin = skinConfig.ID;
        //
        //     Player.Instance.UpdateSkin();
        //     UnlockBtn.gameObject.SetActive(false);
        //     unlocked.SetActive(true);
        // }
        //
        // AudioAssistant.Shot(TYPE_SOUND.BUY_SKIN);
        // HCVibrate.Haptic(HapticTypes.Success);
        // Database.SaveData();
    }

    void Skip()
    {
        if (isReady)
        {
            // GM.Data.SkippedSkins.Add(GM.Data.NextUnlockSkin);
            // GM.Data.NextUnlockSkin = GM.Data.GetNextUnlockSkin();
            // Database.SaveData();
        }

        Close();
    }

    protected override void RegisterEvent()
    {
        base.RegisterEvent();
        CloseButton.onClick.AddListener(Skip);
        UnlockBtn.onClick.AddListener(UnlockNow);
    }

    protected override void UnregisterEvent()
    {
        base.UnregisterEvent();
        CloseButton.onClick.RemoveListener(Skip);
        UnlockBtn.onClick.RemoveListener(UnlockNow);
    }

    public override void OnDisappear()
    {
        base.OnDisappear();
        Instance = null;
    }
}