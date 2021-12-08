using System.Collections;
using System.Collections.Generic;
#if !PROTOTYPE
using RocketTeam.Sdk.Services.Ads;
#endif
using UnityEngine;
using UnityEngine.UI;

public class MainScreen : UIPanel
{
    public override UI_PANEL GetID()
    {
        return UI_PANEL.MainScreen;
    }

    public static MainScreen Instance;

    [SerializeField]
    private GameObject canBuySkinNotice;

    [SerializeField]
    public GameObject btnNoAds;

    [SerializeField]
    private UILabel myGoldLb;

    [SerializeField]
    private UILabel levelLb;

    public static void Show()
    {
        MainScreen newInstance = (MainScreen) GUIManager.Instance.NewPanel(UI_PANEL.MainScreen);
        Instance = newInstance;
        newInstance.OnAppear();
    }

    public void OnAppear()
    {
        if (isInited)
            return;

        base.OnAppear();

#if !PROTOTYPE
        if (GameManager.EnableAds)
            AdManager.Instance.ShowBanner();
#endif

        Init();
    }

    public void CheckNotice()
    {
    }

    void Init()
    {
        CheckNotice();

        levelLb.text = "Level " + GM.Data.User.Level;

        UpdateMoney();

        CheckOfferNoAds();
    }

    private void CheckOfferNoAds()
    {
        btnNoAds.SetActive(GameManager.EnableAds);
    }

    public void UpdateMoney()
    {
        myGoldLb.text = GM.Data.User.Money.ToFormatString();
    }

    public void ShowSetting()
    {
        AudioAssistant.Shot(TYPE_SOUND.BUTTON);
        PopupSetting.Show();
    }

    public void ShowSkin()
    {
        if (!GameManager.NetworkAvailable)
        {
            PopupNoInternet.Show();
            return;
        }

        AudioAssistant.Shot(TYPE_SOUND.BUTTON);
        // PopupShop.Show();
    }

    public void ShowWeapon()
    {
        if (!GameManager.NetworkAvailable)
        {
            PopupNoInternet.Show();
            return;
        }

        AudioAssistant.Shot(TYPE_SOUND.BUTTON);
        // PopupShop.Show(false);
    }

    public void StartGame()
    {
        AudioAssistant.Shot(TYPE_SOUND.BUTTON);

        if (!GameManager.NetworkAvailable)
        {
            PopupNoInternet.Show();
            return;
        }

        PlayScreen.Show();
    }

    public void NoAds()
    {
        AudioAssistant.Shot(TYPE_SOUND.BUTTON);
#if !PROTOTYPE
        IAPManager.Instance.BuyProduct(GameConst.NO_ADS_ID);
#endif
    }

    protected override void RegisterEvent()
    {
        base.RegisterEvent();
        EventGlobalManager.Instance.OnPurchaseNoAds.AddListener(CheckOfferNoAds);
    }

    protected override void UnregisterEvent()
    {
        base.UnregisterEvent();
        EventGlobalManager.Instance.OnPurchaseNoAds.RemoveListener(CheckOfferNoAds);
    }

    public override void OnDisappear()
    {
        base.OnDisappear();
        Instance = null;
    }
}