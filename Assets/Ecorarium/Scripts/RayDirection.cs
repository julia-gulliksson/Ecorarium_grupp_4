using UnityEngine;
public class RayDirection
{
    private Ray ray;
    public Direction direction;
    public RaycastHit hit;
    float range = 4;
    Vector3 targetPoint;
    float hitDistance = 2;
    LayerMask hitMask;

    public RayDirection(Ray rayCast, Direction directionToFace, Vector3 target, LayerMask mask)
    {
        ray = rayCast;
        direction = directionToFace;
        targetPoint = target;
        hitMask = mask;
    }

    public void DrawDebugRay()
    {
        Debug.DrawRay(ray.origin, ray.direction, Color.red);
    }

    public bool CastRay()
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
    Left,
    Right,
    Forward
}