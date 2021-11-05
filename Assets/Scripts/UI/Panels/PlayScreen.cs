using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.NiceVibrations;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PlayScreen : UIPanel
{
    public override UI_PANEL GetID()
    {
        return UI_PANEL.PlayScreen;
    }

    public static PlayScreen Instance;

    public static void Show()
    {
        PlayScreen newInstance = (PlayScreen) GUIManager.Instance.NewPanel(UI_PANEL.PlayScreen);
        Instance = newInstance;
        newInstance.OnAppear();
    }

    public void OnAppear()
    {
        base.OnAppear();
        Init();
    }


    private void Init()
    {
    }

    public override void OnDisappear()
    {
        base.OnDisappear();
        Instance = null;
    }
}