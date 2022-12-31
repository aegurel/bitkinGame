using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : Singleton<Plant>
{
    private bool expired;
    private int experimentTime;
    private int waterLevel;
    private int moistureLevel;
    private int sunLevel;
    private int healthLevel;

    public int WaterLevel { get => waterLevel; set => waterLevel = value; }
    public int MoistureLevel { get => moistureLevel; set => moistureLevel = value; }
    public int SunLevel { get => sunLevel; set => sunLevel = value; }
    public int HealthLevel { get => healthLevel; set => healthLevel = value; }
    public bool Expired { get => expired; set => expired = value; }
    public int ExperimentTime { get => experimentTime; set => experimentTime = value; }
}
