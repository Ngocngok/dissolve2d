using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupNoInternet : UIPanel
{
    public override UI_PANEL GetID()
    {
        return UI_PANEL.PopupNoInternet;
    }

    public static void Show()
    {
        var newInstance = (PopupNoInternet) GUIManager.Instance.NewPanel(UI_PANEL.PopupNoInternet);
        newInstance.OnAppear();
    }

    public void OnAppear()
    {
        base.OnAppear(3);
    }
}