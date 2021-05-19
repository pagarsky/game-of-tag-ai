using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Barracuda;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // summary: Main Class for handling game process

    public GameObject scenarioObject;
    public GameObject stagePrefab;
    public List<Scenario> scenarios;

    public NNModel runnerModel;
    public NNModel chaserModel;

    public int scenariosNumber;
    public int scenariosRows;
    public int agentsPerTeam;
    
    void Awake()
    {
        scenarios = new List<Scenario>();
        Vector3 pos = Vector3.zero;

        for (int j = 0; j < scenariosRows; j++)
        {
            for (int i = 0; i < scenariosNumber; i++)
            {
                GameObject obj = GameObject.Instantiate(scenarioObject);
                Scenario scenario = obj.GetComponent<Scenario>();

                obj.name = "Scenario " + j + i;
                obj.transform.position = pos;

                scenario.Setup(agentsPerTeam, agentsPerTeam, pos, stagePrefab);
                scenarios.Add(scenario);
                pos.x += Config.stageSpawnStep;
            }
            pos.x = 0;
            pos.z += Config.stageSpawnStep;
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
