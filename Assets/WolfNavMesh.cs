using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WolfNavMesh : MonoBehaviour
{
    NavMeshAgent navMeshAgent;
    [SerializeField] private Transform moveTo;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        navMeshAgent.destination = moveTo.position;
    }
}
