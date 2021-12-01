using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Wolf : MonoBehaviour
{
    public Vector3 targetPoint;
    [SerializeField] LayerMask hitMask;
    bool hasFoundTarget = false;
    NavMeshAgent navMeshAgent;
    float range = 4f;
    [SerializeField] public int id;
    float rotationSpeed = 5f;
    Vector3 hitPoint;
    bool foundHitPoint;
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        DetectFence();
        Move();
    }

    void OnDestroy()
    {
        if (hasFoundTarget == true) GameEventsManager.current.WolfLostTarget();
    }

    void DetectFence()
    {
        Vector3 forward = transform.forward;
        Vector3 right = transform.right;
        Ray faceForward = new Ray(transform.position, forward * range);
        Ray faceRight = new Ray(transform.position, right * range);
        Ray faceLeft = new Ray(transform.position, -right * range);

        Debug.DrawRay(transform.position, forward * range, Color.red);
        Debug.DrawRay(transform.position, right * range, Color.blue);
        Debug.DrawRay(transform.position, -right * range, Color.yellow);

        // Create list of rays facing different directions
        List<RayDirection> rayDirections = new List<RayDirection>() { new RayDirection(faceForward, targetPoint, hitMask, range),
        new RayDirection(faceRight, targetPoint, hitMask, range), new RayDirection(faceLeft, targetPoint, hitMask, range)};

        List<RayDirection> raysFoundTarget = new List<RayDirection>();
        foreach (RayDirection ray in rayDirections)
        {
            if (ray.TargetFound())
            {
                raysFoundTarget.Add(ray);
            }
        }
        if (raysFoundTarget.Count > 0)
        {
            if (raysFoundTarget[0].TargetFound())
            {
                if (hasFoundTarget == false) GameEventsManager.current.WolfFoundTarget();
                hasFoundTarget = true;
                //Debug.Log("here " + id);

                if (Vector3.Distance(navMeshAgent.destination, transform.position) < 1f && !foundHitPoint)
                {
                    //Debug.Log("In here " + id);
                    foundHitPoint = true;
                    hitPoint = -raysFoundTarget[0].hit.normal;
                    Instantiate(new GameObject("Hit point" + id), hitPoint, Quaternion.identity);
                }

                if (foundHitPoint)
                {
                    // Rotate towards fence
                    Debug.Log("Rotating");
                    Quaternion look = Quaternion.LookRotation(hitPoint);
                    transform.rotation = Quaternion.Slerp(transform.rotation, look, rotationSpeed * Time.deltaTime);
                }
            }
        }

    }

    void Move()
    {
        navMeshAgent.destination = targetPoint;
    }
}

