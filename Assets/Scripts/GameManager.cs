using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // summary: Main Class for handling game process

    public GameObject agentPrefab;

    public int chasers;
    public int runners;

    public Material[] materials;

    void Awake()
    {
        
    }

    void Start()
    {
        GameObject test = GameObject.Instantiate(agentPrefab);
        AgentController agent = test.GetComponent<AgentController>();
    }

    void Update()
    {
        
    }
}
