using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalanceController : HCMonobehavior
{
    public static BalanceController Instance;

    private void Awake()
    {
        Instance = this;
        Root.SetActive(false);
    }

    public Transform Thumb;
    public GameObject Root;
}