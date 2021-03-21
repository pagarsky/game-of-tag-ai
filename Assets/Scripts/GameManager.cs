using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // summary: Main Class for handling game process

    public GameObject agentPrefab;

    void Awake()
    {
        
    }

    void Start()
    {
        GameObject test = GameObject.Instantiate(agentPrefab);
    }

    void Update()
    {
        
    }
}
