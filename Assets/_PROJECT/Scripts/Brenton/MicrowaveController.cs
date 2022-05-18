using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MicrowaveController : MonoBehaviour
{
    public AudioSource closeAudio, openAudio, beepAudio, runAudio, doneAudio;
    public Light microwaveLight;
    public bool isRunning = false;
    public bool isClosed = true;
    public float timer = 60.0f;
    public float doneMessageTimer = 3.0f;
    public float flickerFrequency, flickerAmplitude;
    //public float linearMapping; //used to be LinearMapping class from Valve
    public XRDoor door;
    public Text text;

    //public float powerMapping; //used to be LinearMapping class from Valve
    public FloatVariable microwavePower;
    //private float microwavePower;
    public Image[] powerImages;
    public UnityEvent onStart;

    [Header("Plate")]
    public Transform plate;
    public float plateRotationSpeed = 1.0f;
    public bool rotatePlate = true;
    
    [Header("Cooking Controls")]
    public bool canCook = false;
    public float cookingSpeed = 1.0f;
    public Color cookedColour = Color.magenta;
    public Transform cookOrigin;
    public float cookRadius = 1.0f;
    float lightBasePower = 0.3f;

    public void StartMicrowave()
    {
        beepAudio.Play();
        if (isClosed)
        {
            if (isRunning)
            {
                timer += 30.0f;
            }
            else
            {
                isRunning = true;
                StartCoroutine(RunMicrowave());
                onStart.Invoke();
            }
        }
    }

    public void Update()
    {
        //microwavePower = 0;//powerMapping.value;
        int powerMax = (int)(microwavePower.RuntimeValue * powerImages.Length);
        for (int i = 0; i < powerImages.Length; i++)
        {
            powerImages[i].enabled = (i < powerMax);
        }


        //if (isClosed & linearMapping > 0.0f)
        if (isClosed & !door.isClosed)
        {
            isClosed = false;
            isRunning = false;
            openAudio.Play();
        }
        //else if(!isClosed && linearMapping == 0.0f)
        else if (!isClosed && door.isClosed)
        {
            isClosed = true;
            closeAudio.Play();
        }
    }

    private void SetTimerText()
    {
        text.text = System.TimeSpan.FromSeconds(timer).ToString(@"m\:ss");
    }

    private void SetTimerText(string msg)
    {
        text.text = msg;
    }

    private void Cook()
    {
        RaycastHit[] hits = Physics.SphereCastAll(cookOrigin.position, cookRadius, Vector3.forward, 0.0f);
        if (hits != null && hits.Length > 0)
        {
            foreach(RaycastHit hit in hits)
            {
                Cookable cookable = hit.transform.GetComponent<Cookable>();
                if (cookable != null)
                {
                    cookable.Cook(cookingSpeed * microwavePower.RuntimeValue);
                    cookable.transform.RotateAround(plate.position, transform.up, plateRotationSpeed * Time.deltaTime);
                }
            }
        }
    }

    IEnumerator ShowDoneMessage()
    {
        SetTimerText("DONE");
        yield return new WaitForSeconds(doneMessageTimer);
        SetTimerText();
    }

    IEnumerator RunMicrowave()
    {
        runAudio.Play();
        //float lightBasePower = microwaveLight.intensity;
        microwaveLight.enabled = true;
        while (isRunning)
        {
            timer -= Time.deltaTime;
            SetTimerText();
            if (rotatePlate)
            {
                plate.localEulerAngles = new Vector3(plate.localEulerAngles.x, (plate.localEulerAngles.y + plateRotationSpeed * Time.deltaTime) % 360f, plate.eulerAngles.z);
            }
            if (canCook)
            {
                Cook();
            }
            microwaveLight.intensity = lightBasePower + (lightBasePower * flickerAmplitude * Mathf.Sin(timer * flickerFrequency));
            if (timer <= 0.0f)
            {
                isRunning = false;
                StartCoroutine(ShowDoneMessage());
                doneAudio.Play();
            }
            yield return null;
        }
        timer = 60.0f;
        runAudio.Stop();
        microwaveLight.enabled = false;
        microwaveLight.intensity = lightBasePower;
    }
}
