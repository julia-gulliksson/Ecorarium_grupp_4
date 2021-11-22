using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wolf : MonoBehaviour
{
    [SerializeField] float speed = 6f;
    [SerializeField] Transform target;
    void Start()
    {

    }

    void Update()
    {
        if (Vector3.Distance(transform.position, target.position) > 0.5f)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.position.x, transform.position.y, target.position.z), step);
        }
    }
}
