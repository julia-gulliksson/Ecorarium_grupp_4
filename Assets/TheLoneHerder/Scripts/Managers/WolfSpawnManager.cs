using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TheLoneHerder
{
    public class WolfSpawnManager : MonoBehaviour
    {
        [SerializeField] GameObject wolf;
        int nrOfWolves = 0;
        [SerializeField] float spawnRadius = 6;
        List<GameObject> wolves = new List<GameObject>();
        List<Vector3> defaultTargetPoints = new List<Vector3>();
        List<Vector3> targetPoints = new List<Vector3>();
        GameObject[] destinationObjects;
        AudioSource source;
        int wolfIncrement = 3;
        int nrOfNightsPassed = 0;

        private void OnEnable()
        {
            GameEventsManager.current.OnNight += NightBegin;
            GameEventsManager.current.OnDay += DayBegin;
        }

        private void OnDisable()
        {
            GameEventsManager.current.OnNight -= NightBegin;
            GameEventsManager.current.OnDay -= DayBegin;
        }

        private void Start()
        {
            destinationObjects = GameObject.FindGameObjectsWithTag("Destination");
            source = GetComponent<AudioSource>();
            foreach (GameObject destination in destinationObjects)
            {
                defaultTargetPoints.Add(destination.transform.position);
            }
        }

        void NightBegin()
        {
            nrOfNightsPassed++;
            nrOfWolves = nrOfNightsPassed * wolfIncrement;
            nrOfWolves = Mathf.Clamp(nrOfWolves, 2, 14);

            // Create a copy of the default target points to populate a new list of target points
            targetPoints = defaultTargetPoints.ToList();

            source.Play();
            StartCoroutine(SpawnWolves());
        }

        void DayBegin()
        {
            StopCoroutine(SpawnWolves());
            wolves.Clear();
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
    }
}