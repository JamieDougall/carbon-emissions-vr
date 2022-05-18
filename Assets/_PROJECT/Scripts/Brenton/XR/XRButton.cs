using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class XRButton : MonoBehaviour
{
    [SerializeField] PressAxis pressAxis = PressAxis.Z;
    [SerializeField] float pressThreshold = 0.01f;
    [SerializeField] float pressLimit = 0.02f;
    bool isPressed = false;
    bool wasPressed = false;
    [SerializeField] Rigidbody rb;

    [SerializeField] bool stickMode = false;
    bool stick = false;
    [SerializeField] float stickModeDisengage = 0.021f;

    [Header("Events")]
    [SerializeField] UnityEvent onPress;
    [SerializeField] UnityEvent onHold;
    [SerializeField] UnityEvent onRelease;

    public void Unstick()
    {
        stick = false;
        rb.isKinematic = false;
    }

    void Update()
    {
        float distance = 0.0f;
        switch (pressAxis)
        {
            case PressAxis.X:
                distance = transform.localPosition.x;
                //isPressed = transform.localPosition.x > pressThreshold;
                break;
            case PressAxis.Y:
                distance = transform.localPosition.y;
                //isPressed = transform.localPosition.y > pressThreshold;
                break;
            case PressAxis.Z:
                distance = transform.localPosition.z;
                //isPressed = transform.localPosition.z > pressThreshold;
                break;
            default:
                break;
        }
        isPressed = distance > pressThreshold;

        if (isPressed)
        {
            if (wasPressed)
            {
                //Already pressed
                onHold.Invoke();
            }
            else
            {
                //Pressed this frame
                onPress.Invoke();
                //Should the button stick
                if (stickMode)
                {
                    stick = true;
                    rb.isKinematic = true;
                    /*
                    if (distance < pressLimit)
                    {
                        switch (pressAxis)
                        {
                            case PressAxis.X:
                                //rb.position = Vector3.right * pressLimit;
                                transform.localPosition = Vector3.right * pressLimit;
                                break;
                            case PressAxis.Y:
                                //rb.position = Vector3.up * pressLimit;
                                transform.localPosition = Vector3.up * pressLimit;
                                break;
                            case PressAxis.Z:
                                //rb.position = Vector3.forward * pressLimit;
                                transform.localPosition = Vector3.forward * pressLimit;
                                break;
                            default:
                                break;
                        }
                    }
                    */
                }
            }
        }
        else
        {
            if(wasPressed)
            {
                //Released this frame
                onRelease.Invoke();
            }
        }

        wasPressed = isPressed;
    }

    [System.Serializable]
    enum PressAxis
    {
        X, Y, Z
    };
}
