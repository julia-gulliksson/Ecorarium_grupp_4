using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Wolf : MonoBehaviour
{
    public Vector3 targetPoint;
    [SerializeField] LayerMask hitMask;
    public bool hasFoundTarget = false;
    NavMeshAgent navMeshAgent;
    float range = 4f;
    [SerializeField] public int id;
    float rotationSpeed = 5f;
    Vector3 hitPoint;
    bool foundHitPoint;
    float heightOffset = 0.5f;
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
        Vector3 startPoint = new Vector3(transform.position.x, transform.position.y + heightOffset, transform.position.z);
        Ray faceForward = new Ray(startPoint, forward * range);
        Ray faceRight = new Ray(startPoint, right * range);
        Ray faceLeft = new Ray(startPoint, -right * range);

        Debug.DrawRay(startPoint, forward * range, Color.red);
        Debug.DrawRay(startPoint, right * range, Color.blue);
        Debug.DrawRay(startPoint, -right * range, Color.yellow);

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

                if (Vector3.Distance(navMeshAgent.destination, transform.position) < 1f && !foundHitPoint)
                {
                    foundHitPoint = true;
                    hitPoint = -raysFoundTarget[0].hit.normal;
                }

                if (foundHitPoint)
                {
                    // Rotate towards fence
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
