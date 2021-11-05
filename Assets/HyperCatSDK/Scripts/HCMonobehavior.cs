using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HCMonobehavior : MonoBehaviour
{
    private Transform privateTransform = null;

    [HideInInspector]
    public Transform Transform
    {
        get
        {
            if (privateTransform == null)
                privateTransform = this.transform;

            return privateTransform;
        }
    }


    private GameManager gameManager;

    [HideInInspector]
    public GameManager GM
    {
        get
        {
            if (gameManager == null)
                gameManager = GameManager.Instance;

            return gameManager;
        }
    }

    private ConfigManager cfg;

    [HideInInspector]
    public ConfigManager Cfg
    {
        get
        {
            if (cfg == null)
                cfg = ConfigManager.Instance;

            return cfg;
        }
    }

    public virtual void OnSpawned()
    {
    }

    public virtual void OnDespawned()
    {
    }
}