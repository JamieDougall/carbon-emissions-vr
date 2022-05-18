using System.Collections;
using UnityEngine;

public class BalloonController : MonoBehaviour
{
    public GameObject balloonPrefab;
    public Transform spawnPoint;
    public bool detachBalloonsFromParent = false;
    //public bool parentToSpawnPoint = false;
    public float spawnInterval = 1.0f;
    public bool startSpawningOnAwake = true;
    public int numToSpawn = 100;
    public float finalBalloonScale = 1.0f;
    public float totalSpawnTime = 0.0f;
    [Header("Colour Controls")]
    public Color balloonColour = Color.red;
    public bool randomizeColour = false;
    public float randomRotationVariation = 5.0f;

    [Header("Appliance")]
    public string Appliance;
    private bool _spawnActive = false;
    private int _numSpawned = 0;
    private float _spawnTotal = 0.0f;

    private void Awake()
    {
        if (startSpawningOnAwake)
            SpawnActive = true;
    }

    public float GetSpawnTotal(){ return _spawnTotal; }

    public bool SpawnActive
    {
        get
        {
            return _spawnActive;
        }
        set
        {
            _spawnActive = value;
            StopCoroutine(SpawnBalloons());
            if (_spawnActive)
            {
                StartCoroutine(SpawnBalloons());
            }
            /*
            else
            {
                StopCoroutine(SpawnBalloons());
            }
            */
        }
    }

    public void SpawnBalloons(float num)
    {
        SpawnActive = false;
        //DeleteBalloons();
        _numSpawned = 0;
        numToSpawn = Mathf.CeilToInt(num);

        if (totalSpawnTime != 0.0f)
            spawnInterval = totalSpawnTime / num;
    
        finalBalloonScale = 1.0f;

        if (numToSpawn > num)
            finalBalloonScale = num - (int)num;

        SpawnActive = true;
    }

    public void SpawnBalloons(string appliance)
    {
        SpawnBalloons(DataController.GetNumBalloons(appliance));
    }

    public void SpawnApplianceBalloons()
    {
        SpawnBalloons(Appliance);
    }

    IEnumerator SpawnBalloons()
    {
        while (_spawnActive && _numSpawned < numToSpawn)
        {
            SpawnBalloon();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    public void DeleteBalloons()
    {
        Balloon[] balloons = spawnPoint.GetComponentsInChildren<Balloon>();
        //Balloon[] balloons = GameObject.FindObjectsOfType<Balloon>();

        foreach (Balloon balloon in balloons)
        {
            GameObject.Destroy(balloon.gameObject);
        }

        _numSpawned = 0;
        _spawnTotal = 0.0f;
    }

    IEnumerator DestroyBalloonsCoroutine()
    {
        Balloon[] balloons = spawnPoint.GetComponentsInChildren<Balloon>();
        //Balloon[] balloons = GameObject.FindObjectsOfType<Balloon>();

        foreach (Balloon balloon in balloons)
        {
            balloon.DestroyBalloon();
            yield return null;
        }
        _numSpawned = 0;
        _spawnTotal = 0.0f;
    }

    public void DestroyBalloons()
    {
        StartCoroutine(DestroyBalloonsCoroutine());
    }

    public void SpawnBalloon()
    {
        _numSpawned++;

        GameObject newBalloon = GameObject.Instantiate(balloonPrefab);
        Balloon newBalloonScript = newBalloon.GetComponent<Balloon>();

        newBalloon.transform.parent = spawnPoint;
        newBalloonScript.detachFromParent = detachBalloonsFromParent;

        newBalloonScript.balloonMaxScale = 1.0f / this.transform.lossyScale.x;
        float maxScale = newBalloonScript.balloonMaxScale;

        if (_numSpawned == numToSpawn)
        {
            newBalloonScript.balloonMaxScale *= finalBalloonScale;
            _spawnTotal += finalBalloonScale;
        }
        else
        {
            _spawnTotal += 1.0f;
        }

        if (totalSpawnTime != 0.0f)
            newBalloon.GetComponent<Balloon>().fillSpeed = (maxScale / newBalloonScript.balloonMaxScale) * 1.0f / spawnInterval;

        newBalloon.transform.position = spawnPoint.position;
        //newBalloon.transform.localRotation *= Quaternion.Euler(Random.Range(0.0f, randomRotationVariation), Random.Range(0.0f, randomRotationVariation), Random.Range(0.0f, randomRotationVariation));
        newBalloon.transform.localEulerAngles = new Vector3(Random.Range(0.0f, randomRotationVariation), Random.Range(0.0f, randomRotationVariation), Random.Range(0.0f, randomRotationVariation));
        newBalloon.GetComponentInChildren<MeshRenderer>().material.SetColor("_BaseColor",
            randomizeColour ? new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f)) : balloonColour);
    }
}
