using System;
using System.Collections;
using System.Collections.Generic;
#if !PROTOTYPE
using RocketTeam.Sdk.Services.Ads;
#endif
using UnityEngine;
using UnityEngine.UI;

public class PopupOfferNoAds : UIPanel
{
    public override UI_PANEL GetID()
    {
        return UI_PANEL.PopupOfferNoAds;
    }

    public static PopupOfferNoAds Instance;

    public static void Show()
    {
        PopupOfferNoAds newInstance = (PopupOfferNoAds) GUIManager.Instance.NewPanel(UI_PANEL.PopupOfferNoAds);
        Instance = newInstance;
        newInstance.OnAppear();
    }

    public void OnAppear()
    {
        if (isInited)
            return;

        base.OnAppear();
    }

    public void Refuse()
    {
        AudioAssistant.Shot(TYPE_SOUND.BUTTON);
#if !PROTOTYPE
        AdManager.Instance.ShowInterstitial("RefuseOfferNoAds", 1);
#endif
        Close();
    }

    public void BuyNoAds()
    {
        AudioAssistant.Shot(TYPE_SOUND.BUTTON);
#if !PROTOTYPE
        IAPManager.Instance.BuyProduct(GameConst.NO_ADS_ID);
#endif
        Close();
    }

    public override void OnDisappear()
    {
        AudioAssistant.Shot(TYPE_SOUND.BUTTON);
        base.OnDisappear();
        Instance = null;
    }
}