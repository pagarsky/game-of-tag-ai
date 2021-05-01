using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class Scenario : MonoBehaviour
{
    private List<AgentController> _chasersList = new List<AgentController>();
    private List<AgentController> _runnersList = new List<AgentController>();

    Unity.MLAgents.SimpleMultiAgentGroup _chasers = new Unity.MLAgents.SimpleMultiAgentGroup();
    Unity.MLAgents.SimpleMultiAgentGroup _runners = new Unity.MLAgents.SimpleMultiAgentGroup();

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

    public void Setup(int chasers, int runners, Vector3 globalPosition)
    {
        ChasersAmount = chasers;
        RunnersAmount = runners;

        this._stage = GameObject.Instantiate(stagePrefab, this.gameObject.transform);
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
            Vector3 position = Utils.RandomVector3(-Config.spawnSpread, Config.spawnSpread, 0, 5);
            GameObject runner = GameObject.Instantiate(agentPrefab, this.gameObject.transform);

            runner.transform.position = globalPos + position;

            AgentController controller = runner.GetComponent<AgentController>();
            controller.Initialize(materials[0], GetChasers, Config.Teams.RUNNERS);

            _runnersList.Add(controller);
            _runners.RegisterAgent(controller);
        }
        for (int i = 0; i < ChasersAmount; i++)
        {
            Vector3 position = Utils.RandomVector3(-Config.spawnSpread, Config.spawnSpread, 0, 5);
            GameObject chaser = GameObject.Instantiate(agentPrefab, this.gameObject.transform);

            chaser.transform.position = globalPos + position;

            AgentController controller = chaser.GetComponent<AgentController>();
            controller.Initialize(materials[1], GetRunners, Config.Teams.CHASERS);

            _chasersList.Add(controller);
            _chasers.RegisterAgent(controller);
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
}
