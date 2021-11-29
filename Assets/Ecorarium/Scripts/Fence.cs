using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fence : MonoBehaviour
{
    MeshFilter meshFilter;
    Vector3 fromPosition;
    Vector3 toPosition;
    private List<Vector3> targetPoints = new List<Vector3>();
    [SerializeField] WolfSpawn wolfSpawn;

    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        fromPosition = transform.TransformPoint(meshFilter.mesh.bounds.max);
        toPosition = transform.TransformPoint(meshFilter.mesh.bounds.min);
        StartCoroutine(CreateTargetPoints());
    }

    IEnumerator CreateTargetPoints()
    {
        //TODO: Make dynamic according to number of wolves
        float lerpPoint = 0.5f;
        while (targetPoints.Count <= wolfSpawn.nrOfWolves / 4)
        {

            Vector3 targetPoint = Vector3.Lerp(fromPosition, toPosition, lerpPoint);

            //lerpPoint += 0.1f;
            targetPoints.Add(targetPoint);

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
