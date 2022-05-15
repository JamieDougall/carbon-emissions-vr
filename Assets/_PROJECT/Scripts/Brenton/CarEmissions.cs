using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SQLite4Unity3d;
using System.IO;
using System.Linq;
using UnityEditor;

[ExecuteInEditMode]
public class CarEmissions : MonoBehaviour
{
    //private SQLiteConnection db;
    private string dbPath { get { return Path.Combine(Application.streamingAssetsPath, "vehicles.db"); } }
    public const double mpgToKmPerLitre = 0.425144;
    public string[] makes;
    public string[] models;
    public int[] years;
    public int selectedMake = 0;
    public int selectedModel = 0;
    public int selectedYear = 0;
    public Plot plot;

    public void DrawPlot()
    {
        using (SQLiteConnection db = new SQLiteConnection(dbPath))
        {
            var query = db.Query<DataPoint>("select make, model, year, city08, highway08, trany, displ from vehicles where make = ? and model = ?", makes[selectedMake], models[selectedModel]);
            if (query.Count == 0)
            {
                Debug.Log("No valid entries");
            }
            List<Vector3> plotPoints = new List<Vector3>();
            foreach (DataPoint dp in query)
            {
                plotPoints.Add(new Vector3(dp.year, (float)dp.city08));
            }
            plot.linePoints = plotPoints.ToArray();
            if (Application.isPlaying)
            {
                plot.title = string.Format("km Per Litre for {0} {1} range", makes[selectedMake], models[selectedModel]);
                plot.xLabel = "Year";
                plot.yLabel = "km/L";
                plot.DrawScatterPlot();
            }
        }

    }
    
    public void GetKMPerLitre()
    {
        using (SQLiteConnection db = new SQLiteConnection(dbPath))
        {
            var query = db.Query<DataPoint>("select make, model, year, city08, highway08, trany, displ from vehicles where make = ? and model = ? and year = ?", makes[selectedMake], models[selectedModel], years[selectedYear].ToString());
            if (query.Count == 0)
            {
                Debug.Log("No valid entries");
            }
            foreach(DataPoint dp in query)
            {
                Debug.Log(string.Format("{0} {1} {2}, \nCity: {3} km/L\nHighway: {4}km/L\nTransmission: {5}\nDisplacement: {6} L", dp.year, dp.make, dp.model, dp.city08 * mpgToKmPerLitre, dp.highway08 * mpgToKmPerLitre, dp.trany, dp.displ));
            }
        }
    }

    public void LoadYears()
    {
        using (SQLiteConnection db = new SQLiteConnection(dbPath))
        {
            var query = db.Query<DataPoint>("select distinct year from vehicles where make = ? and model = ?", makes[selectedMake], models[selectedModel]);
            years = query.Select(s => s.year).ToArray();
        }
    }
    
    public void LoadModels()
    {
        using (SQLiteConnection db = new SQLiteConnection(dbPath))
        {
            models = db.Query<DataPoint>("select distinct model from vehicles where make = ?", @makes[selectedMake]).Select(s => s.model/* + " " + s.year*/).OrderBy(q=>q).ToArray();
        }
    }

    public void LoadMakes()
    {
        using (SQLiteConnection db = new SQLiteConnection(dbPath))
        {
            makes = db.Query<DataPoint>("select make from vehicles").Select(s => s.make).Distinct().OrderBy(q => q).ToArray();
        }
    }

    public void SaveMakes()
    {
        using (var file = File.CreateText(Path.Combine(Application.streamingAssetsPath, "vehicle-makes.csv")))
        {
            foreach (string make in makes)
            {
                file.WriteLine(make);
            }
        }
    }

    [System.Serializable]
    public class DataPoint
    {
        public string make { get; set; }
        public string model { get; set; }
        public int year { get; set; }
        public double city08 { get; set; }
        public double highway08 { get; set; }
        public string trany { get; set; }
        public string displ { get; set; }
    }
}
