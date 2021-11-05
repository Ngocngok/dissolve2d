using System;
using System.Collections;
using System.Collections.Generic;
using Sigtrap.Relays;
using UnityEngine;

public class UITweenPlayer : MonoBehaviour
{
    public bool AutoPlay;

    public List<UITweener> ListTween;

    public Relay OnForwardFinish = new Relay();
    public Relay OnReserveFinish = new Relay();

    public float MaxDuration
    {
        get
        {
            float result = 0f;

            foreach (var anim in ListTween)
            {
                if (anim == null)
                    continue;

                if (result < anim.MaxDuration)
                    result = anim.MaxDuration;
            }

            return result;
        }
    }

    private void OnEnable()
    {
        if (AutoPlay)
            PlayForward();
    }

    void ForwardFinish()
    {
        OnForwardFinish.Dispatch();
    }

    void ReserveFinish()
    {
        OnReserveFinish.Dispatch();
    }

    public void PlayForward()
    {
        if (ListTween == null || ListTween.Count <= 0)
        {
            ForwardFinish();
            return;
        }

        foreach (var tweener in ListTween)
        {
            if (tweener == null || !tweener.gameObject.activeSelf)
                continue;

            tweener.AnimIn();
        }

        Invoke(nameof(ForwardFinish), MaxDuration);
    }

    public void PlayReserve()
    {
        if (ListTween == null || ListTween.Count <= 0)
        {
            ForwardFinish();
            return;
        }

        foreach (var tweener in ListTween)
        {
            if (tweener == null || !tweener.gameObject.activeSelf)
                continue;

            tweener.AnimOut();
        }

        Invoke(nameof(ReserveFinish), MaxDuration);
    }
}