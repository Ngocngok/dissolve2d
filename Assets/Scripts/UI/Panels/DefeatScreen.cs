using System.Collections;
using System.Collections.Generic;

//using RocketTeam.Sdk.Services.Ads;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DefeatScreen : UIPanel
{
    public override UI_PANEL GetID()
    {
        return UI_PANEL.DefeatScreen;
    }

    public static DefeatScreen Instance;

    [SerializeField]
    private Button retryButton;

    [SerializeField]
    private Button skipLevelButton;

    [SerializeField]
    private UILabel levelLb;

    [SerializeField]
    private TMP_Text myGoldLb;

    [SerializeField]
    private TMP_Text rewardLb;

    public static void Show()
    {
        DefeatScreen newInstance = (DefeatScreen) GUIManager.Instance.NewPanel(UI_PANEL.DefeatScreen);
        Instance = newInstance;
        newInstance.OnAppear();
    }

    public void OnAppear()
    {
        if (isInited)
            return;

        base.OnAppear();

        Init();
        // AudioAssistant.Shot(TYPE_SOUND.DEFEAT);

        //AndroidManager.RejectFeedback();
    }

    private int currentGold;
    private int rewardGold;

    void Init()
    {
        levelLb.text = "Level " + GM.Data.User.Level;
        currentGold = GM.Data.User.Money;
        rewardGold = 50 + 25 * GM.Data.User.Level;
        rewardLb.text = rewardGold.ToFormatString();
        GM.Data.User.Money += rewardGold;
        Database.SaveData();
        myGoldLb.ChangeText(currentGold, GM.Data.User.Money, 1f);
    }

    void Retry()
    {
        // LevelController.Instance.ResetLevel();
    }

    void SkipLevel()
    {
        if (!GameManager.EnableAds)
            OnCompleteAds(1);

        // else
        //     AdManager.Instance.ShowAdsReward(OnCompleteAds, "SkipLevel");
    }

    void OnCompleteAds(int result)
    {
        GM.Data.User.Level++;
        Database.SaveData();

        // LevelController.Instance.ResetLevel();
    }

    protected override void RegisterEvent()
    {
        base.RegisterEvent();
        retryButton.onClick.AddListener(Retry);
        skipLevelButton.onClick.AddListener(SkipLevel);
    }

    protected override void UnregisterEvent()
    {
        base.UnregisterEvent();
        retryButton.onClick.RemoveListener(Retry);
        skipLevelButton.onClick.RemoveListener(SkipLevel);
    }

    public override void OnDisappear()
    {
        base.OnDisappear();
        Instance = null;
    }
}