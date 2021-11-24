using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wolf : MonoBehaviour
{
    [SerializeField] float speed = 6f;
    public Vector3 targetPoint;
    [SerializeField] Transform target;
    RaycastHit hit;
    [SerializeField] float range = 2;
    [SerializeField] LayerMask hitMask;
    bool moving;
    Vector3 originalUp;

    private void Start()
    {
        originalUp = transform.up;

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
            //Debug.Log(hit.transform.name);
            moving = false;
        }
        else
        {
            moving = true;
        }
    }

    private void Move()
    {

        if (moving)
        {
            float step = speed * Time.deltaTime;
            Vector3 targetPostition = new Vector3(target.position.x, transform.position.y, target.position.z);
            transform.LookAt(targetPostition);
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.position.x, transform.position.y, target.position.z), step);
        }
    }
}
