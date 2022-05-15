using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFunctions : MonoBehaviour
{
    public KeyCode popBalloonsKey = KeyCode.P;
    private BalloonController[] balloonControllers;

    private void Awake()
    {
        balloonControllers = GameObject.FindObjectsOfType<BalloonController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(popBalloonsKey))
        {
            PopAllBalloons();
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
            
    }

    public void PopAllBalloons()
    {
        foreach (BalloonController bc in balloonControllers)
        {
            bc.DestroyBalloons();
        }
    }
}
