using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIProgressBar : MonoBehaviour
{
    public Text ProgressLb;
    public Image FG;
    public Image BG;
    public Image Thumb;

    float value;

    private void LateUpdate()
    {
        if (Sliced || Thumb == null)
            return;

        if (FG.fillAmount > 0.96f)
        {
            Thumb.enabled = false;
        }
        else
        {
            Thumb.enabled = true;
            float x = FG.fillAmount * FG.rectTransform.sizeDelta.x - FG.rectTransform.sizeDelta.x / 2f;
            Thumb.rectTransform.localPosition = new Vector3(x, Thumb.rectTransform.localPosition.y, Thumb.rectTransform.localPosition.z);
        }
    }

    public bool Sliced = false;

    public void SetValue(float oldValue, float newValue, bool reverse = false)
    {
        Utils.ChangeImageFill(FG, value, oldValue, 0f);
        value = oldValue;
        SetValue(newValue, reverse);
    }

    public void SetValue(float newValue, bool reverse = false)
    {
        if (!Sliced)
        {
            if (reverse)
                Utils.ChangeImageFill(FG, value, newValue, 0.3f);
            else
            {
                if (newValue < value)
                {
                    StopAllCoroutines();
                    StartCoroutine(Fill(value, newValue));
                }
                else
                    Utils.ChangeImageFill(FG, value, newValue, 0.3f);
            }
        }
        else
        {
            float maxW = BG.rectTransform.sizeDelta.x;
            float oldW = maxW * value;
            float newW = maxW * newValue;

            if (reverse)
            {
                Utils.ChangeImageWidth(FG.rectTransform, newW, 0.3f);
            }
            else
            {
                if (newW < oldW)
                {
                    StopAllCoroutines();
                    StartCoroutine(Slice(oldW, newW, maxW));
                }
                else
                    Utils.ChangeImageWidth(FG.rectTransform, newW, 0.3f);
            }
        }

        value = newValue;
    }

    IEnumerator Slice(float oldValue, float newValue, float maxW)
    {
        float valueToFull = maxW - oldValue;
        float totalValue = maxW - oldValue + newValue;

        float time1 = valueToFull / totalValue * 0.3f;
        float time2 = newValue / totalValue * 0.3f;

        if (time1 > 0f)
        {
            Utils.ChangeImageWidth(FG.rectTransform, maxW, time1);
            yield return new WaitForSeconds(time1);
            FG.rectTransform.sizeDelta = new Vector2(0, FG.rectTransform.sizeDelta.y);
        }

        if (time2 > 0f)
            Utils.ChangeImageWidth(FG.rectTransform, newValue, time2);
    }

    IEnumerator Fill(float oldValue, float newValue)
    {
        float valueToFull = 1f - oldValue;
        float totalValue = 1f - oldValue + newValue;

        float time1 = valueToFull / totalValue * 0.3f;
        float time2 = newValue / totalValue * 0.3f;

        if (time1 > 0f)
        {
            Utils.ChangeImageFill(FG, value, 1f, time1);
            yield return new WaitForSeconds(time1);
            FG.fillAmount = 0f;
        }

        if (time2 > 0f)
            Utils.ChangeImageFill(FG, 0f, newValue, time2);
    }
}