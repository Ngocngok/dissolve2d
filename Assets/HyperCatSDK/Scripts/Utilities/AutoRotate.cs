using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRotate : HCMonobehavior
{
    public float Speed;

    public RotateAxis Axis = RotateAxis.Z;

    private float newValue = 0f;

    private void Update()
    {
        newValue += Time.deltaTime * Speed;

        switch (Axis)
        {
            case RotateAxis.X:
                Transform.localEulerAngles = new Vector3(newValue, Transform.localEulerAngles.y, Transform.localEulerAngles.z);
                break;
            case RotateAxis.Y:
                Transform.localEulerAngles = new Vector3(Transform.localEulerAngles.x, newValue, Transform.localEulerAngles.z);
                break;
            case RotateAxis.Z:
                Transform.localEulerAngles = new Vector3(Transform.localEulerAngles.x, Transform.localEulerAngles.y, newValue);
                break;
        }
    }
}

public enum RotateAxis
{
    X,
    Y,
    Z
}