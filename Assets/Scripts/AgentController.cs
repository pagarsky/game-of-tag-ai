using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.EventSystems;

public class AgentController : MonoBehaviour
{
    private CapsuleCollider _collider;
    private Rigidbody _rb;
    private Material _material;

    void Awake()
    {
        _collider = GetComponent<CapsuleCollider>();
        _material = GetComponent<Material>();
        _rb = GetComponent<Rigidbody>();
        Debug.Log("Agent Spawned");
    }

    public void Initialize(Material mat)
    {
        _material = mat;
        GetComponent<Renderer>().material = _material;
    }

    void Start()
    {
    }

    void FixedUpdate()
    {
        var hits = _CastRays();
        

        DrawRays(hits);
    }

    private List<RaycastHit> _CastRays()
    {
        int raysAmount = Config.agentRaysAmount;

        float angle = 0f;
        float step = Mathf.Deg2Rad * (360 / raysAmount);

        List<RaycastHit> hits = new List<RaycastHit>();
        for (int i = 0; i < raysAmount; i++)
        {
            Vector3 direction = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle));

            RaycastHit hit;
            Physics.Raycast(transform.position, direction, out hit, Config.rayMaxDistance);

            hits.Add(hit);
            angle += step;
        }

        return hits;
    }

    public void DrawRays(List<RaycastHit> hits)
    {
        foreach (var hit in hits)
        {
            Debug.DrawLine(transform.position, hit.point, Config.rayColor);
        }
    }
}
