using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIToggleGroup : MonoBehaviour
{
    public List<UIToggle> listToggle;

    public bool ScaleDiff = false;

    public void Select(UIToggle toggle)
    {
        foreach (var uiToggle in listToggle)
        {
            uiToggle.Toggle(uiToggle == toggle);
        }
    }
}