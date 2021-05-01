using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Config
{
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

    public static string ChasersBehaviourName = "ChasersBehaviour";
    public static string RunnersBehaviourName = "RunnersBehaviour";

    public static float rayMaxDistance = 3000f;
    public static float spawnSpread = 45f;
    public static float stageSpawnStep = 100f;
    public static float forceMultiplier = 10f;
    public static float sigmoidK = 5f;

    public static Color rayColor = Color.yellow;
    public static int agentRaysAmount = 8;

    public static bool FULL_OBSERVABILITY = false;
}