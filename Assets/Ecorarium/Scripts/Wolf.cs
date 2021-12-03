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
    [SerializeField] float heightOffset = 1f;
    [SerializeField] float heightOffsetLower = 0.2f;
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
        Vector3 startPointLower = new Vector3(transform.position.x, transform.position.y + heightOffsetLower, transform.position.z);
        Ray faceForward = new Ray(startPoint, forward * range);
        Ray faceForwardLower = new Ray(startPointLower, forward * range);
        Ray faceRight = new Ray(startPoint, right * range);
        Ray faceRightLower = new Ray(startPointLower, right * range);
        Ray faceLeft = new Ray(startPoint, -right * range);
        Ray faceLeftLower = new Ray(startPointLower, -right * range);

        Debug.DrawRay(startPoint, forward * range, Color.red);
        Debug.DrawRay(startPoint, right * range, Color.blue);
        Debug.DrawRay(startPoint, -right * range, Color.yellow);
        Debug.DrawRay(startPointLower, forward * range, Color.red);
        Debug.DrawRay(startPointLower, right * range, Color.blue);
        Debug.DrawRay(startPointLower, -right * range, Color.yellow);

        // Create list of rays facing different directions
        List<RayDirection> rayDirections = new List<RayDirection>() { new RayDirection(faceForward, targetPoint, hitMask, range, Direction.Forward), new RayDirection(faceForwardLower, targetPoint, hitMask, range, Direction.Forward),
        new RayDirection(faceRight, targetPoint, hitMask, range, Direction.Right), new RayDirection(faceRightLower, targetPoint, hitMask, range, Direction.Right),
        new RayDirection(faceLeftLower, targetPoint, hitMask, range, Direction.Left), new RayDirection(faceLeft, targetPoint, hitMask, range, Direction.Left)};

        List<RayDirection> raysFoundTarget = new List<RayDirection>();
        foreach (RayDirection ray in rayDirections)
        {
            if (ray.TargetFound())
            {
                raysFoundTarget.Add(ray);
            }
        }
        if (raysFoundTarget.Count > 0 && raysFoundTarget[0].TargetFound())
        {
            HandleDetectedFence(raysFoundTarget);
        }
    }

    void HandleDetectedFence(List<RayDirection> rays)
    {
        RayDirection firstRay = rays[0];

        if (Vector3.Distance(navMeshAgent.destination, transform.position) < 1f && !hasFoundTarget)
        {
            GameEventsManager.current.WolfFoundTarget();
            hasFoundTarget = true;
            hitPoint = -firstRay.hit.normal;
        }

        if (hasFoundTarget)
        {
            Quaternion look = Quaternion.LookRotation(hitPoint);
            if (transform.rotation == look && rays.Count == 1 && firstRay.direction != Direction.Forward)
            {
                // Account for wolves at end of fence facing the wrong way
                float yRotation = -90f;
                if (firstRay.direction == Direction.Left) yRotation = 90.0f;
                Vector3 newTarget = new Vector3(0.0f, yRotation, 0.0f);
                Quaternion newLook = Quaternion.LookRotation(newTarget);
                transform.rotation = Quaternion.Slerp(transform.rotation, newLook, rotationSpeed * Time.deltaTime);
            }
            else
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, look, rotationSpeed * Time.deltaTime);
            }
        }
    }

    void Move()
    {
        navMeshAgent.destination = targetPoint;
    }
}
