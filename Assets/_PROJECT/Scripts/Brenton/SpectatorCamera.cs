using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpectatorCamera : MonoBehaviour
{
    public Camera spectator;
    TransformMimic mimic;

    void Start()
    {
        mimic = GetComponent<TransformMimic>();
        spectator.stereoTargetEye = StereoTargetEyeMask.None;
    }

    public void ChangeSmoothness(float inc)
    {
        mimic.delay += inc;
    }

    public void SetSmoothness(float smoothness)
    {
        mimic.delay = smoothness;
    }
    public void ChangeSmoothness(Slider slider)
    {
        mimic.delay = slider.value;
    }

    public void ChangeFOV(Slider slider)
    {
        spectator.fieldOfView = slider.value;
    }

}
