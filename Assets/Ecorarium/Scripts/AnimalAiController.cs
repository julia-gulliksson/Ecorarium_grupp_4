using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;


public class AnimalAiController : MonoBehaviour
{
    NavMeshAgent animal;

    public LayerMask whatisWalkable, whatIsEatable;

    //Breeding
    int gender;


    //Memory
    [SerializeField]
    List<Vector3> memmoryFood = new List<Vector3>();

    //Hunger
    float hunger = 0;
    float timePassed;
    float hungerThreshhold, starvation;
    float timeBetweenTick = 1.0f;

    //Searching
    [SerializeField]
    Vector3 walkPoint;
    [SerializeField]
    bool walkPointSet;
    public float walkPointRange;

    //States
    public float sightRange, eatRange;
    public bool foodInSight, foodInEatRange;

    //Targeting
    GameObject target;

    private void Start()
    {
        EventHandler.current.OnBreedingCall += BreedingCall;
        animal = GetComponent<NavMeshAgent>();
        timePassed = Time.time;
        hungerThreshhold = 50.0f;
        starvation = 200.0f;
        gender = Random.Range(0, 2);
    }

    private void BreedingCall(Vector3 callPosition, int callerGender)
    {
        
    }

    private void CallToBreed(Vector3 position, int ownGender)
    {
        EventHandler.current.BreedingCall(transform.position, gender);
    }

    private void Update()
    {
        //Check if food is in Range
        
        foodInSight = Physics.CheckSphere(transform.position, sightRange, whatIsEatable);
        foodInEatRange = Physics.CheckSphere(transform.position, eatRange, whatIsEatable);

        if ((Time.time-timePassed)> timeBetweenTick )
        {
            timePassed = Time.time;
            hunger += 5;
            //print("Hunger: " + hunger + "\n´Time: " + Time.time);
        }

        
        if ((!foodInSight && !foodInEatRange) || hunger < hungerThreshhold) SearchForNeeds();
        if (foodInSight && !foodInEatRange && hunger >= hungerThreshhold) ChaseNeed();
        if (foodInSight && foodInEatRange && hunger >= hungerThreshhold) ConsumeNeed();

        if (hunger > starvation)
        {
            Destroy(gameObject);
        }
    }
    
    private void SearchForNeeds()
    {
        if ((!walkPointSet && hunger < hungerThreshhold) || (!walkPointSet && memmoryFood.Count == 0)) SeachForWalkPoint();
        if (!walkPointSet && hunger >= hungerThreshhold) SearchByMemory();

        if (walkPointSet) animal.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        if (distanceToWalkPoint.magnitude < 1.0f)
        {
            walkPointSet = false;
        }

    }
    private void SearchByMemory()
    {
        
        if (memmoryFood.Count != 0)
        {

            float error = Random.Range(-4.0f, 4.0f);
            walkPoint.x += error;
            walkPoint.z += error;
            int randomKey = Random.Range(0, memmoryFood.Count);
            float mX =  error;
            float mZ =  error;

            walkPoint = new Vector3(memmoryFood[randomKey].x + mX, transform.position.y, memmoryFood[randomKey].z + mZ);
            
            if (Physics.Raycast(walkPoint, -transform.up, 2f, whatisWalkable))
                walkPointSet = true;
        }

    }

    private void SeachForWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        Vector2 randomDirection = new Vector2(randomX, randomZ);
        
        walkPoint = new Vector3(transform.position.x + randomDirection.x, transform.position.y, transform.position.z + randomDirection.y);
       // walkPoint = walkPoint.normalized;
        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatisWalkable))
            walkPointSet = true;
            
                
    }

    private void ChaseNeed()
    {
        target = GetTarget();
        animal.SetDestination(target.transform.position);

    }

    private GameObject GetTarget()
    {
        Dictionary<float, Collider> shortestDistance = new Dictionary<float, Collider>();
        Collider[] food = Physics.OverlapSphere(transform.position, sightRange, whatIsEatable);
        foreach (Collider obj in food)
        {
            float randomScrambler = Random.Range(-0.05f, 0.05f);
            Vector3 distanceBetweenAnimalAndFood = obj.transform.position - transform.position;
            shortestDistance.Add(distanceBetweenAnimalAndFood.magnitude+ randomScrambler, obj);
        }
        float closestKey = shortestDistance.Keys.Min();
        GameObject foodTarget = shortestDistance[closestKey].gameObject;
        return foodTarget;
    }

    private void ConsumeNeed()
    {
        Destroy(target);
        hunger -= 50;
        memmoryFood.Add(transform.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        print("HIT Somthing");
        walkPointSet = false;
    }
}
