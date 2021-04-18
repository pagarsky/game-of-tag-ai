using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject scenarioObject;
    public List<Scenario> scenarios;
    public int scenariosNumber;
    // summary: Main Class for handling game process
    void Awake()
    {
        scenarios = new List<Scenario>();
        Vector3 pos = Vector3.zero;

        for (int i = 0; i < scenariosNumber; i++)
        {
            GameObject obj = GameObject.Instantiate(scenarioObject);
            Scenario scenario = obj.GetComponent<Scenario>();

            obj.name = "Scenario " + i;
            obj.transform.position = pos;

            scenario.Setup(3, 3, pos);
            scenarios.Add(scenario);
            pos.x += Config.stageSpawnStep;
        }
    }

    void Start()
    {
        foreach (var s in scenarios)
        {
            s.Run();
        }
    }
}
