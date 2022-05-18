using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class XRDrawer : MonoBehaviour
{
    [SerializeField] Vector3 axis = Vector3.forward;
    [SerializeField] float drawerLimit = 0.47f;
    [SerializeField] float closeDistance = 0.05f;
    float drawerDistance = 0.0f;
    [SerializeField] Rigidbody rb;
    Vector3 startPosition = Vector3.zero;
    [SerializeField] bool isClosed = false;
    [SerializeField] UnityEvent onClosed;
    [SerializeField] UnityEvent onOpened;
    [SerializeField] AudioSource closeAudio;
    [SerializeField] AudioSource openAudio;
    [SerializeField] bool grabbed = false;
    [SerializeField] XRSimpleInteractable interactable;
    [SerializeField] float positionSpeed = 1.0f;
    [SerializeField] Transform drawerRoot;
    float lastHandDistance = 0.0f;


    private void OnValidate()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }
        axis.Normalize();
    }

    private void Start()
    {
        startPosition = transform.localPosition;
    }

    public void Grab()
    {
        Vector3 handPosition = drawerRoot.InverseTransformPoint(interactable.firstInteractorSelecting.transform.position);
        lastHandDistance = axis.x * handPosition.x + axis.y * handPosition.y + axis.z * handPosition.z;
        grabbed = true;
    }

    public void Release()
    {
        grabbed = false;
        if (!isClosed)
        {
            rb.isKinematic = false;
            /*
            Vector3 handPosition = drawerRoot.InverseTransformPoint(interactable.firstInteractorSelecting.transform.position);
            float handDistance = axis.x * handPosition.x + axis.y * handPosition.y + axis.z * handPosition.z;
            rb.velocity = (handDistance - lastHandDistance) * axis;
            */
        }
    }

    private void ExecuteGrabbed()
    {
        Vector3 handPosition = drawerRoot.InverseTransformPoint(interactable.firstInteractorSelecting.transform.position);
        float handDistance = axis.x * handPosition.x + axis.y * handPosition.y + axis.z * handPosition.z;
        transform.localPosition = (drawerDistance + handDistance - lastHandDistance) * axis;
        lastHandDistance = handDistance;
        //rb.velocity = (interactable.firstInteractorSelecting.transform.position /*target.position*/ - rb.position) / (Time.fixedDeltaTime / positionSpeed);

    }

    private void FixedUpdate()
    {
        if (grabbed)
        {
            rb.isKinematic = true;
            ExecuteGrabbed();
        }
        drawerDistance = transform.localPosition.x * axis.x + transform.localPosition.y * axis.y + transform.localPosition.z * axis.z;
        if (drawerDistance < closeDistance)
        {
            //Closed
            if (isClosed)
            {
                //Already closed
            }
            else
            {
                //Closed this frame
                onClosed.Invoke();
                if (closeAudio != null)
                {
                    closeAudio?.Play();
                }
                rb.isKinematic = true;
            }
            //rb.position = transform.TransformPoint(startPosition);
            //rb.position = transform.TransformPoint(Vector3.zero);
            transform.localPosition = Vector3.zero;
            isClosed = true;
        }
        else
        {
            //Open
            if (isClosed)
            {
                //Opened this frame
                isClosed = false;
                onOpened.Invoke();
                if (openAudio != null)
                {
                    openAudio?.Play();
                }
                rb.isKinematic = false;
            }
            else
            {
                //Already open
            }
            isClosed = false;
        }
        if (drawerDistance > drawerLimit)
        {
            //rb.position = transform.TransformPoint(/*startPosition +*/ axis * drawerLimit);
            transform.localPosition = axis * drawerLimit;
        }
    }
}
