using UnityEngine;

public class RayDirection
{
    private Ray ray;
    public RaycastHit hit;
    float range;
    Vector3 targetPoint;
    float hitDistance = 4;
    LayerMask hitMask;
    public Direction direction;

    public RayDirection(Ray rayCast, Vector3 target, LayerMask mask, float rayRange, Direction facingDirection)
    {
        ray = rayCast;
        targetPoint = target;
        hitMask = mask;
        range = rayRange;
        direction = facingDirection;
    }

    public bool TargetFound()
    {
        if (Physics.Raycast(ray, out hit, range, hitMask))
        {
            return Vector3.Distance(hit.point, targetPoint) < hitDistance;
        }
        return false;
    }
}

public enum Direction
{
    Forward,
    Left,
    Right
}