using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

//HELP FROM https://www.youtube.com/watch?v=VG8hLKyTiJQ
public class XRHand : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Rigidbody rb;
    [SerializeField] float positionSpeed = 1.0f;
    [SerializeField] float rotationSpeed = 1.0f;
    [SerializeField] float maxDistance = 4.0f;
    [SerializeField] float handActivationTimeAfterDrop = 2.0f;
    [SerializeField] bool handEnabled = true;
    [SerializeField] InputActionReference toggleRayInteractor;
    [SerializeField] GameObject directInteractor;
    [SerializeField] GameObject rayInteractor;

    [Header("Animation")]
    [SerializeField] Animator animator;
    [SerializeField] InputAction trigger;
    [SerializeField] InputAction grip;

    private void OnEnable()
    {
        if (toggleRayInteractor != null)
        {
            toggleRayInteractor.action.performed += (c) => ToggleRay(true);
            toggleRayInteractor.action.canceled += (c) => ToggleRay(false);
            toggleRayInteractor.action.Enable();
        }
        trigger.Enable();
        grip.Enable();
    }
    private void OnDisable()
    {
        if (toggleRayInteractor != null)
        {
            toggleRayInteractor.action.performed -= (c) => ToggleRay(true);
            toggleRayInteractor.action.canceled -= (c) => ToggleRay(false);
            toggleRayInteractor.action.Disable();
        }
        trigger.Disable();
        grip.Disable();
    }

    private void Update()
    {
        float t = trigger.ReadValue<float>();
        SetTriggerAnimation(t);
        float g = grip.ReadValue<float>();
        SetGripAnimation(g);
    }

    private void SetTriggerAnimation(float value)
    {
        animator.SetFloat("trigger", value);
    }

    private void SetGripAnimation(float value)
    {
        animator.SetFloat("grip", value);
    }


    private void ToggleRay(bool toggle)
    {
        directInteractor.SetActive(!toggle);
        rayInteractor.SetActive(toggle);
        target = toggle ? rayInteractor.transform : directInteractor.transform;
    }

    void ToggleColliders(bool toggle)
    {
        Collider[] colliders = rb.GetComponentsInChildren<Collider>();
        foreach (Collider collider in colliders)
        {
            if (!collider.isTrigger)
            {
                collider.enabled = toggle;
            }
        }
    }

    IEnumerator EnableHandAfterTime()
    {
        rb.gameObject.SetActive(true);
        ToggleColliders(false);
        yield return new WaitForSeconds(handActivationTimeAfterDrop);
        ToggleColliders(true);
    }

    public void ToggleHand(bool toggle)
    {
        handEnabled = toggle;
        if (handEnabled)
        {
            StartCoroutine(EnableHandAfterTime());
        }
        else
        {
            StopAllCoroutines();
            rb.gameObject.SetActive(false);
        }
    }

    void FixedUpdate()
    {
        if (!handEnabled)
        {
            return;
        }

        rb.velocity = (target.position - rb.position) / (Time.fixedDeltaTime / positionSpeed);

        if (Vector3.SqrMagnitude(target.position - rb.position) > maxDistance)
        {
            rb.position = target.position;
            rb.velocity = Vector3.zero;
        }

        Quaternion rotDiff = target.rotation * Quaternion.Inverse(rb.rotation);
        rotDiff.ToAngleAxis(out float angleInDegree, out Vector3 rotationAxis);
        Vector3 rotDiffInDegrees = angleInDegree * rotationAxis;
        Vector3 newAngularVelocity = rotDiffInDegrees * Mathf.Deg2Rad / (Time.fixedDeltaTime / rotationSpeed);
        if (float.IsInfinity(newAngularVelocity.x) || float.IsInfinity(newAngularVelocity.y) || float.IsInfinity(newAngularVelocity.z))
        {
            return;
        }
        else
        {
            //rb.angularVelocity = (rotDiffInDegrees * Mathf.Deg2Rad / (Time.fixedDeltaTime / rotationSpeed));
            rb.angularVelocity = newAngularVelocity;
        }
    }
}
