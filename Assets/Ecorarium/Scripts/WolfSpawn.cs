using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfSpawn : MonoBehaviour
{
    [SerializeField] GameObject wolf;
    private List<GameObject> wolves = new List<GameObject>();
    [SerializeField] Transform sheepEnclosure;
    private List<Vector3> targetPoints = new List<Vector3>();
    [SerializeField] int nrOfWolves = 10;
    Vector3 fromPosition;
    Vector3 toPosition;
    MeshFilter enclosureMeshFilter;

    void Start()
    {

        enclosureMeshFilter = sheepEnclosure.GetComponent<MeshFilter>();
        fromPosition = sheepEnclosure.TransformPoint(enclosureMeshFilter.mesh.bounds.max);
        toPosition = sheepEnclosure.TransformPoint(enclosureMeshFilter.mesh.bounds.min);
        StartCoroutine(CreateTargetPoints());
    }

    IEnumerator CreateTargetPoints()
    {
        float lerpPoint = 0.0f;
        while (targetPoints.Count < nrOfWolves)
        {
            Vector3 targetPoint = Vector3.Lerp(fromPosition, toPosition, lerpPoint);

            targetPoints.Add(targetPoint);

            lerpPoint += 0.1f;

            if (targetPoints.Count == nrOfWolves)
            {
                // All target point spots are created, begin spawning wolves
                yield return StartCoroutine(SpawnWolves());
            }
            else
            {
                yield return null;
            }
        }
    }

    IEnumerator SpawnWolves()
    {
        while (wolves.Count < nrOfWolves)
        {
            float x = UnityEngine.Random.Range(-10, 10);
            float y = 0.5f;
            float z = UnityEngine.Random.Range(-40, -30);
            Vector3 positioning = new Vector3(x, y, z);

            bool tooClose = false;

            foreach (GameObject wolf in wolves)
            {
                if (Vector3.Distance(wolf.transform.position, positioning) < 0.4f)
                {
                    tooClose = true;
                }
            }

            if (!tooClose)
            {
                try
                {
                    GameObject wolfObj = Instantiate(wolf, positioning, Quaternion.identity);
                    Wolf wolfScript = wolfObj.GetComponent<Wolf>();

                    // Assign a targetPoint to the wolf
                    wolfScript.targetPoint = targetPoints[0];
                    // Remove targetPoint in list, since no other wolves should get this targetPoint
                    targetPoints.Remove(targetPoints[0]);
                    wolves.Add(wolfObj);

                }
                catch (Exception e)
                {
                    Debug.LogWarning(e + " Could not create wolf");
                }
            }
            yield return null;
        }
    }
}
