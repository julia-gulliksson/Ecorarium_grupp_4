using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfSpawn : MonoBehaviour
{
    [SerializeField] GameObject wolf;
    List<GameObject> wolves = new List<GameObject>();
    [SerializeField] public int nrOfWolves = 10;
    List<Vector3> targetPoints = new List<Vector3>();
    [SerializeField] float spawnRadius = 6;
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
        while (wolves.Count < nrOfWolves && targetPoints.Count > 0)
        {
            Vector2 radius = UnityEngine.Random.insideUnitCircle * spawnRadius;
            Vector3 positioning = new Vector3(transform.position.x + radius.x, transform.position.y, transform.position.z + radius.y);
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
                WolfStateManager wolfScript = wolfObj.GetComponent<WolfStateManager>();
                wolfScript.targetPoint = targetPoint;
                wolfScript.id = wolves.Count;
                wolves.Add(wolfObj);
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