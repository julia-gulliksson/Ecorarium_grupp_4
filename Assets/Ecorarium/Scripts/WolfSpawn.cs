using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfSpawn : MonoBehaviour
{
    [SerializeField] GameObject wolf;
    List<GameObject> wolves = new List<GameObject>();
    List<Vector3> allTargetPoints = new List<Vector3>();
    [SerializeField] public int nrOfWolves = 10;
    int nrOfTargetPointsGenerated;
    [SerializeField] Transform startPoint;
    [SerializeField] Transform endPoint;
    float distance = 0.4f;
    [SerializeField] List<GameObject> fenceSides;
    List<Vector3> targetPoints = new List<Vector3>();

    //private void OnEnable()
    //{
    //    GameEventsManager.current.onTargetPointsGenerated += StartSpawningWolves;
    //}

    //private void OnDisable()
    //{
    //    GameEventsManager.current.onTargetPointsGenerated -= StartSpawningWolves;
    //}

    private void Start()
    {
        StartCoroutine(DetermineFencePositions());
    }

    IEnumerator DetermineFencePositions()
    {
        foreach (GameObject destination in GameObject.FindGameObjectsWithTag("Destination"))
        {
            targetPoints.Add(destination.transform.position);

        }
        //{
        //    MeshCollider[] fencePositions = fenceSide.GetComponentsInChildren<MeshCollider>();
        //    for (int i = 0; i < fencePositions.Length; i++)
        //    {
        //        targetPoints.Add(fencePositions[i].bounds.center);
        //        Instantiate(new GameObject("position"), fencePositions[i].bounds.center, Quaternion.identity);
        //    }

        //}
        yield return StartCoroutine(SpawnWolves());
        //yield return null;
    }

    //void StartSpawningWolves(List<Vector3> targetPoints)
    //{
    //    nrOfTargetPointsGenerated++;
    //    allTargetPoints.AddRange(targetPoints);
    //    Debug.Log(allTargetPoints.Count + " All target points");
    //    if (nrOfTargetPointsGenerated == 4)
    //    {
    //        // All fences have generated their target points, spawn wolves
    //        StartCoroutine(SpawnWolves());
    //    }
    //}

    IEnumerator SpawnWolves()
    {

        while (wolves.Count < 16)
        {
            float x = UnityEngine.Random.Range(endPoint.position.x, startPoint.position.x);
            float y = UnityEngine.Random.Range(endPoint.position.y, startPoint.position.y);
            float z = UnityEngine.Random.Range(endPoint.position.z, startPoint.position.z);
            Vector3 positioning = new Vector3(x, y, z);

            bool tooClose = false;

            foreach (GameObject wolf in wolves)
            {
                if (Vector3.Distance(wolf.transform.position, positioning) < distance)
                {
                    tooClose = true;
                }
            }

            if (!tooClose)
            {
                Vector3 targetPoint = Vector3.zero;
                try
                {
                    int randomIndex = UnityEngine.Random.Range(0, targetPoints.Count);
                    targetPoint = targetPoints[randomIndex];
                    // Remove targetPoint in list, since no other wolves should get this targetPoint
                    targetPoints.Remove(targetPoints[randomIndex]);
                }
                catch
                {
                    Debug.LogWarning("Target point index not found");
                }

                if (targetPoint != Vector3.zero)
                {
                    GameObject wolfObj = Instantiate(wolf, positioning, Quaternion.identity);
                    Wolf wolfScript = wolfObj.GetComponent<Wolf>();
                    wolfScript.targetPoint = targetPoint;
                    wolfScript.id = wolves.Count;
                    wolves.Add(wolfObj);
                }
            }
            yield return null;
        }
    }
}
