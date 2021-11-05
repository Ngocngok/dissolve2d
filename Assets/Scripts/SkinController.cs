using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinController : HCMonobehavior
{
    public List<GameObject> ListSkin = new List<GameObject>();

    public void Init(SKINS id)
    {
        for (int i = 0; i < ListSkin.Count; i++)
        {
            ListSkin[i].SetActive(i == (int) id);
        }
    }
}