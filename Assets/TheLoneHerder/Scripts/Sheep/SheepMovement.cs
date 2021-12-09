using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace TheLoneHerder
{
    public class SheepMovement : MonoBehaviour, IDestroyable
    {
        //Refrences
        [SerializeField] private GameObject gorePrefab;
        [SerializeField] Vector3 walkPoint;
        private AudioSource baa;
        NavMeshAgent animal;

        //Variables
        [SerializeField] private bool walkPointSet;
        [SerializeField] private float walkPointRange = 2;
        private float timer;
        private float soundTimer;
        private float freeWillBaa;

        public LayerMask whatisWalkable;
        void Start()
        {
            //Get components to refrences
            animal = GetComponent<NavMeshAgent>();
            baa = GetComponent<AudioSource>();
            //set first random time intervall before making baa sound
            freeWillBaa = Random.Range(1.0f, 80.0f);
            //set sound timer to current time
            soundTimer = Time.time;
        }

        void Update()
        {
            Walk();
            

            Baa();
        }

        private void Baa()
        {
            //Check if the soundTimer is greater than the randomly set freeWillBaa varibale
            if ((Time.time - soundTimer) > freeWillBaa)
            {
                //check if there is a baa sound present
                try
                {
                    //Play sound
                    baa.Play();
                }
                catch
                {
                    //Generate a warning if it is not present
                    Debug.LogWarning("Sheep audio source not found");
                }

                //Set a new random value for freeWillBaa, randomness is the defenition of free will... take that determinism!
                freeWillBaa = Random.Range(10.0f, 80.0f);
                //Reset timer
                soundTimer = Time.time;
            }
        }
        private void Walk()
        {
            //If walkPointSet is false, pick a walkpoint
            if ((!walkPointSet)) SeachForWalkPoint();

            //If walkPointSet is true set destination to walkPoint
            if (walkPointSet) animal.SetDestination(walkPoint);
            
            //Reset walkPointSet if close enough to walkPoint
            Vector3 distanceToWalkPoint = transform.position - walkPoint;
            if (distanceToWalkPoint.magnitude < 1.0f)
            {
                walkPointSet = false;
            }
            //Reset walkPointSet if too long time has passed before reaching destination
            if ((Time.time - timer) > 2.0f)
            {
                // Pick new destination
                walkPointSet = false;
            }

        }

        private void SeachForWalkPoint()
        {
            //Pick a random value depending on set walk range for X and Z axis
            float randomZ = Random.Range(-walkPointRange, walkPointRange);
            float randomX = Random.Range(-walkPointRange, walkPointRange);

            //Place the calculated random values in a new Vector3 and take the current position into acount
            walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

            //check if the pick position is placed on a surface that is walkable, if it is, set walkPointSet to true
            if (Physics.Raycast(walkPoint, -transform.up, 2f, whatisWalkable))
            {
                walkPointSet = true;
                //Set timer to current time, so that we can check if it takes to long for the sheep to reach it's destination
                timer = Time.time;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            //Reset walkpoint if sheep colides with somthing
            walkPointSet = false;
        }

        public void Damage()
        {
            //Damage effects
            Instantiate(gorePrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}