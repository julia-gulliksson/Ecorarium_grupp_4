using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Wolf : MonoBehaviour
{
    [SerializeField] float speed = 6f;
    public Vector3 targetPoint;
    public int id;
    //[SerializeField] Transform target;
    RaycastHit hit;
    [SerializeField] float range = 4;
    [SerializeField] LayerMask hitMask;
    bool moving;
    bool hasCollided = false;
    NavMeshAgent navMeshAgent;

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
        Debug.DrawRay(transform.position, forward * range, Color.red);

        if (Physics.Raycast(transform.position, forward, out hit, range, hitMask))
        {
            moving = false;
            //TODO: Check if wolf is close enough, call event then

            if (hasCollided == false) GameEventsManager.current.WolfFoundTarget(true, id);
            hasCollided = true;

        }
        else
        {
            moving = true;
            if (hasCollided == true) GameEventsManager.current.WolfFoundTarget(false, id);
            hasCollided = false;
        }
    }

    private void OnDestroy()
    {
        if (hasCollided == true) GameEventsManager.current.WolfFoundTarget(false, id);
    }

    private void Move()
    {

        if (moving)
        {
            navMeshAgent.destination = targetPoint;

            //float step = speed * Time.deltaTime;
            //Vector3 targetPostition = new Vector3(targetPoint.x, transform.position.y, targetPoint.z);
            //transform.LookAt(targetPostition);
            //transform.position = Vector3.MoveTowards(transform.position, targetPostition, step);
        }
        
    }
}
