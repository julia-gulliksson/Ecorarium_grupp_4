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
        float lerpPoint = 0.1f;
        float lerpBy = 0.8f / (wolfSpawn.nrOfWolves / 4);
        while (targetPoints.Count < wolfSpawn.nrOfWolves / 4)
        {
            Vector3 targetPoint = Vector3.Lerp(fromPosition, toPosition, lerpPoint);
            lerpPoint += lerpBy;
            targetPoints.Add(targetPoint);
            Instantiate(new GameObject("Point " + lerpPoint), targetPoint, Quaternion.identity);

            if (targetPoints.Count == wolfSpawn.nrOfWolves / 4)
            {
                Debug.Log(targetPoints.Count + " All created");
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
