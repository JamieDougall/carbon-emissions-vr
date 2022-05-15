using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class DataController : MonoBehaviour
{
    public TextAsset Data;
    public DataPoint[] DataPoints;
    public static Dictionary<string, float> NumBalloons = new Dictionary<string, float>();
    public bool debug = false;

    public static float GetNumBalloons(string appliance)
    {
        float num = 0.0f;
        NumBalloons.TryGetValue(appliance, out num);
        return num;
    }

    private void Start()
    {
        LoadData();
    }

    public void LoadData()
    {
        string[] lines = Data.text.Split("\n"[0]);
        //app	mean_kW	active	category	room	data_source_link	data_download_date	attribute_notes_html_link	attribute_notes_docx_link	attribute_notes_download_date	energy_units_kW	hrs_per_use	per_use_multiplier	use_per_day	expected_use_W	star	df_id	notes	co2_kgs_per_use	balloons_per_use
        //name = 0
        //numballoons = 19
        //lines 1 to 19
        List<DataPoint> data = new List<DataPoint>();
        NumBalloons.Clear();
        int currentLine = 0;
        foreach (string line in lines)
        {
            try
            {
                string[] split = line.Split(',');
                DataPoint newDataPoint = new DataPoint()
                {
                    Appliance = split[0],
                    NumBalloons = float.Parse(split[19])
                };
                data.Add(newDataPoint);
                NumBalloons.Add(newDataPoint.Appliance, newDataPoint.NumBalloons);
            }
            catch
            {
                if (debug)
                {
                    Debug.Log("Line " + currentLine + " invalid");
                }
            }
            currentLine++;
        }
        DataPoints = data.ToArray();
    }

    [System.Serializable]
    public class DataPoint
    {
        public string Appliance;
        public float NumBalloons;
    }
}
