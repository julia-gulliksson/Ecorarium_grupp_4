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
    float lerpMiddle = 0.5f;

    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        fromPosition = transform.TransformPoint(meshFilter.mesh.bounds.max);
        toPosition = transform.TransformPoint(meshFilter.mesh.bounds.min);
        float lerpPoint = 0.1f;
        //TODO: Twerk these numbers
        float lerpBy = 0.8f / (wolfSpawn.nrOfWolves);
        while (lerpNumbers.Count < (wolfSpawn.nrOfWolves))
        {
            lerpNumbers.Add(lerpPoint);
            lerpPoint += lerpBy;
        }


        lerpNumbers.ForEach(l => Debug.Log(l));

        //float closest = ClosestToMiddle();
        //Debug.Log(nearest + " Nearest");
        StartCoroutine(CreateTargetPoints());
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
