using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Wolf : MonoBehaviour
{
    public Vector3 targetPoint;
    [SerializeField] LayerMask hitMask;
    bool hasFoundTarget = false;
    NavMeshAgent navMeshAgent;
    float range = 4;
    public int id;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        DetectFence();
        Move();
    }


    void DetectFence()
    {
        Vector3 forward = transform.forward;
        Ray faceForward = new Ray(transform.position, forward * range);

        Debug.DrawRay(transform.position, forward * range, Color.red);

        RayDirection fenceRay = new RayDirection(faceForward, targetPoint, hitMask, range);

        if (fenceRay.TargetFound())
        {
            Quaternion look = Quaternion.LookRotation(-fenceRay.hit.normal);
            transform.rotation = Quaternion.Slerp(transform.rotation, look, 5f * Time.deltaTime);
            hasFoundTarget = true;
        }

    }

    private void OnDestroy()
    {
        if (hasFoundTarget == true) GameEventsManager.current.WolfLostTarget();
    }

    private void Move()
    {
        navMeshAgent.destination = targetPoint;
    }
}

