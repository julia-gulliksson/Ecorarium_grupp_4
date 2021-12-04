using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfSpawn : MonoBehaviour
{
    [SerializeField] GameObject wolf;
    List<GameObject> wolves = new List<GameObject>();
    [SerializeField] public int nrOfWolves = 10;
    float distance = 0.4f;
    [SerializeField] List<GameObject> fenceSides;
    List<Vector3> targetPoints = new List<Vector3>();
    float spawnRadius = 10;
    GameObject[] destinationObjects;
    public AudioSource source;

    private void Start()
    {
        StartCoroutine(DetermineFencePositions());
        GameEventsManager.current.OnNight += NightBegin;
    }

    IEnumerator DetermineFencePositions()
    {
        destinationObjects = GameObject.FindGameObjectsWithTag("Destination");
        foreach (GameObject destination in destinationObjects)
        {
            targetPoints.Add(destination.transform.position);
        }
        yield return StartCoroutine(SpawnWolves());
    }

    IEnumerator SpawnWolves()
    {
        while (wolves.Count < destinationObjects.Length)
        {
            Vector2 radius = UnityEngine.Random.insideUnitCircle * spawnRadius;
            Vector3 positioning = new Vector3(transform.position.x + radius.x, transform.position.y, transform.position.z + radius.y);

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
                    //targetPoint = new Vector3(1.99259f, 2.097882f, 6.81123f);
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

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }

    void NightBegin()
    {
        source.Play();
    }
}