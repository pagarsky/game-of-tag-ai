using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class Scenario : MonoBehaviour
{
    private List<AgentController> _chasersList = new List<AgentController>();
    private List<AgentController> _runnersList = new List<AgentController>();

    //Unity.MLAgents.SimpleMultiAgentGroup _chasers = new Unity.MLAgents.SimpleMultiAgentGroup();
    //Unity.MLAgents.SimpleMultiAgentGroup _runners = new Unity.MLAgents.SimpleMultiAgentGroup();

    public int ChasersAmount { get; private set; }
    public int RunnersAmount { get; private set; }
    public Vector3 globalPos {
        get
        {
            return this._stage.transform.position;
        }
    }

    public Material[] materials = new Material[2];
    public GameObject agentPrefab;
    public GameObject stagePrefab;

    private GameObject _stage;

    public void Setup(int chasers, int runners, Vector3 globalPosition, GameObject stage)
    {
        ChasersAmount = chasers;
        RunnersAmount = runners;

        this._stage = GameObject.Instantiate(stage, this.gameObject.transform);
        this._stage.transform.position = globalPosition;
    }

    public void Run()
    {
        SpawnAgents();
    }

    private void SpawnAgents()
    {
        for (int i = 0; i < RunnersAmount; i++)
        {
            GameObject runner = GameObject.Instantiate(agentPrefab, this.gameObject.transform);

            Vector3 position = _GetSpawnablePosition(globalPos);
            runner.transform.position = position;

            AgentController controller = runner.GetComponent<AgentController>();
            controller.Initialize(materials[0], GetChasers, Config.Teams.RUNNERS);

            _runnersList.Add(controller);
            //_runners.RegisterAgent(controller);
        }
        for (int i = 0; i < ChasersAmount; i++)
        {
            GameObject chaser = GameObject.Instantiate(agentPrefab, this.gameObject.transform);

            Vector3 position = _GetSpawnablePosition(globalPos);
            chaser.transform.position = position;

            AgentController controller = chaser.GetComponent<AgentController>();
            controller.Initialize(materials[1], GetRunners, Config.Teams.CHASERS);

            _chasersList.Add(controller);
            //_chasers.RegisterAgent(controller);
        }
    }

    public List<AgentController> GetRunners()
    {
        return _runnersList;
    }

    public List<AgentController> GetChasers()
    {
        return _chasersList;
    }

    private Vector3 _GetSpawnablePosition(Vector3 globalPos)
    {
        Vector3 pos;
        do pos = globalPos + Utils.RandomVector3(-Config.spawnSpread, Config.spawnSpread, Config.spawnHeight);
        while (!CanSpawnHere(pos));

        return pos;
    }

    private bool CanSpawnHere(Vector3 currentCoords)
    {
        foreach (var chaser in _chasersList)
        {
            var distance = (chaser.transform.position - currentCoords).magnitude;
            if (distance <= chaser.radius * 2f)
            {
                return false;
            }
        }
        foreach (var runner in _runnersList)
        {
            var distance = (runner.transform.position - currentCoords).magnitude;
            if (distance <= runner.radius * 2f)
            {
                return false;
            }
        }
        return true;
    }
}
