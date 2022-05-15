using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Plot : MonoBehaviour
{
    public Type type;
    public Vector3 plotDimensions = Vector3.one;
    public LineRenderer lineRenderer;
    public GameObject pointPrefab;
    public MeshRenderer background;
    public Transform canvas;
    public Transform plotOrigin;

    [Header("Bar Plot")]
    public GameObject labelPrefab;
    public GameObject barPrefab;
    public Bin[] bins;
    public float barTextOffset = -0.05f;

    [Header("Line Plot")]
    public Vector3[] linePoints;
    [SerializeField]
    private Vector3[] scaledPoints;
    public bool showPoints = false;
    [Range(0.0f, 0.2f)]
    public float pointSize = 0.01f;
    [Range(0.0f, 1.0f)]
    public float pointSquash = 0.0f;

    [Header("Colours")]
    public Color backgroundColour;
    public Color lineColour;
    public Color pointColour;
    public Color barColour;

    [Header("Text")]
    public Text titleText;
    public Text xLabelText;
    public Text yLabelText;
    public Text xLimitText;
    public Text yLimitText;
    public string title, xLabel, yLabel;

    public void SetColours()
    {
        background.material.SetColor("_BaseColor", backgroundColour);
        lineRenderer.material.SetColor("_BaseColor", lineColour);
        pointPrefab.GetComponent<MeshRenderer>().sharedMaterial.SetColor("_BaseColor", pointColour);
        barPrefab.GetComponentInChildren<MeshRenderer>().sharedMaterial.SetColor("_BaseColor", barColour);
    }

    public void SetText()
    {
        titleText.text = title;
        xLabelText.text = xLabel;
        yLabelText.text = yLabel;
    }

    public Vector3 GetMinimum()
    {
        float minX = linePoints.Min(s => s.x);
        float minY = linePoints.Min(s => s.y);
        float minZ = linePoints.Min(s => s.z);
        /*
        if (minX == 0.0f)
        {
            minX = 1.0f;
        }
        if (minY == 0.0f)
        {
            minY = 1.0f;
        }
        if (minZ == 0.0f)
        {
            minZ = 1.0f;
        }
        */
        //xLimitText.text = minX.ToString("0.##");
        //yLimitText.text = minY.ToString("0.##");
        return new Vector3(minX, minY, minZ);
    }

    public Vector3 GetMaximum()
    {
        float maxX = linePoints.Max(s => s.x);
        float maxY = linePoints.Max(s => s.y);
        float maxZ = linePoints.Max(s => s.z);
        if (maxX == 0.0f)
        {
            maxX = 1.0f;
        }
        if (maxY == 0.0f)
        {
            maxY = 1.0f;
        }
        if (maxZ == 0.0f)
        {
            maxZ = 1.0f;
        }
        xLimitText.text = maxX.ToString("0.##");
        yLimitText.text = maxY.ToString("0.##");
        return new Vector3(maxX, maxY, maxZ);
    }

    public void ScalePoints()
    {
        Vector3 min = GetMinimum();
        Vector3 max = GetMaximum();
        scaledPoints = new Vector3[linePoints.Length];
        for (int i = 0; i < scaledPoints.Length; i++)
        {
            scaledPoints[i] = new Vector3(
                ((max.x - min.x) == 0.0f) ? 0.0f : (linePoints[i].x - min.x) * plotDimensions.x / (max.x - min.x),
                ((max.y - min.y) == 0.0f) ? 0.0f : (linePoints[i].y - min.y) * plotDimensions.y / (max.y - min.y),
                ((max.z - min.z) == 0.0f) ? 0.0f : (linePoints[i].z - min.z) * plotDimensions.z / (max.z - min.z));
        }
    }

    public void DrawScatterPlot(bool clear = true)
    {
        if (clear)
        {
            ClearPlot();
            ScalePoints();
            SetColours();
            SetText();
        }
        foreach (Vector3 point in scaledPoints)
        {
            GameObject newPoint = GameObject.Instantiate(pointPrefab, plotOrigin);
            newPoint.tag = "plotelement";
            newPoint.transform.localScale = new Vector3(pointSize, pointSize, pointSize * (1.0f - pointSquash));
            newPoint.transform.localPosition = point;
        }
    }

    public void DrawLinePlot()
    {
        ClearPlot();
        ScalePoints();
        SetColours();
        SetText();
        lineRenderer.positionCount = scaledPoints.Length;
        lineRenderer.SetPositions(scaledPoints);
        if (showPoints)
        {
            DrawScatterPlot(false);
        }
    }

    public void DrawBarPlot()
    {
        ClearPlot();
        SetColours();
        SetText();
        float maxValue = bins.Max(x => x.value);
        xLimitText.text = "";
        yLimitText.text = maxValue.ToString("0.##");

        float barWidth = plotDimensions.x / bins.Length;
        lineRenderer.positionCount = bins.Length * 4 + 1;
        Vector3[] positions = new Vector3[lineRenderer.positionCount];
        positions[0] = Vector3.zero;
        for(int i = 0; i < bins.Length; i++)
        {
            float value = (maxValue == 0.0f) ? 0.0f : bins[i].value * plotDimensions.y / maxValue;
            //Bottom Left
            positions[i * 4 + 1] = new Vector3(i * barWidth, 0.0f, 0.0f);

            //Top Left
            positions[i * 4 + 2] = new Vector3(i * barWidth, value, 0.0f);

            //Top Right
            positions[i * 4 + 3] = new Vector3((i + 1) * barWidth, value, 0.0f);
            
            //Bottom Right
            positions[i * 4 + 4] = new Vector3((i + 1) * barWidth, 0.0f, 0.0f);

            //Place Bar Object
            GameObject newBar = GameObject.Instantiate(barPrefab, plotOrigin);
            newBar.tag = "plotelement";
            newBar.transform.localPosition = new Vector3((i + 0.5f) * barWidth, 0.0f, 0.001f);
            newBar.transform.localScale = new Vector3(barWidth / 0.1f, value, 1.0f);
        }
        lineRenderer.SetPositions(positions);
        DrawBarText();
    }

    public void DrawBarText()
    {
        float barWidth = plotDimensions.x / bins.Length;
        Vector3 origin = new Vector3(-0.5f, -0.5f + barTextOffset, -0.005f);
        for (int i = 0; i < bins.Length; i++)
        {
            GameObject newLabel = GameObject.Instantiate(labelPrefab, canvas);
            newLabel.tag = "plotelement";
            newLabel.transform.localPosition = origin + (i + 0.5f) * barWidth * Vector3.right;
            newLabel.GetComponent<Text>().text = bins[i].label;
        }
    }

    public void ClearPlot()
    {
        lineRenderer.positionCount = 1;
        lineRenderer.SetPosition(0, Vector3.zero);
        Transform[] points = GetComponentsInChildren<Transform>();
        foreach(Transform point in points)
        {
            if (point.gameObject.tag.Equals("plotelement"))
            {
                GameObject.Destroy(point.gameObject);
            }
        }
    }

    public void DummyValues()
    {
        linePoints = new Vector3[20];
        for (int i = 0; i < linePoints.Length; i++)
        {
            linePoints[i] = i * Vector3.right * 0.1f + Mathf.Pow(i * 0.1f, 2) * Vector3.up;
        }
        bins = new Bin[10];
        for (int i = 0; i < bins.Length; i++)
        {
            bins[i] = new Bin("Bin" + i, Random.Range(0.0f, 10.0f));
        }
    }

    public void Awake()
    {
        DummyValues();
        DrawBarPlot();
    }


    [System.Serializable]
    public class Bin
    {
        public string label;
        public float value;

        public Bin(string inLabel, float inValue)
        {
            label = inLabel;
            value = inValue;
        }
    }

    [System.Serializable]
    public enum Type
    {
        Line,
        Scatter,
        Bar
    }
}
