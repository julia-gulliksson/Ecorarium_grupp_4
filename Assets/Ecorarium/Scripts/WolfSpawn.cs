using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfSpawn : MonoBehaviour
{
    [SerializeField] GameObject wolf;
    private List<GameObject> wolves = new List<GameObject>();
    [SerializeField] float waifForSeconds = 0.2f;
    [SerializeField] Transform sheepEnclosure;
    private List<Vector3> targetPoints = new List<Vector3>();

    void Start()
    {
        StartCoroutine(CreateTargetPoints());
    }



    private void Update()
    {
        if (targetPoints.Count == 10)
        {
            StartCoroutine(SpawnWolves());
        }
    }

    IEnumerator CreateTargetPoints()
    {
        float centerX = sheepEnclosure.localScale.x / 2;
        float plusX = sheepEnclosure.position.x + centerX;
        float minusX = sheepEnclosure.position.x - centerX;
        while (targetPoints.Count < 10)
        {
            float x = Random.Range(plusX, minusX);
            Vector3 targetPoint = new Vector3(x, sheepEnclosure.position.y, sheepEnclosure.position.z);
            targetPoints.Add(targetPoint);
            yield return null;
        }
    }

    IEnumerator SpawnWolves()
    {
        while (wolves.Count < 10)
        {
            float x = Random.Range(-10, 10);
            float y = 0.5f;
            float z = Random.Range(-10, 10);
            Vector3 positioning = new Vector3(x, y, z);

            bool isTooClose = false;

            for (int i = 0; i < wolves.Count; i++)
            {
                if (Vector3.Distance(wolves[i].transform.position, positioning) < 0.4f)
                {
                    isTooClose = true;
                }
            }

            if (!isTooClose)
            {
                GameObject wolfObj = Instantiate(wolf, positioning, Quaternion.identity);
                Wolf wolfScript = wolfObj.GetComponent<Wolf>();
                Debug.Log(targetPoints[0]);

                wolves.Add(wolfObj);
                int wolfIndex = wolves.FindIndex(w => w == wolfObj);
                wolfScript.targetPoint = targetPoints[wolfIndex];
                targetPoints.Remove(targetPoints[wolfIndex]);
                targetPoints.ForEach(t => Debug.Log(t + " TARGET POINT"));
                Debug.Log(targetPoints.Count + " TAGET POINT COUNT");
            }


            yield return new WaitForSeconds(waifForSeconds);
        }
    }
}
