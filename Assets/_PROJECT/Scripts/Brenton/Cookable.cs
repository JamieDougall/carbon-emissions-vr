using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cookable : MonoBehaviour
{
    public float Progress = 0.0f;
    public Color UncookedColour = Color.white;
    public Color CookedColour = Color.red;
    public Color BurntColour = Color.black;
    public MeshRenderer Rend;
    public float burnSpeed = 0.5f;

    public static Color GetColor(Color color1, Color color2, float factor)
    {
        float r = Mathf.Lerp(color1.r, color2.r, factor);
        float g = Mathf.Lerp(color1.g, color2.g, factor);
        float b = Mathf.Lerp(color1.b, color2.b, factor);
        return new Color(r, g, b);
    }
    
    public void Cook(float inc)
    {
        Progress += (Progress < 100.0f ? 1.0f : burnSpeed) * inc;
        Progress = Mathf.Clamp(Progress, 0.0f, 200.0f);

        Color color = (Progress < 100.0f) ? GetColor(UncookedColour, CookedColour, Progress / 100.0f) : GetColor(CookedColour, BurntColour, (Progress - 100.0f) / 100.0f);
        Rend.material.SetColor("_BaseColor", color);
    }
}
