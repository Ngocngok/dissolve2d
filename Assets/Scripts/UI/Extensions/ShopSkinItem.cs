using System;
using System.Collections;
using System.Collections.Generic;
using HyperCatSdk;
using MoreMountains.NiceVibrations;
#if !PROTOTYPE
using RocketTeam.Sdk.Services.Ads;
#endif
using UnityEngine;
using UnityEngine.UI;

public class ShopSkinItem : UIItem
{
    [SerializeField]
    private UILabel moneyCostLb;

    [SerializeField]
    private UILabel adsCostLb;

    [SerializeField]
    private UILabel requireLb;

    [SerializeField]
    private GameObject toggle;

    [SerializeField]
    private GameObject unlockLb;

    [SerializeField]
    private UILabel nameLb;

    private SkinConfig mySkinCfg = null;

    [SerializeField]
    private SkinController characterSkin;

    [SerializeField]
    private Button useBtn;

    [SerializeField]
    private Button buyBtn;

    [SerializeField]
    private Button adsBtn;

    [SerializeField]
    private GameObject hiddenSkin;

    public void UpdateSelected()
    {
        // if (GM.Data.IsSkinUnlocked(mySkinCfg))
        // {
        //     toggle.SetActive(mySkinCfg.ID == GM.Data.MySkin);
        //     useBtn.gameObject.SetActive(mySkinCfg.ID != GM.Data.MySkin);
        //     unlockLb.SetActive(mySkinCfg.ID != GM.Data.MySkin);
        // }
    }

    public void Init(SKINS id)
    {
        // mySkinCfg = Cfg.GetSkinConfig(id);
        // nameLb.text = mySkinCfg.Name;
        // characterSkin.Init(mySkinCfg.ID);
        // if (GM.Data.IsSkinUnlocked(mySkinCfg))
        // {
        //     adsBtn.gameObject.SetActive(false);
        //     buyBtn.gameObject.SetActive(false);
        //     requireLb.gameObject.SetActive(false);
        //     toggle.SetActive(id == GM.Data.MySkin);
        //     useBtn.gameObject.SetActive(id != GM.Data.MySkin);
        //     unlockLb.SetActive(id != GM.Data.MySkin);
        //     hiddenSkin.SetActive(false);
        //     characterSkin.gameObject.SetActive(true);
        // }
        // else
        // {
        //     unlockLb.SetActive(false);
        //     useBtn.gameObject.SetActive(false);
        //     toggle.SetActive(false);
        //     if (GM.Data.Level >= mySkinCfg.UnlockLevel)
        //     {
        //         buyBtn.gameObject.SetActive(mySkinCfg.GoldCost > 0);
        //         moneyCostLb.text = mySkinCfg.GoldCost.ToFormatString();
        //
        //         adsBtn.gameObject.SetActive(mySkinCfg.AdsCost > 0);
        //         adsCostLb.text = GM.Data.GetSkinWatchedAds(mySkinCfg.ID) + "/" + mySkinCfg.AdsCost;
        //
        //         requireLb.gameObject.SetActive(false);
        //         hiddenSkin.SetActive(false);
        //         characterSkin.gameObject.SetActive(true);
        //     }
        //     else
        //     {
        //         characterSkin.gameObject.SetActive(false);
        //         hiddenSkin.SetActive(true);
        //         buyBtn.gameObject.SetActive(false);
        //         adsBtn.gameObject.SetActive(false);
        //         requireLb.gameObject.SetActive(true);
        //         requireLb.text = "Level " + mySkinCfg.UnlockLevel;
        //     }
        // }
    }

    void OnButtonUse()
    {
        AudioAssistant.Shot(TYPE_SOUND.BUTTON);
        UseSkin();
        PopupShop.Instance.UpdateSelectAll();
        HCVibrate.Haptic(HapticTypes.SoftImpact);
    }

    void OnButtonBuyGold()
    {
        AudioAssistant.Shot(TYPE_SOUND.BUTTON);
        BuySkinGold();

        PopupShop.Instance.UpdateSelectAll();
        HCVibrate.Haptic(HapticTypes.Success);
    }

    void OnButtonBuyAds()
    {
        AudioAssistant.Shot(TYPE_SOUND.BUTTON);
        BuySkinAds();

        HCVibrate.Haptic(HapticTypes.Success);
    }

    void UseSkin()
    {
        // if (!GM.Data.IsSkinUnlocked(mySkinCfg) || GM.Data.MySkin == mySkinCfg.ID)
        //     return;
        //
        // GM.Data.MySkin = mySkinCfg.ID;
        //
        // Player.Instance.UpdateSkin();
        Database.SaveData();
    }

    void BuySkinGold()
    {
        if (!EnoughMoney())
            return;

        // GM.Data.Money -= mySkinCfg.GoldCost;
        // if (!GM.Data.SkinUnlockData.ContainsKey(mySkinCfg.ID))
        //     GM.Data.SkinUnlockData.Add(mySkinCfg.ID, mySkinCfg.AdsCost + 1);
        // else
        //     GM.Data.SkinUnlockData[mySkinCfg.ID] += mySkinCfg.AdsCost + 1;
        // GM.Data.MySkin = mySkinCfg.ID;
        //
        // Player.Instance.UpdateSkin();
        MainScreen.Instance.CheckNotice();
        Database.SaveData();
        MainScreen.Instance.UpdateMoney();

        Init(mySkinCfg.ID);
        // AudioAssistant.Shot(TYPE_SOUND.BUY_SKIN);
    }

    void BuySkinAds()
    {
        if (GM.Data.User.PurchasedNoAds)
            OnBuySkin(1);
#if !PROTOTYPE
        else
            AdManager.Instance.ShowAdsReward(OnBuySkin, "BuySkin");
#endif
    }

    void OnBuySkin(int result)
    {
        // if (!GM.Data.SkinUnlockData.ContainsKey(mySkinCfg.ID))
        //     GM.Data.SkinUnlockData.Add(mySkinCfg.ID, 1);
        // else
        //     GM.Data.SkinUnlockData[mySkinCfg.ID] += 1;
        //
        // if (GM.Data.SkinUnlockData[mySkinCfg.ID] >= mySkinCfg.AdsCost)
        // {
        //     GM.Data.MySkin = mySkinCfg.ID;
        //
        //     Player.Instance.UpdateSkin();
        //     MainScreen.Instance.CheckNotice();
        //     AudioAssistant.Shot(TYPE_SOUND.BUY_SKIN);
        // }

        Database.SaveData();
        MainScreen.Instance.UpdateMoney();

        Init(mySkinCfg.ID);

        PopupShop.Instance.UpdateSelectAll();
    }

    bool EnoughMoney()
    {
        return GM.Data.User.Money >= mySkinCfg.GoldCost;
    }

    private void OnEnable()
    {
        useBtn.onClick.AddListener(OnButtonUse);
        buyBtn.onClick.AddListener(OnButtonBuyGold);
        adsBtn.onClick.AddListener(OnButtonBuyAds);
    }

    private void OnDisable()
    {
        useBtn.onClick.RemoveListener(OnButtonUse);
        buyBtn.onClick.RemoveListener(OnButtonBuyGold);
        adsBtn.onClick.RemoveListener(OnButtonBuyAds);
    }
}