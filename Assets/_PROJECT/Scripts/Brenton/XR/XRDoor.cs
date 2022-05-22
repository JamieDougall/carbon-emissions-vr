using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class XRDoor : MonoBehaviour
{
    [SerializeField] DoorAxis axis = DoorAxis.Y;
    //[SerializeField] bool invertAngle = false;
    [SerializeField] float maxAngle = 180.0f;
    [SerializeField] float closeAngle = 0.03f;
    public bool isClosed = false;
    [SerializeField] UnityEvent onClose;
    [SerializeField] UnityEvent onOpen;
    [SerializeField] Rigidbody rb;
    float angle;
    Vector3 localStartRotation = Vector3.zero;

    [Header("Audio")]
    [SerializeField] AudioSource closeAudio;
    [SerializeField] float maxVolumeVelocity = 5.0f;

    private void OnValidate()
    {
        if(rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }
    }

    private void Start()
    {
        localStartRotation = transform.localEulerAngles;
    }

    private void Update()
    {
        GetAngle();
    }

    public void GetAngle()
    {
        switch (axis)
        {
            case DoorAxis.X:
                angle = 1.0f - Mathf.Clamp01((maxAngle - Mathf.Abs(transform.localEulerAngles.x)) / maxAngle);
                break;
            case DoorAxis.Y:
                angle = 1.0f - Mathf.Clamp01((maxAngle - Mathf.Abs(transform.localEulerAngles.y)) / maxAngle);
                break;
            case DoorAxis.Z:
                angle = 1.0f - Mathf.Clamp01((maxAngle - Mathf.Abs(transform.localEulerAngles.z)) / maxAngle);
                break;
            default:
                break;
        }
        if (angle < closeAngle)
        {
            // door is within close angle
            angle = 0.0f;
            transform.localEulerAngles = localStartRotation;
            if (!isClosed)
            {
                // door was not already closed
                if (closeAudio != null)
                {
                    float speed = rb.velocity.magnitude;
                    closeAudio.volume = Mathf.Clamp(speed, 0.0f, maxVolumeVelocity) / maxVolumeVelocity;
                }
                onClose.Invoke();
                rb.isKinematic = true;
                isClosed = true;
            }
        }
        else
        {
            rb.isKinematic = false;
            if(isClosed)
            {
                onOpen.Invoke();
                isClosed = false;
            }
        }
    }

    public float Openness
    {
        get
        {
            switch (axis)
            {
                case DoorAxis.X:
                    return 1.0f - Mathf.Clamp01((maxAngle - transform.localEulerAngles.x) / maxAngle);
                case DoorAxis.Y:
                    return 1.0f - Mathf.Clamp01((maxAngle - transform.localEulerAngles.y) / maxAngle);
                case DoorAxis.Z:
                    return 1.0f - Mathf.Clamp01((maxAngle - transform.localEulerAngles.z) / maxAngle);
                default:
                    return 0.0f;
            }
        }
    }

    [System.Serializable]
    enum DoorAxis
    {
        X, Y, Z
    };
}
