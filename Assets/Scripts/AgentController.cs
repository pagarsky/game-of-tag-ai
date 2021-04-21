using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AgentController : MonoBehaviour
{
    private CapsuleCollider _collider;
    private Rigidbody _rb;
    private Material _material;

    private Utils.ListAgentDelegate _GetEnemies;

    void Awake()
    {
        _collider = GetComponent<CapsuleCollider>();
        _material = GetComponent<Material>();
        _rb = GetComponent<Rigidbody>();

        Debug.Log("Agent Spawned");
    }

    public void Initialize(Material mat, Utils.ListAgentDelegate GetEnemies)
    {
        _material = mat;
        GetComponent<Renderer>().material = _material;
        
        _GetEnemies = GetEnemies;
    }

    void FixedUpdate()
    {
        var directions = _GenerateCastDirections();
        var hits = _CastObstacleRays(directions);
        var angles = _CastEnemiesRays(directions);

        DrawRays(hits);
    }

    private List<Vector3> _GenerateCastDirections()
    {
        int raysAmount = Config.agentRaysAmount;
        float angle = 0f;
        float step = Mathf.Deg2Rad * (360 / raysAmount);
        List<Vector3> dirs = new List<Vector3>();

        for (int i = 0; i < raysAmount; i++)
        {
            dirs.Add(new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)));
            angle += step;
        }
        return dirs;
    }

    private List<RaycastHit> _CastObstacleRays(List<Vector3> directions)
    {
        List<RaycastHit> hits = new List<RaycastHit>();
        foreach (var dir in directions)
        {
            RaycastHit hit;
            Physics.Raycast(transform.position, dir, out hit, Config.rayMaxDistance);

            hits.Add(hit);
        }

        return hits;
    }

    private List<Tuple<float, float>> _CastEnemiesRays(List<Vector3> directions)
    {
        List<AgentController> enemies = _GetEnemies?.Invoke();
        List<Tuple<float, float>> angleTuples = new List<Tuple<float, float>>();

        foreach (AgentController e in enemies)
        {
            Vector3 direction = (transform.position - e.transform.position).normalized;
            Vector3 lhs = e.transform.position + Vector3.Cross(direction, Vector3.up).normalized * e._collider.radius;
            Vector3 rhs = e.transform.position - Vector3.Cross(direction, Vector3.up).normalized * e._collider.radius;

            float closestLhs = Mathf.Infinity;
            float closestRhs = Mathf.Infinity;
            foreach (var cast in directions)
            {
                var lhsAngle = Vector3.SignedAngle(lhs - transform.position, cast, Vector3.up);
                var rhsAngle = Vector3.SignedAngle(rhs - transform.position, cast, Vector3.up);

                closestLhs = Math.Abs(lhsAngle) < Math.Abs(closestLhs) ? lhsAngle : closestLhs;
                closestRhs = Math.Abs(rhsAngle) < Math.Abs(closestRhs) ? rhsAngle : closestRhs;
            }
            angleTuples.Add(Tuple.Create(closestLhs, closestRhs));
            //TODO: Review for potential bug

            //Debug.DrawLine(transform.position, lhs);
            //Debug.DrawLine(transform.position, rhs);
            //Debug.Log(closestLhs);
            //Debug.Log(closestRhs);
        }

        return angleTuples;
    }

    public void DrawRays(List<RaycastHit> hits)
    {
        foreach (var hit in hits)
        {
            Debug.DrawLine(transform.position, hit.point, Config.rayColor);
        }
    }
}
