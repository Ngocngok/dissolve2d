using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UILabel : MonoBehaviour
{
    [SerializeField]
    Text unityText;
    [SerializeField]
    TMP_Text tmpText;

    private void Start()
    {
        if (unityText != null)
            myColor = unityText.color;
        if (tmpText != null)
            myColor = tmpText.color;
    }

    string myValue = string.Empty;

    public string text
    {
        get
        {
            return myValue;
        }

        set
        {
            myValue = value;
            if (unityText != null)
                unityText.text = value;
            if (tmpText != null)
                tmpText.text = value;
        }
    }

    Color myColor = Color.white;

    public Color color
    {
        get
        {
            return myColor;
        }

        set
        {
            myColor = value;
            if (unityText != null)
                unityText.color = value;
            if (tmpText != null)
                tmpText.color = value;
        }
    }

    public void Show()
    {
        if (unityText != null)
            unityText.enabled = true;
        if (tmpText != null)
            tmpText.enabled = true;
    }

    public void Hide()
    {
        if (unityText != null)
            unityText.enabled = false;
        if (tmpText != null)
            tmpText.enabled = false;
    }
}
