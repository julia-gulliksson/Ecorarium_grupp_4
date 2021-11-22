using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfSpawn : MonoBehaviour
{
    [SerializeField] GameObject wolf;
    private List<GameObject> wolves = new List<GameObject>();
    [SerializeField] float waifForSeconds = 0.2f;

    void Start()
    {
        StartCoroutine(SpawnWolves());
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
                GameObject cloudObj = Instantiate(wolf, positioning, Quaternion.identity);
                wolves.Add(cloudObj);
            }

            yield return new WaitForSeconds(waifForSeconds);
        }
    }
}
