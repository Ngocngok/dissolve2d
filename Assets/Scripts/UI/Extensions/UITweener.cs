using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Sigtrap.Relays;
#if UNITY_EDITOR
using Sirenix.OdinInspector;
#endif
using UnityEngine;

public class UITweener : MonoBehaviour
{
    public bool AutoPlay;

    public bool HideInFinish = false;

    public CanvasGroup Root;

    public List<AnimUISetup> Anims;

    public Vector3 originPos;

#if UNITY_EDITOR
    [Button]
    void SetOriginPosition()
    {
        originPos = transform.localPosition;
    }
#endif

    private void OnEnable()
    {
        if (AutoPlay)
            AnimIn();
    }

    public float MaxDuration
    {
        get
        {
            float result = 0f;

            foreach (var anim in Anims)
            {
                if (anim == null)
                    continue;

                if (result < anim.Delay + anim.Duration)
                    result = anim.Delay + anim.Duration;
            }

            return result;
        }
    }

    public enum ANIM
    {
        NONE,
        MOVE_LEFT,
        MOVE_RIGHT,
        MOVE_UP,
        MOVE_DOWN,
        SCALE,
        FADE
    }

    IEnumerator Move(AnimUISetup info, bool forward = true)
    {
        yield return new WaitForSeconds(info.Delay);

        Vector3 dir = Vector3.zero;

        switch (info.Anim)
        {
            case ANIM.MOVE_LEFT:
                dir = Vector3.left;
                break;

            case ANIM.MOVE_RIGHT:
                dir = Vector3.right;
                break;

            case ANIM.MOVE_UP:
                dir = Vector3.up;
                break;

            case ANIM.MOVE_DOWN:
                dir = Vector3.down;
                break;
        }

        if (forward)
        {
            Root.transform.localPosition = originPos + dir * info.ValueFrom;
            Root.transform.DOLocalMove(originPos + dir * info.ValueTo, info.Duration, false).OnComplete(() =>
            {
                if (HideInFinish)
                    this.gameObject.SetActive(false);
            });
        }
        else
        {
            Root.transform.localPosition = originPos + dir * info.ValueTo;
            Root.transform.DOLocalMove(originPos + dir * info.ValueFrom, info.Duration, false).OnComplete(() =>
            {
                if (HideInFinish)
                    this.gameObject.SetActive(false);
            });
        }
    }

    IEnumerator Fade(AnimUISetup info, bool forward = true)
    {
        yield return new WaitForSeconds(info.Delay);

        if (forward)
        {
            Root.alpha = info.ValueFrom;
            Root.DOFade(info.ValueTo, info.Duration).OnComplete(() =>
            {
                if (HideInFinish)
                    this.gameObject.SetActive(false);
            });
        }
        else
        {
            Root.alpha = info.ValueTo;
            Root.DOFade(info.ValueFrom, info.Duration).OnComplete(() =>
            {
                if (HideInFinish)
                    this.gameObject.SetActive(false);
            });
        }
    }

    IEnumerator Scale(AnimUISetup info, bool forward)
    {
        yield return new WaitForSeconds(info.Delay);

        if (forward)
        {
            Root.transform.localScale = Vector3.one * info.ValueFrom;
            Root.transform.DOScale(Vector3.one * info.ValueTo, info.Duration).OnComplete(() =>
            {
                if (HideInFinish)
                    this.gameObject.SetActive(false);
            });
        }
        else
        {
            Root.transform.localScale = Vector3.one * info.ValueTo;
            Root.transform.DOScale(Vector3.one * info.ValueFrom, info.Duration).OnComplete(() =>
            {
                if (HideInFinish)
                    this.gameObject.SetActive(false);
            });
        }
    }

    public void AnimIn()
    {
        Reset();
        PlayAnim(true);
    }

    public void AnimOut()
    {
        Complete();
        PlayAnim(false);
    }

    void PlayAnim(bool forward)
    {
        Root.DOKill();

        if (Anims == null || Anims.Count <= 0)
        {
            if (forward)
                ForwardFinish();
            else
                ReserveFinish();
            return;
        }

        foreach (var anim in Anims)
        {
            if (anim == null)
                continue;

            switch (anim.Anim)
            {
                case ANIM.MOVE_UP:
                case ANIM.MOVE_DOWN:
                case ANIM.MOVE_LEFT:
                case ANIM.MOVE_RIGHT:
                    StartCoroutine(Move(anim, forward));
                    break;
                case ANIM.SCALE:
                    StartCoroutine(Scale(anim, forward));
                    break;
                case ANIM.FADE:
                    StartCoroutine(Fade(anim, forward));
                    break;
            }
        }

        CancelInvoke();
        if (forward)
            Invoke(nameof(ForwardFinish), MaxDuration);
        else
            Invoke(nameof(ReserveFinish), MaxDuration);
    }

    public void Reset()
    {
        Root.DOKill();

        if (Anims == null || Anims.Count <= 0)
        {
            return;
        }

        foreach (var anim in Anims)
        {
            if (anim == null)
                continue;

            switch (anim.Anim)
            {
                case ANIM.MOVE_UP:
                    Root.transform.localPosition = originPos + Vector3.up * anim.ValueFrom;
                    break;
                case ANIM.MOVE_DOWN:
                    Root.transform.localPosition = originPos + Vector3.down * anim.ValueFrom;
                    break;
                case ANIM.MOVE_LEFT:
                    Root.transform.localPosition = originPos + Vector3.left * anim.ValueFrom;
                    break;
                case ANIM.MOVE_RIGHT:
                    Root.transform.localPosition = originPos + Vector3.right * anim.ValueFrom;
                    break;
                case ANIM.SCALE:
                    Root.transform.localScale = anim.ValueFrom * Vector3.one;
                    break;
                case ANIM.FADE:
                    Root.alpha = anim.ValueFrom;
                    break;
            }
        }
    }

    public void Complete()
    {
        Root.DOKill();

        if (Anims == null || Anims.Count <= 0)
        {
            return;
        }

        foreach (var anim in Anims)
        {
            if (anim == null)
                continue;

            switch (anim.Anim)
            {
                case ANIM.MOVE_UP:
                    Root.transform.localPosition = originPos + Vector3.up * anim.ValueTo;
                    break;
                case ANIM.MOVE_DOWN:
                    Root.transform.localPosition = originPos + Vector3.down * anim.ValueTo;
                    break;
                case ANIM.MOVE_LEFT:
                    Root.transform.localPosition = originPos + Vector3.left * anim.ValueTo;
                    break;
                case ANIM.MOVE_RIGHT:
                    Root.transform.localPosition = originPos + Vector3.right * anim.ValueTo;
                    break;
                case ANIM.SCALE:
                    Root.transform.localScale = anim.ValueTo * Vector3.one;
                    break;
                case ANIM.FADE:
                    Root.alpha = anim.ValueTo;
                    break;
            }
        }
    }

    public Relay OnForwardFinish = new Relay();
    public Relay OnReserveFinish = new Relay();

    void ForwardFinish()
    {
        OnForwardFinish.Dispatch();
        Complete();
    }

    void ReserveFinish()
    {
        OnReserveFinish.Dispatch();
        Reset();
    }

    [System.Serializable]
    public class AnimUISetup
    {
        public ANIM Anim;
        public float Delay;
        public float Duration;
        public float ValueFrom;
        public float ValueTo;
    }
}