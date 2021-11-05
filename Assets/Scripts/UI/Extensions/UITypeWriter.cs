using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITypeWriter : MonoBehaviour
{
    public bool AutoStart = false;
    public string Content;
    public float Duration;
    public bool IsFinished = true;
    public UILabel Label;
    public string[] Words;

    private void Start()
    {
        if (AutoStart)
            StartEffect();
    }

    public void Init(string content, float duration)
    {
        Content = content;
        Duration = duration;
    }

    private int wordIndex = 0;
    private string curStr = "";

    public void StartEffect()
    {
        if (Duration <= 0f)
        {
            FinishEffect();
        }
        else
        {
            Words = Content.Split(' ');
            IsFinished = false;
            var delay = (float) Duration / (float) (Words.Length * 1.5f);
            wordIndex = 0;
            curStr = "";
            InvokeRepeating(nameof(AddCharacter), 0, delay);
        }
    }

    public void FinishEffect()
    {
        IsFinished = true;
        Label.text = Content;
        wordIndex = 0;
        curStr = "";
        CancelInvoke(nameof(AddCharacter));
    }

    private void AddCharacter()
    {
        if (wordIndex >= Words.Length - 1)
        {
            FinishEffect();
            return;
        }

        curStr += " " + Words[wordIndex];
        wordIndex++;
        Label.text = curStr;
    }
}