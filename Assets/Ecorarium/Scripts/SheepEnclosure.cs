using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepEnclosure : MonoBehaviour
{

    private int wolfCollisions;
    GameObject positioning;

    private void Start()
    {
        positioning = new GameObject("Positioning");
        //StartCoroutine(CreateTransformPoints());
    }

    IEnumerator CreateTransformPoints()
    {
        Debug.Log(transform.position + " POSITION");
        Debug.Log(transform.localScale + " SCALE");
        float centerX = transform.localScale.x / 2;
        float plusX = transform.position.x + centerX;
        float minusX = transform.position.x - centerX;
        Debug.Log(transform.position.x + " POSITION X");
        Debug.Log(centerX + "CENTER X");
        for (int i = 0; i < 10; i++)
        {
            float x = Random.Range(plusX, minusX);
            Debug.Log(x + " XXX");
            Instantiate(positioning, new Vector3(x, transform.position.y, transform.position.z), Quaternion.identity);
            yield return null;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        wolfCollisions++;
        if (wolfCollisions >= 10)
        {
            Debug.Log("Complete");
        }
    }
}
