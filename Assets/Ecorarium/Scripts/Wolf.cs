using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Wolf : MonoBehaviour
{
    public Vector3 targetPoint;
    RaycastHit hit;
    [SerializeField] LayerMask hitMask;
    bool moving = true;
    bool hasCollided = false;
    NavMeshAgent navMeshAgent;
    float range = 4;

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
        Vector3 right = transform.right;

        Ray faceForward = new Ray(transform.position, forward * range);
        Ray faceRight = new Ray(transform.position, right * range);
        Ray faceLeft = new Ray(transform.position, -right * range);

        List<RayDirection> rayDirections = new List<RayDirection>() { new RayDirection(faceForward, Direction.Forward, targetPoint, hitMask),
            new RayDirection(faceRight, Direction.Right, targetPoint, hitMask),
            new RayDirection(faceLeft, Direction.Left, targetPoint, hitMask) };

        List<RayDirection> detectingFences = new List<RayDirection>();

        foreach (RayDirection ray in rayDirections)
        {
            ray.DrawDebugRay();
            if (ray.CastRay())
            {
                detectingFences.Add(ray);
            }
        }

        if (detectingFences.Count == 2)
        {
            foreach (RayDirection ray in detectingFences)
            {
                if (ray.direction == Direction.Forward)
                {
                    Debug.Log("Forward");
                }
            }
            Vector3 middle = Vector3.Lerp(detectingFences[0].hit.point, detectingFences[1].hit.point, 0.5f);
            Instantiate(new GameObject("Middle"), middle, Quaternion.identity);
        }

        //if (Physics.Raycast(transform.position, forward, out hit, range))
        //{
        //    if (Vector3.Distance(hit.point, targetPoint) < 2f)
        //    {
        //        Debug.Log("Framme");
        //        moving = false;

        //        if (hasCollided == false) GameEventsManager.current.WolfFoundTarget();
        //        hasCollided = true;

        //        // TODO: Make rotation work
        //        //Vector3 direction = hit.point - transform.position;
        //        //Debug.DrawRay(transform.position, direction, Color.green);
        //        //float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        //        //if (angle != 0)
        //        //{
        //        //    Quaternion angleAxis = Quaternion.AngleAxis(0, Vector3.up);
        //        //    transform.rotation = Quaternion.Slerp(transform.rotation, angleAxis, Time.deltaTime * 10);
        //        //}
        //    }

        //}
        //else
        //{
        //    moving = true;
        //    if (hasCollided == true) GameEventsManager.current.WolfLostTarget();
        //    hasCollided = false;
        //}
    }

    private void OnDestroy()
    {
        if (hasCollided == true) GameEventsManager.current.WolfLostTarget();
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
        else
        {
            navMeshAgent.destination = transform.position;
        }
    }
}

