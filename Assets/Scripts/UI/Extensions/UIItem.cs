using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIItem : HCMonobehavior
{
    [SerializeField]
    public UISprite ItemIcon;

    [SerializeField]
    public UILabel QuantityLb;

    [SerializeField]
    public Button Interaction;

    [HideInInspector]
    public string CodeName;

    public int ItemIndex;

    public bool PixelPerfect = false;

    public virtual void Init(string codeName, long quantity)
    {
        this.CodeName = codeName;

        if (QuantityLb)
            QuantityLb.text = quantity.ToFormatString();
        if (ItemIcon)
        {
            ItemIcon.SetSprite(codeName);
            if (PixelPerfect)
                ItemIcon.MakePixelPerfect();
        }
    }
}