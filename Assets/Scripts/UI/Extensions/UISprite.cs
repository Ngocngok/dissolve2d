using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;

[RequireComponent(typeof(Image))]
public class UISprite : MonoBehaviour
{
    public SpriteAtlas Atlas;

    [SerializeField]
    public Image image;

    private void Start()
    {
        if (image == null)
            image = GetComponent<Image>();

        if (image == null)
            return;
    }

    public void SetSprite(string spriteName)
    {
        if (Atlas == null || image == null)
            return;

        image.sprite = Atlas.GetSprite(spriteName);
    }

    public void SetAlpha(float alpha)
    {
        if (image == null)
            return;

        image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
    }

    public void SetColor(Color c)
    {
        if (image == null)
            return;

        image.color = c;
    }

    public void MakePixelPerfect()
    {
        if (image == null)
            return;

        image.SetNativeSize();
    }

    public void Hide()
    {
        image.enabled = false;
    }

    public void Show()
    {
        image.enabled = true;
    }
}