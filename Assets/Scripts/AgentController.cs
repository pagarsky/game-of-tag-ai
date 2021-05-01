using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;


public class AgentController : Agent
{
    private CapsuleCollider _collider;
    private Rigidbody _rb;
    private Material _material;
    private Utils.ListAgentDelegate _GetEnemies;
    
    public Config.Teams team { get; private set; }

    void Awake()
    {
        _collider = GetComponent<CapsuleCollider>();
        _material = GetComponent<Material>();
        _rb = GetComponent<Rigidbody>();

        Debug.Log("Agent Spawned");
    }

    public override void OnEpisodeBegin()
    {
        transform.localPosition += Utils.RandomVector3(0, 5f);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        var directions = _GenerateCastDirections();
        var hits = _CastObstacleRays(directions);
        var enemyAngles = _CastEnemiesRays(directions);
        
        var obstacleDistances = hits.Select(h => h.distance).ToList();



        // Agent local position
        sensor.AddObservation(transform.localPosition);

        // N raycast distances
        sensor.AddObservation(obstacleDistances);

        // enemies angles
        foreach (var ang in enemyAngles)
        {
            sensor.AddObservation(ang);
        }
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {

    }

    private float _ClosestEnemyDist(List<AgentController> enemies)
    {
        float min = Mathf.Infinity;
        foreach (var e in enemies)
        {
            float distance = (e.transform.position - transform.position).magnitude;
            min = (distance < min) ? distance : min;
        }
        return min;
    }

    public void Initialize(Material mat, Utils.ListAgentDelegate GetEnemies, Config.Teams team)
    {
        this.team = team;
        _material = mat;
        _GetEnemies = GetEnemies;
        
        gameObject.name = (team == Config.Teams.CHASERS) ? "chaser" : "runner";
        GetComponent<Renderer>().material = _material;
    }

    void FixedUpdate()
    {
        var directions = _GenerateCastDirections();
        var hits = _CastObstacleRays(directions);
        var angles = _CastEnemiesRays(directions);

        DrawRays(hits);
        foreach (var a in angles)
        {
            Debug.Log(String.Join(" ", a));
        }
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

    private List<float[]> _CastEnemiesRays(List<Vector3> directions)
    {
        List<AgentController> enemies = _GetEnemies?.Invoke();
        var enemiesAngles = new List<float[]>();
        
        foreach (var enemy in enemies)
        {
            int i = 0;
            Vector3 cast = (transform.position - enemy.transform.position).normalized;

            if (!Config.FULL_OBSERVABILITY && !_CanSee(enemy, -cast))
            {
                enemiesAngles.Add(Enumerable.Repeat(0f, Config.agentRaysAmount).ToArray());
                continue;
            }

            float[] angles = Enumerable.Repeat(Mathf.Infinity, Config.agentRaysAmount).ToArray();
            foreach (var ray in directions)
            {
                Vector3 lhs = enemy.transform.position + Vector3.Cross(cast, Vector3.up).normalized * enemy._collider.radius;
                Vector3 rhs = enemy.transform.position - Vector3.Cross(cast, Vector3.up).normalized * enemy._collider.radius;
                int currCellIdx = i;
                int nextCellIdx = (i + 1) % Config.agentRaysAmount;

                var lhsAngle = Vector3.SignedAngle(lhs - transform.position, ray, Vector3.up);
                var rhsAngle = Vector3.SignedAngle(rhs - transform.position, ray, Vector3.up);

                var tuple = _DecideAngles(lhsAngle, rhsAngle);
                angles[currCellIdx] = _MinByAbs(tuple.Item1, angles[currCellIdx]);
                angles[nextCellIdx] = _MinByAbs(tuple.Item2, angles[nextCellIdx]);
                i++;
            }
            enemiesAngles.Add(angles);
        }
        return enemiesAngles;
    }

    private Tuple<float, float> _DecideAngles(float lhsAngle, float rhsAngle)
    {
        Tuple<float, float> angles;
        if (lhsAngle * rhsAngle > 0)
        {
            if (lhsAngle < 0)
            {
                angles = new Tuple<float, float>(Math.Max(lhsAngle, rhsAngle), 0);
            }
            else
            {
                angles = new Tuple<float, float>(0, Math.Min(lhsAngle, rhsAngle));
            }
        }
        else
        {
            angles = new Tuple<float, float>(Math.Min(lhsAngle, rhsAngle), Math.Max(lhsAngle, rhsAngle));
        }
        return angles;
    }

    private float _MaxByAbs(float item1, float item2)
    {
        return Math.Abs(item1) > Math.Abs(item2) ? item1 : item2;
    }

    private float _MinByAbs(float item1, float item2, bool notZero = false)
    {
        if (notZero) return _NotZeroMinByAbs(item1, item2);
        return Math.Abs(item1) < Math.Abs(item2) ? item1 : item2;
    }

    private float _NotZeroMinByAbs(float item1, float item2)
    {
        return Math.Abs(item1) < Math.Abs(item2) ?
            item1 == 0f ? item2 : item1
            :
            item2 == 0f ? item1 : item2;
    }

    private bool _CanSee(AgentController enemy, Vector3 cast)
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, cast, out hit);

        return GameObject.Equals(hit.collider.gameObject, enemy.gameObject);
    }

    public void DrawRays(List<RaycastHit> hits)
    {
        foreach (var hit in hits)
        {
            Debug.DrawLine(transform.position, hit.point, Config.rayColor);
        }
    }
}
