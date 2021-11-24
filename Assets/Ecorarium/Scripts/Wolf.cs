using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wolf : MonoBehaviour
{
    [SerializeField] float speed = 6f;
    public Vector3 targetPoint;
    //[SerializeField] Transform target;
    RaycastHit hit;
    [SerializeField] float range = 2;
    [SerializeField] LayerMask hitMask;
    bool moving;
    bool hasCollided = false;

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

            if (hasCollided == false) GameEventsManager.current.WolfFoundTarget(true);
            hasCollided = true;

        }
        else
        {
            moving = true;
            if (hasCollided == true) GameEventsManager.current.WolfFoundTarget(false);
            hasCollided = false;
        }
    }

    private void OnDestroy()
    {
        if (hasCollided == true) GameEventsManager.current.WolfFoundTarget(false);
    }

    private void Move()
    {

        if (moving)
        {
            float step = speed * Time.deltaTime;
            Vector3 targetPostition = new Vector3(targetPoint.x, transform.position.y, targetPoint.z);
            transform.LookAt(targetPostition);
            transform.position = Vector3.MoveTowards(transform.position, targetPostition, step);
        }
    }
}
