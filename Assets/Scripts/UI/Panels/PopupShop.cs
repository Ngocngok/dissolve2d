using System.Collections;
using System.Collections.Generic;
#if !PROTOTYPE
using RocketTeam.Sdk.Services.Ads;
#endif
using UnityEngine;
using UnityEngine.UI;

public class PopupShop : UIPanel
{
    public override UI_PANEL GetID()
    {
        return UI_PANEL.PopupShop;
    }

    public static PopupShop Instance;

    [SerializeField]
    private Button CloseButton;

    [SerializeField]
    private Transform RootGrid;

    [SerializeField]
    private GameObject prefabItem;

    private List<ShopSkinItem> listItem = new List<ShopSkinItem>();

    public static void Show(bool showSkin = true)
    {
        PopupShop newInstance = (PopupShop) GUIManager.Instance.NewPanel(UI_PANEL.PopupShop);
        Instance = newInstance;
        newInstance.OnAppear(showSkin);
    }

    public void OnAppear(bool showSkin = true)
    {
        if (isInited)
            return;

        base.OnAppear();

        Init(showSkin);
    }

    void Init(bool showSkin = true)
    {
        GameManager.DestroyChildren(RootGrid);
        prefabItem.SetActive(true);
        listItem.Clear();

        if (showSkin)
        {
            // foreach (var pair in Cfg.Skins)
            // {
            //     var obj = Instantiate(prefabItem, RootGrid);
            //     var script = obj.GetComponent<ShopSkinItem>();
            //     script.Init(pair.Key);
            //     listItem.Add(script);
            // }

#if !PROTOTYPE
            if (!GameManager.Instance.Data.User.PurchasedNoAds)
                AdManager.Instance.ShowInterstitial("OpenSkinShop", 1);
#endif
        }

        prefabItem.SetActive(false);
    }

    public void UpdateSelectAll()
    {
        foreach (var shopSkinItem in listItem)
        {
            shopSkinItem.UpdateSelected();
        }
    }

    protected override void RegisterEvent()
    {
        base.RegisterEvent();
        CloseButton.onClick.AddListener(Close);
    }

    protected override void UnregisterEvent()
    {
        base.UnregisterEvent();
        CloseButton.onClick.RemoveListener(Close);
    }

    public override void OnDisappear()
    {
        AudioAssistant.Shot(TYPE_SOUND.BUTTON);
        base.OnDisappear();
        Instance = null;
    }
}