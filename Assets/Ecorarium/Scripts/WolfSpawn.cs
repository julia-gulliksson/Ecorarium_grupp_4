using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfSpawn : MonoBehaviour
{
    [SerializeField] GameObject wolf;
    private List<GameObject> wolves = new List<GameObject>();
    private List<Vector3> allTargetPoints = new List<Vector3>();
    [SerializeField] public int nrOfWolves = 4;
    [SerializeField] public int nrOfTargetPointsGenerated;
    private void OnEnable()
    {
        GameEventsManager.current.onTargetPointsGenerated += StartSpawningWolves;
    }

    private void OnDisable()
    {
        GameEventsManager.current.onTargetPointsGenerated -= StartSpawningWolves;
    }

    void StartSpawningWolves(List<Vector3> targetPoints)
    {
        nrOfTargetPointsGenerated++;
        allTargetPoints.AddRange(targetPoints);
        if (nrOfTargetPointsGenerated == 4)
        {
            StartCoroutine(SpawnWolves());
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
                    wolfScript.targetPoint = allTargetPoints[0];
                    wolfScript.id = wolves.Count;
                    // Remove targetPoint in list, since no other wolves should get this targetPoint
                    allTargetPoints.Remove(allTargetPoints[0]);
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
