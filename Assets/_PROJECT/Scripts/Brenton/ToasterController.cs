using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Events;

public class ToasterController : MonoBehaviour
{
    [Header("Search")]
    public Transform toastableSearchOrigin;
    public float toastableSearchRadius;
    public Toastable toastable;
    private Transform toastableOriginalParent;

    [Header("Lock")]
    public Transform toastableLockOrigin;
    public float lockSpeed = 1.0f;
    public Transform bottomCollider;

    [Header("Cooking")]
    public bool enableCooking = true;
    private bool isCooking = false;
    public float cookSpeed = 0.3f;
    public float cookTime = 30.0f;
    public float cookTimer = 30.0f;
    public MeshRenderer[] cookingElements;
    public float elementHeatSpeed = 1.0f;
    public Color elementColourCool;
    public Color elementColourHot;
    public UnityEvent onFinishedCooking;

    //[Header("Toaster Button")]
    //public HoverButton hoverButton;

    [Header("Audio")]
    public AudioSource toasterLatch;
    public AudioSource toasterBuzz;

    private Coroutine changeElementColour = null;
    private Coroutine positionToastable = null;
    private float elementProgress = 0.0f;

    [Header("Balloons")]
    public BalloonController balloonController;


    // Update is called once per frame
    void Update()
    {
        //bottomCollider.localPosition = hoverButton.movingPart.localPosition;
        if (enableCooking && isCooking)
        {
            cookTimer += Time.deltaTime;
            if (cookTimer > cookTime)
            {
                isCooking = false;
                if (toastable != null)
                {
                    ReleaseToastable();
                    toasterLatch.Play();
                }
                onFinishedCooking.Invoke();
                if (changeElementColour == null)
                {
                    changeElementColour = StartCoroutine(ChangeElementColour());
                }
            }
            Cook();
        }
    }

    public void CheckForToast()
    {
        toasterLatch.Play();
        RaycastHit[] hits = Physics.SphereCastAll(toastableSearchOrigin.position, toastableSearchRadius, Vector3.up, 0.0f);
        if (hits != null & hits.Length > 0)
        {
            foreach(RaycastHit hit in hits)
            {
                Toastable toast = hit.transform.GetComponentInParent<Toastable>();
                //Interactable interactable = hit.transform.GetComponentInParent<Interactable>();
                if (toast != null /*&& interactable != null*/)
                {
                    toastable = toast;
                    //interactable.enabled = false;
                    toast.GetComponent<Rigidbody>().isKinematic = true;
                    LockToastable();
                    ToggleCooking(true);
                    return;
                }
                else
                {
                    onFinishedCooking.Invoke();
                }
            }
        }
    }

    public void SpawnBalloons()
    {
        balloonController.totalSpawnTime = cookTime;
        balloonController.SpawnApplianceBalloons();
    }

    IEnumerator ChangeElementColour()
    {
        //float progress = 0.0f;
        bool changingColour = true;
        bool heating = isCooking;
        while (changingColour)
        {
            if(isCooking)
            {
                elementProgress += elementHeatSpeed * Time.deltaTime;
            }
            else
            {
                elementProgress -= elementHeatSpeed * Time.deltaTime;
            }
            elementProgress = Mathf.Clamp(elementProgress, 0.0f, 100.0f);

            if (elementProgress == 100.0f || elementProgress == 0.0f)
            {
                changingColour = false;
            }
            foreach(MeshRenderer element in cookingElements)
            {
                element.material.SetColor("_BaseColor", Cookable.GetColor(elementColourCool, elementColourHot, elementProgress / 100.0f));
                element.material.SetColor("_EmissionColor", new Color(Mathf.Lerp(0.0f, 1.0f, elementProgress / 100.0f), 0.0f, 0.0f));
            }
            yield return null;
        }
        changeElementColour = null;
    }

    IEnumerator PositionToastable()
    {
        float moveTimer = 0.0f;
        bool inPosition = false;
        Transform toast = toastable.transform;
        Vector3 startPosition = toast.localPosition;
        Quaternion startRotation = toast.localRotation;
        while (!inPosition)
        {
            moveTimer += lockSpeed * Time.deltaTime;
            if (moveTimer > 1.0f)
            {
                moveTimer = 1.0f;
                inPosition = true;
            }
            toast.localPosition = Vector3.Lerp(startPosition, Vector3.zero, moveTimer);
            toast.localRotation = Quaternion.Slerp(startRotation, Quaternion.identity, moveTimer);
            yield return null;
        }
        positionToastable = null;
    }
    
    public void LockToastable()
    {
        //hoverButton.enabled = false;
        //hoverButton.movingPart.localPosition = hoverButton.localMoveDistance;
        toastableOriginalParent = toastable.transform.parent;
        toastable.transform.GetComponent<Rigidbody>().isKinematic = true;
        //toastable.transform.GetComponent<Interactable>().enabled = false;
        toastable.transform.GetComponent<Collider>().enabled = false;
        toastable.transform.SetParent(toastableLockOrigin);
        positionToastable = StartCoroutine(PositionToastable());
    }

    public void ReleaseToastable()
    {
        toastable.transform.GetComponent<Rigidbody>().isKinematic = false;
        //toastable.transform.GetComponent<Interactable>().enabled = true;
        toastable.transform.GetComponent<Collider>().enabled = true;
        toastable.transform.SetParent(toastableOriginalParent);
        //hoverButton.enabled = true;
        ToggleCooking(false);
    }

    public void ToggleCooking(bool toggle)
    {
        cookTimer = 0.0f;
        isCooking = toggle;
        if (isCooking)
        {
            toasterBuzz.Play();
            SpawnBalloons();
        }
        else
        {
            toasterBuzz.Stop();
        }
        if (changeElementColour == null)
        {
            changeElementColour = StartCoroutine(ChangeElementColour());
        }
    }

    public void Cook()
    {
        if (toastable != null)
        {
            toastable.Cook(cookSpeed * Time.deltaTime);
        }
    }
}
