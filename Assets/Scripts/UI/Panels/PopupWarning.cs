using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopupWarning : UIPanel
{
    public override UI_PANEL GetID()
    {
        return UI_PANEL.PopupWarning;
    }

    public TMP_Text MessageText;

    public static void Show(string message, float duration = 2f)
    {
        PopupWarning newInstance = (PopupWarning)GUIManager.Instance.NewPanel(UI_PANEL.PopupWarning);
        newInstance.OnAppear(message, duration);
    }

    private void OnAppear(string message, float duration)
    {
        base.OnAppear(duration);
        MessageText.text = message;
    }
}