using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIToggle : MonoBehaviour
{
    public UIToggleGroup group;
    public List<GameObject> toggleObj;
    public UnityEvent OnToggleEvent;

    public void Select()
    {
        group.Select(this);
    }

    public void Toggle(bool on)
    {
        foreach (var obj in toggleObj)
        {
            if (obj != null)
                obj.SetActive(on);
        }

        if (on)
        {
            OnToggleEvent.Invoke();
        }

        if (group.ScaleDiff)
        {
            if (on)
                transform.localScale = new Vector3(1f, 1f, 1f);
            else
                transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
        }
    }
}