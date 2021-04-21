using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class Utils
{

    public delegate List<AgentController> ListAgentDelegate();

    public static Vector3 RandomVector3(float min, float max)
    {
        return new Vector3(
            Random.Range(min, max),
            Random.Range(min, max),
            Random.Range(min, max)
        );
    }

    public static Vector3 RandomVector3(float Min, float Max, float yMin = 0, float yMax = 5)
    {
        return new Vector3(
            Random.Range(Min, Max),
            Random.Range(yMin, yMax),
            Random.Range(Min, Max)
        );
    }
}