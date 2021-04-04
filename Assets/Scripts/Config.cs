using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Config
{
    //public static Dictionary<string, Material> Colors = new Dictionary<string, Material>()
    //{
    //    {"Chasers", Resources.Load<Material>("Materials/ChasersMaterial")},
    //    {"Runners", Resources.Load<Material>("Materials/RunnersMaterial")}
    //};

    public enum Teams
    {
        CHASERS,
        RUNNERS
    }

    public enum Layers
    {
        DEFAULT = 0,
        IGNORE_RAYCAST = 2,
    }

    public static int agentRaysAmount = 15;
    public static float rayMaxDistance = 4e10f;
    public static Color rayColor = Color.yellow;

}