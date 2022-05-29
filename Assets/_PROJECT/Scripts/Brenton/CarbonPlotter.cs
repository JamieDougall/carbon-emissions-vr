using System.Linq;
using UnityEngine;

public class CarbonPlotter : MonoBehaviour
{
    public float carbonPerBalloon;
    public Plot plot;
    private BalloonController[] balloonControllers;
    public float refreshInterval;
    private float timer;

    public void UpdatePlot()
    {
        if (balloonControllers == null)
        {
            balloonControllers = GameObject.FindObjectsOfType<BalloonController>().Where(p => !string.IsNullOrWhiteSpace(p.Appliance)).ToArray();
        }
        plot.bins = balloonControllers.Select(s => new Plot.Bin(s.Appliance, s.GetSpawnTotal() * carbonPerBalloon)).ToArray();
        plot.title = "Your Household Carbon Emissions";
        plot.xLabel = "Appliances";
        plot.yLabel = "Carbon Emissions (kg CO2 equivalent)";
        plot.DrawBarPlot();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > refreshInterval)
        {
            timer = 0.0f;
            UpdatePlot();
        }
    }
}
