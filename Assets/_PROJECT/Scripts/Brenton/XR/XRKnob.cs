using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class XRKnob : MonoBehaviour
{
    [SerializeField] TurnAxis turnAxis = TurnAxis.Z;

    [Header("Activation")]
    [SerializeField] float turnThreshold = 0.01f;
    bool isActivated = false;
    bool wasActivated = false;
    [SerializeField] UnityEvent onActivate;
    [SerializeField] UnityEvent whileActivated;
    [SerializeField] UnityEvent onDeactivate;

    [Header("Float Value")]
    [SerializeField] FloatVariable value;
    [SerializeField] float knobValue;
    float angle = 0.0f;
    [SerializeField] float minValue = 0.0f;
    [SerializeField] float maxValue = 1.0f;
    [SerializeField] float minAngle = -60.0f;
    [SerializeField] float maxAngle = 60.0f;


    void Update()
    {
        switch (turnAxis)
        {
            case TurnAxis.X:
                angle = transform.localEulerAngles.x;
                break;
            case TurnAxis.Y:
                angle = transform.localEulerAngles.y;
                break;
            case TurnAxis.Z:
                angle = transform.localEulerAngles.z;
                break;
            default:
                break;
        }
        while (angle > 180.0f)
        {
            angle -= 360.0f;
        }
        isActivated = angle >= turnThreshold;
        value.RuntimeValue = minValue + (maxValue - minValue) * (angle - minAngle) / (maxAngle - minAngle);

        if (isActivated)
        {
            if (wasActivated)
            {
                whileActivated.Invoke();
            }
            else
            {
                onActivate.Invoke();
            }
        }
        else
        {
            if (wasActivated)
            {
                onDeactivate.Invoke();
            }
        }
        wasActivated = isActivated;
        knobValue = value.RuntimeValue;
    }

    [System.Serializable]
    enum TurnAxis
    {
        X, Y, Z
    };
}
