using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class Balloon : MonoBehaviour
{
    public float fillSpeed = 1.0f;
    private float _fillState = 0.0f;
    public float balloonMaxScale = 0.2f;
    public Rigidbody rb;
    public float buoyancy = 1.0f;
    public bool buoyant = true;
    [Tooltip("Lifetime in seconds")]
    public float smokeLifetime = 60.0f;

    public GameObject smokePrefab;
    public Transform smokeSpawnPoint;
    public GameObject nubPrefab;
    public Transform nubSpawnPoint;
    public UnityEvent onFilled;
    public bool detachFromParent = false;

    public XRGrabInteractable grabInteractable;

    private void Awake()
    {
        if (_fillState < 1.0f)
        {
            //rb.isKinematic = true;
            StartCoroutine(Fill());
        }
        else
        {
            rb.isKinematic = false;
            StartCoroutine(KeepUp());
        }
    }

    public void DestroyBalloon()
    {
        if (smokePrefab != null)
        {
            GameObject newSmoke = GameObject.Instantiate(smokePrefab, smokeSpawnPoint.position, smokeSpawnPoint.rotation);
            GameObject.Destroy(newSmoke, 60.0f);
        }
        
        GameObject newNub = GameObject.Instantiate(nubPrefab, nubSpawnPoint.position, nubSpawnPoint.rotation);
        GameObject.Destroy(newNub, 60.0f);

        GameObject.Destroy(gameObject);
    }

    IEnumerator KeepUp()
    {
        while (buoyant)
        {
            rb.AddForce(Vector3.up * buoyancy * Time.deltaTime * 90.0f, ForceMode.Force);
            yield return null;
        }
    }

    IEnumerator Fill()
    {
        Vector3 startPosition = transform.position;
        while(_fillState < 1.0f)
        {
            _fillState += Time.deltaTime * fillSpeed;
            if (_fillState > 1.0f)
            {
                _fillState = 1.0f;
            }

            transform.localScale = Vector3.one * _fillState * balloonMaxScale;

            if (transform.parent != null)
            {
                rb.position = transform.parent.position;
            }
            else
            {
                rb.position = startPosition;
            }
            yield return null;
        }
        
        rb.isKinematic = false;

        onFilled.Invoke();
        if (detachFromParent)
        {
            transform.parent = null;
        }
        StartCoroutine(KeepUp());
        grabInteractable.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<BalloonPopper>() != null)
        {
            DestroyBalloon();
        }
    }
}
