using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonController : MonoBehaviour
{
    public GameObject balloonPrefab;
    public Transform spawnPoint;
    public float spawnInterval = 1.0f;
    private bool _spawnActive = false;
    public bool startSpawningOnAwake = true;
    public int numToSpawn = 100;
    public float finalBalloonScale = 1.0f;
    private int numSpawned = 0;
    public float totalSpawnTime = 0.0f;
    [Header("Colour Controls")]
    public Color balloonColour = Color.red;
    public bool randomizeColour = false;
    private float spawnTotal = 0.0f;

    [Header("Appliance")]
    public string Appliance;

    public float GetSpawnTotal()
    {
        return spawnTotal;
    }

    public bool SpawnActive
    {
        get
        {
            return _spawnActive;
        }
        set
        {
            _spawnActive = value;
            if (_spawnActive)
            {
                StartCoroutine(SpawnBalloons());
            }
            else
            {
                StopCoroutine(SpawnBalloons());
            }
        }
    }


    private void Awake()
    {
        if (startSpawningOnAwake)
        {
            SpawnActive = true;
        }
    }

    public void SpawnBalloons(float num)
    {
        SpawnActive = false;
        //DeleteBalloons();
        numSpawned = 0;
        numToSpawn = Mathf.CeilToInt(num);
        if (totalSpawnTime != 0.0f)
        {
            spawnInterval = totalSpawnTime / num;
        }
        finalBalloonScale = 1.0f;
        if (numToSpawn > num)
        {
            finalBalloonScale = num - (int)num;
        }
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
        while (_spawnActive && numSpawned < numToSpawn)
        {
            SpawnBalloon();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    public void DeleteBalloons()
    {
        Balloon[] balloons = GetComponentsInChildren<Balloon>();
        foreach(Balloon balloon in balloons)
        {
            GameObject.Destroy(balloon.gameObject);
        }
        numSpawned = 0;
        spawnTotal = 0.0f;
    }

    IEnumerator DestroyBalloonsCoroutine()
    {
        Balloon[] balloons = GetComponentsInChildren<Balloon>();
        foreach (Balloon balloon in balloons)
        {
            balloon.DestroyBalloon();
            yield return null;
        }
        numSpawned = 0;
        spawnTotal = 0.0f;
    }

    public void DestroyBalloons()
    {
        StartCoroutine(DestroyBalloonsCoroutine());
    }

    public void SpawnBalloon()
    {
        numSpawned++;
        GameObject newBalloon = GameObject.Instantiate(balloonPrefab, this.transform);
        newBalloon.GetComponent<Balloon>().balloonMaxScale = 1.0f / this.transform.lossyScale.x;
        float maxScale = newBalloon.GetComponent<Balloon>().balloonMaxScale;
        if (numSpawned == numToSpawn)
        {
            newBalloon.GetComponent<Balloon>().balloonMaxScale *= finalBalloonScale;
            spawnTotal += finalBalloonScale;
        }
        else
        {
            spawnTotal += 1.0f;
        }
        if (totalSpawnTime != 0.0f)
        {
            newBalloon.GetComponent<Balloon>().fillSpeed = (maxScale / newBalloon.GetComponent<Balloon>().balloonMaxScale) * 1.0f / spawnInterval;
        }
        newBalloon.transform.position = spawnPoint.position;
        newBalloon.GetComponentInChildren<MeshRenderer>().material.SetColor("_BaseColor",
            randomizeColour ? new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f)) : balloonColour);
    }
}
