using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfAttackFenceState : WolfBaseState
{
    Vector3 hitPoint;
    WolfStateManager wolf;
    float heightOffset = 1f;
    float heightOffsetLower = 0.2f;
    float range = 4f;
    bool hasFoundTarget = false;
    float rotationSpeed = 5f;

    public override void EnterState(WolfStateManager wolfRef)
    {
        wolfRef.navMeshAgent.SetDestination(wolfRef.targetPoint);
        wolf = wolfRef;
    }

    public override void UpdateState()
    {
        DetectFence();
    }

    void DetectFence()
    {
        Transform transform = wolf.transform;
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
        List<RayDirection> rayDirections = new List<RayDirection>() { new RayDirection(faceForward, wolf.targetPoint, wolf.hitMask, range, Direction.Forward), new RayDirection(faceForwardLower, wolf.targetPoint, wolf.hitMask, range, Direction.Forward),
        new RayDirection(faceRight, wolf.targetPoint, wolf.hitMask, range, Direction.Right), new RayDirection(faceRightLower, wolf.targetPoint, wolf.hitMask, range, Direction.Right),
        new RayDirection(faceLeftLower, wolf.targetPoint, wolf.hitMask, range, Direction.Left), new RayDirection(faceLeft, wolf.targetPoint, wolf.hitMask, range, Direction.Left)};

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

        if (Vector3.Distance(wolf.navMeshAgent.destination, wolf.transform.position) < 1f && !hasFoundTarget)
        {
            IFence fence = firstRay.hit.collider.GetComponent<IFence>();
            if (fence != null)
            {
                fence.WolfHit();
                wolf.fenceScript = fence;
            }

            GameEventsManager.current.WolfAttacking(wolf.id);

            hasFoundTarget = true;
            hitPoint = -firstRay.hit.normal;
        }

        if (hasFoundTarget)
        {
            Quaternion look = Quaternion.LookRotation(hitPoint);
            if (wolf.transform.rotation == look && rays.Count == 1 && firstRay.direction != Direction.Forward)
            {
                // Account for wolves at end of fence facing the wrong way
                float yRotation = -90f;
                if (firstRay.direction == Direction.Left) yRotation = 90.0f;
                Vector3 newTarget = new Vector3(0.0f, yRotation, 0.0f);
                Quaternion newLook = Quaternion.LookRotation(newTarget);
                wolf.transform.rotation = Quaternion.Slerp(wolf.transform.rotation, newLook, rotationSpeed * Time.deltaTime);
            }
            else
            {
                wolf.transform.rotation = Quaternion.Slerp(wolf.transform.rotation, look, rotationSpeed * Time.deltaTime);
            }
        }
    }

    public void HandleFenceBreak()
    {
        GameEventsManager.current.WolfStopAttacking(wolf.id);
        wolf.fenceScript?.WolfLost();
        wolf.SwitchState(wolf.AttackSheepState);
    }

    public override void OnDestroy()
    {
        wolf.fenceScript?.WolfLost();
    }
}
