using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Fence : MonoBehaviour
{
    //MeshFilter meshFilter;
    Vector3 fromPosition;
    Vector3 toPosition;
    private List<Vector3> targetPoints = new List<Vector3>();
    [SerializeField] WolfSpawn wolfSpawn;
    List<float> lerpNumbers = new List<float>();
    float lerpMiddle = 0.5f;
    MeshFilter meshFilter;

    void Start()
    {


        //MeshCollider[] fencePositions = GetComponentsInChildren<MeshCollider>();
        //Debug.Log(fencePositions.Length);
        //for (int i = 1; i < fencePositions.Length; i++)
        //{
        //    Instantiate(new GameObject("Position from parent"), fencePositions[i].bounds.center, Quaternion.identity);

        //}
        //foreach (Transform fencePosition in fencePositions)
        //{
        //    targetPoints.Add(fencePosition.position);
        //    Instantiate(new GameObject("Position from parent"), fencePosition.position, Quaternion.identity);
        //}


    }

    Bounds GetMaxBounds()
    {
        MeshFilter[] renderers = GetComponentsInChildren<MeshFilter>();
        //if (renderers.Length == 0) return new Bounds(transform.position, Vector3.zero);
        var b = renderers[0].mesh.bounds;
        foreach (MeshFilter r in renderers)
        {
            b.Encapsulate(r.mesh.bounds);
        }
        return b;
    }

    float ClosestToMiddle()
    {
        return lerpNumbers.Aggregate((min, next) => Mathf.Abs(next - lerpMiddle) > Mathf.Abs(min - lerpMiddle) ? min : next);
    }

    IEnumerator CreateTargetPoints()
    {
        while (targetPoints.Count < wolfSpawn.nrOfWolves / 4)
        {
            float lerpPoint = ClosestToMiddle();
            Vector3 targetPoint = Vector3.Lerp(fromPosition, toPosition, lerpPoint);
            targetPoints.Add(targetPoint);
            lerpNumbers.Remove(lerpPoint);
            Instantiate(new GameObject("Point " + lerpPoint), targetPoint, Quaternion.identity);

            if (targetPoints.Count == wolfSpawn.nrOfWolves / 4)
            {
                // All target points are created
                GameEventsManager.current.TargetPointsGenerated(targetPoints);
            }
            else
            {
                yield return null;
            }
        }
    }

    //TODO: Health system
}
