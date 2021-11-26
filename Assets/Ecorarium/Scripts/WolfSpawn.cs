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
    float centerX;
    float plusX;
    float minusX;

    void Start()
    {
        centerX = sheepEnclosure.localScale.x / 2;
        plusX = sheepEnclosure.position.x + centerX;
        minusX = sheepEnclosure.position.x - centerX;
        StartCoroutine(CreateTargetPoints());
    }

    IEnumerator CreateTargetPoints()
    {
        while (targetPoints.Count < 10)
        {
            float x = UnityEngine.Random.Range(plusX, minusX);
            Vector3 targetPoint = new Vector3(x, sheepEnclosure.position.y, sheepEnclosure.position.z);
            bool tooClose = false;
            foreach (Vector3 point in targetPoints)
            {
                if (Vector3.Distance(point, targetPoint) < 2f)
                {
                    tooClose = true;
                }
            }
            if (!tooClose) targetPoints.Add(targetPoint);


            if (targetPoints.Count == 10)
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
        while (wolves.Count < 10)
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
