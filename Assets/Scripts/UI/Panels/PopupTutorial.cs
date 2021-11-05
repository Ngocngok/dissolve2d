using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupTutorial : UIPanel
{
    public override UI_PANEL GetID()
    {
        return UI_PANEL.PopupTutorial;
    }

    public static PopupTutorial Instance;

    [SerializeField]
    private GameObject handObject;

    [SerializeField]
    private UILabel descLb;

    public static void Show()
    {
        PopupTutorial newInstance = (PopupTutorial) GUIManager.Instance.NewPanel(UI_PANEL.PopupTutorial);
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
    }

    protected override void RegisterEvent()
    {
        base.RegisterEvent();
    }

    protected override void UnregisterEvent()
    {
        base.UnregisterEvent();
    }

    public override void OnDisappear()
    {
        base.OnDisappear();
        Time.timeScale = 1f;
        Instance = null;
    }
}