using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;


public class SheepMovement : MonoBehaviour
{
    NavMeshAgent animal;

    [SerializeField]
    Vector3 walkPoint;
    [SerializeField]
    bool walkPointSet;
    private AudioSource baa;


    public float walkPointRange = 2;

    float timer;
    float soundTimer;
    float freeWillBaa;

    public LayerMask whatisWalkable, whatIsEatable;
    void Start()
    {
        animal = GetComponent<NavMeshAgent>();
        baa = GetComponent<AudioSource>();
        freeWillBaa = Random.Range(1.0f, 80.0f);
        soundTimer = Time.time;


    }

    // Update is called once per frame
    void Update()
    {
        SearchForNeeds();
        if ((Time.time - timer) > 2.0f)
        {
            walkPointSet = false;

        }
        if ((Time.time - soundTimer) > freeWillBaa)
        {
            baa.Play();
            freeWillBaa = Random.Range(10.0f, 80.0f);
            soundTimer = Time.time;

        }
    }

    private void SearchForNeeds()
    {
        if ((!walkPointSet)) SeachForWalkPoint();
        

        if (walkPointSet) animal.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        if (distanceToWalkPoint.magnitude < 1.0f)
        {
            walkPointSet = false;
        }

    }

    private void SeachForWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        // walkPoint = walkPoint.normalized;
        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatisWalkable))
        {
            walkPointSet = true;
            timer = Time.time;
        }
            



    }
    private void OnTriggerEnter(Collider other)
    {
        print("HIT Somthing");
        walkPointSet = false;
    }
}
