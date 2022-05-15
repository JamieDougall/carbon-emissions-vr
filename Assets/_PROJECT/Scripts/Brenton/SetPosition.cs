using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class SetPosition : MonoBehaviour
{
    public Mode mode;
    public Transform targetTransform;
    public Vector3 targetPosition;
    public Vector3 targetRotation;
    private Vector3 originalPosition;
    private Vector3 originalRotation;
    //Interactable interactable; 
    Rigidbody rb;
    public bool grabbed = false;
    public float distanceThreshold = 0.1f;
    public float timeBeforeReset = 1.0f;
    private float resetTimer = 0.0f;
    public AudioSource returnSound;
    public bool playReturnSound = false;

    public void SetGrabbed(bool isGrabbed)
    {
        grabbed = isGrabbed;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        //interactable = GetComponent<Interactable>();
        //interactable.onAttachedToHand += Interactable_onAttachedToHand;
        //interactable.onDetachedFromHand += Interactable_onDetachedFromHand;
        originalPosition = transform.position;
        originalRotation = transform.eulerAngles;
    }

    /*private void Interactable_onDetachedFromHand(Hand hand)
    {
        grabbed = false;
        resetTimer = 0.0f;
    }

    private void Interactable_onAttachedToHand(Hand hand)
    {
        grabbed = true;
    }*/

    private void Update()
    {
        if (!grabbed & Vector3.Magnitude(originalPosition - transform.position) > distanceThreshold)
        {
            resetTimer += Time.deltaTime;
            if (resetTimer > timeBeforeReset)
            {
                Reset();
            }
        }
        else if (grabbed)
        {
            resetTimer = 0.0f;
        }
    }

    public void Set()
    {
        if (rb != null)
        {
            rb.isKinematic = true;
        }
        switch (mode)
        {
            case Mode.Transform:
                transform.position = targetTransform.position;
                transform.rotation = targetTransform.rotation;
                break;
            case Mode.Vector3:
                transform.position = targetPosition;
                transform.eulerAngles = targetRotation;
                break;
            default:
                break;
        }
        if (playReturnSound)
        {
            returnSound.Play();
        }
        if (rb != null)
        {
            rb.isKinematic = true;
        }
    }

    public void Set(float time)
    {
        StartCoroutine(SetAfterTime(time));
    }

    public IEnumerator SetAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        Set();
    }

    public void Reset()
    {
        if (rb != null)
        {
            rb.isKinematic = true;
        }
        transform.position = originalPosition;
        transform.eulerAngles = originalRotation;
        if (rb != null)
        {
            rb.isKinematic = false;
        }
        resetTimer = 0.0f;
        if (playReturnSound)
        {
            returnSound.Play();
        }
    }

    public void Reset(float time)
    {
        StartCoroutine(ResetAfterTime(time));
    }

    public IEnumerator ResetAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        Reset();
    }

    [System.Serializable]
    public enum Mode
    {
        Transform,
        Vector3
    }
}
