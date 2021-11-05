using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class UILoading : MonoBehaviour
{
    public Canvas Root;

    [SerializeField]
    SpriteAtlas Atlas;

    public UISprite LoadingBG;
    public UILabel HintLb;
    public UILabel LoadingLb;
    public UIProgressBar ProgressBar;


    public void Write(string s)
    {
        LoadingLb.text = s;
    }

    public void Progress(float p)
    {
        ProgressBar.SetValue(p);
    }
}