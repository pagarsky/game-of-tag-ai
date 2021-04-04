using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class Scenario : MonoBehaviour
{

    public int ChasersAmount { get; private set; }
    public int RunnersAmount { get; private set; }

    private List<AgentController> _chasers = new List<AgentController>();
    private List<AgentController> _runners = new List<AgentController>();
    private GameObject _agentPrefab;

    void Awake()
    {
        
    }

    public void Setup(GameObject agentPrefab, int chasers, int runners)
    {
        _agentPrefab = agentPrefab;
        ChasersAmount = chasers;
        RunnersAmount = runners;
    }

    public void Run()
    {
        SpawnAgents();
    }

    private void SpawnAgents()
    {
        for (int i = 0; i < RunnersAmount; i++)
        {
            GameObject runner = GameObject.Instantiate(_agentPrefab);
            AgentController controller = runner.GetComponent<AgentController>();

            _runners.Add(controller);
        }
        for (int i = 0; i < ChasersAmount; i++)
        {
            GameObject chaser = GameObject.Instantiate(_agentPrefab);
            AgentController controller = chaser.GetComponent<AgentController>();

            _chasers.Add(controller);
        }
    }

    void Start()
    {
        
    }

    void FixedUpdate()
    {
        
    }

}
