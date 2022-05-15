using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LoopingTimer : MonoBehaviour
{
    public int timeToWait;
    public bool beginOnStart = false;
    public UnityEvent eventToTrigger;
    private bool isRunning = false;
    void Start()
    {
        if(beginOnStart)
            StartTimerLoop();
    }

    public void StartTimerLoop()
    {
        if(!isRunning)
        {
            isRunning = true;
            StartCoroutine(TimerLoop());
        }
    }

    public void StopTimerLoop()
    {
        isRunning = false;
        StopCoroutine(TimerLoop());
    }

    IEnumerator TimerLoop()
    {
        while(isRunning)
        {
            yield return new WaitForSeconds(timeToWait);
            eventToTrigger.Invoke();
        }
    }
}
