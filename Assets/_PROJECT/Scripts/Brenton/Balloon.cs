using System.Collections;
using UnityEngine;
using UnityEngine.Events;

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

    private void Awake()
    {
        if (_fillState < 1.0f)
        {
            rb.isKinematic = true;
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
        //GameObject newSmoke = GameObject.Instantiate(smokePrefab, smokeSpawnPoint.position, smokeSpawnPoint.rotation);
        //GameObject.Destroy(newSmoke, 60.0f);
        
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
        while(_fillState < 1.0f)
        {
            _fillState += Time.deltaTime * fillSpeed;
            if (_fillState > 1.0f)
            {
                _fillState = 1.0f;
            }

            transform.localScale = Vector3.one * _fillState * balloonMaxScale;
            yield return null;
        }
        
        rb.isKinematic = false;
        onFilled.Invoke();
        StartCoroutine(KeepUp());
    }

    private void OnTriggerEnter(Collider other)
    {
        DestroyBalloon();
    }
}
