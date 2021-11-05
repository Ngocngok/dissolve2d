using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupSetting : UIPanel
{
    public override UI_PANEL GetID()
    {
        return UI_PANEL.PopupSetting;
    }

    public static PopupSetting Instance;

    [SerializeField]
    private Button CloseButton;

    [SerializeField]
    private Button CloseBG;

    [SerializeField]
    Button btnVibrate;

    [SerializeField]
    private GameObject vibrateOff;

    [SerializeField]
    private GameObject vibrateOn;

    [SerializeField]
    Button btnSound;

    [SerializeField]
    private GameObject soundOff;

    [SerializeField]
    private GameObject soundOn;

    [SerializeField]
    Button btnRestorePurchase;

    public static void Show()
    {
        PopupSetting newInstance = (PopupSetting) GUIManager.Instance.NewPanel(UI_PANEL.PopupSetting);
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

    void Init()
    {
        vibrateOn.SetActive(GM.Data.Setting.Haptic == 1);
        vibrateOff.SetActive(GM.Data.Setting.Haptic == 0);

        soundOn.SetActive(GM.Data.Setting.Sound == 1);
        soundOff.SetActive(GM.Data.Setting.Sound == 0);
    }

    public void SwitchSound()
    {
        if (GM.Data.Setting.Sound == 1)
            GM.Data.Setting.Sound = 0;
        else
            GM.Data.Setting.Sound = 1;

        soundOn.SetActive(GM.Data.Setting.Sound == 1);
        soundOff.SetActive(GM.Data.Setting.Sound == 0);

        Database.SaveData();
        EventGlobalManager.Instance.OnUpdateSetting.Dispatch();

        AudioAssistant.Shot(TYPE_SOUND.BUTTON);
    }

    public void SwitchVibrate()
    {
        if (GM.Data.Setting.Haptic == 1)
            GM.Data.Setting.Haptic = 0;
        else
            GM.Data.Setting.Haptic = 1;

        vibrateOn.SetActive(GM.Data.Setting.Haptic == 1);
        vibrateOff.SetActive(GM.Data.Setting.Haptic == 0);

        Database.SaveData();
        EventGlobalManager.Instance.OnUpdateSetting.Dispatch();

        AudioAssistant.Shot(TYPE_SOUND.BUTTON);
    }

    public void OnClickRestorePurchase()
    {
        AudioAssistant.Shot(TYPE_SOUND.BUTTON);
#if UNITY_IOS
        IAPManager.Instance.RestorePurchases();
#endif
    }

    protected override void RegisterEvent()
    {
        base.RegisterEvent();
        CloseButton.onClick.AddListener(Close);
        CloseBG.onClick.AddListener(Close);
    }

    protected override void UnregisterEvent()
    {
        base.UnregisterEvent();
        CloseButton.onClick.RemoveListener(Close);
        CloseBG.onClick.RemoveListener(Close);
    }

    public override void OnDisappear()
    {
        base.OnDisappear();
        Instance = null;
    }

    public override void Close()
    {
        AudioAssistant.Shot(TYPE_SOUND.BUTTON);
        base.Close();
    }
}