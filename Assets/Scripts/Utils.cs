using System.Linq;
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

    public static Vector3 RandomVector3(float Min, float Max, float height)
    {
        return new Vector3(
            Random.Range(Min, Max),
            height,
            Random.Range(Min, Max)
        );
    }


    public static void Normalize(ref float[] x, float a, float b)
    {
        float max = Enumerable.Max(x);
        float min = Enumerable.Min(x);

        float diff = (max - min) != 0f ? max - min : 1;

        for (int i = 0; i < x.Length; i++)
        {
            x[i] = (b - a) * (x[i] - min) / diff + a;
        }
    }

    public static float Sigmoid(float x, float k = 1)
    {
        return 1f / (1 + Mathf.Exp(-k * x));
    }
}