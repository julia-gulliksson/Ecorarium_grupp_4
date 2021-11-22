using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wolf : MonoBehaviour
{
    [SerializeField] float speed = 6f;
    [SerializeField] Transform target;
    public Vector3 targetPoint;

    private void Start()
    {
        Debug.Log("Transform point: " + targetPoint);
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, targetPoint) > 0.5f)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetPoint, step);
        }
    }
}
