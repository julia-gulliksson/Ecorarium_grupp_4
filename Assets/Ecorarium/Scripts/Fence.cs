using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Fence : MonoBehaviour
{
    MeshFilter meshFilter;
    Vector3 fromPosition;
    Vector3 toPosition;
    private List<Vector3> targetPoints = new List<Vector3>();
    [SerializeField] WolfSpawn wolfSpawn;
    List<float> lerpNumbers = new List<float>();

    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        fromPosition = transform.TransformPoint(meshFilter.mesh.bounds.max);
        toPosition = transform.TransformPoint(meshFilter.mesh.bounds.min);
        float lerpPoint = 0.1f;
        float lerpBy = 0.8f / (wolfSpawn.nrOfWolves + 2);
        Debug.Log(lerpBy + " LERP BY");
        while (lerpNumbers.Count < (wolfSpawn.nrOfWolves + 2))
        {
            lerpNumbers.Add(lerpPoint);
            lerpPoint += lerpBy;
        }
        var nearest = lerpNumbers.Aggregate((current, next) => Mathf.Abs((long)current - 0.5f) < Mathf.Abs((long)next - 0.5f) ? current : next);
        lerpNumbers.ForEach(l => Debug.Log(l));
        //float closest = ClosestToMiddle();
        Debug.Log(nearest + " Nearest");
        //StartCoroutine(CreateTargetPoints());
    }

    IEnumerator CreateTargetPoints()
    {
        float lerpPoint = 0.1f;
        float lerpBy = 0.8f / wolfSpawn.nrOfWolves;
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

    float ClosestToMiddle()
    {
        float closest = 0.5f;
        foreach (float lerpNumber in lerpNumbers)
        {
            if (lerpNumber > 0 && lerpNumber <= Mathf.Abs(closest))
            {
                closest = lerpNumber;
            }
            else if (lerpNumber < 0 && -lerpNumber < Mathf.Abs(closest))
            {
                closest = lerpNumber;
            }
        }
        return closest;
    }

    //TODO: Health system
}
