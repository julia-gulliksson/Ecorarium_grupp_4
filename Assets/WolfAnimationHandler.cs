using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WolfAnimationHandler : MonoBehaviour
{
    Animator wolfAnimation;
    float velocity;
    float normalizedVelocity;
    NavMeshAgent wolfNa;
    Wolf wolfController;
    
    void Start()
    {
        GameEventsManager.current.onWolfCollide += Attack;
        wolfAnimation = GetComponent<Animator>();
        wolfNa = GetComponent<NavMeshAgent>();
        wolfController = GetComponent<Wolf>();
        
    }

    private void Attack(bool obj, int id)
    {
        if(id == wolfController.id)
        {
            wolfAnimation.SetBool("Attack", obj);
            print("Attack " + id);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        wolfAnimation.SetFloat("Velocity", GetNormalVelocity());
    }

    private float GetNormalVelocity()
    {
        velocity = wolfNa.velocity.magnitude;
        normalizedVelocity = velocity / wolfNa.desiredVelocity.magnitude;
        return normalizedVelocity;
    }
}
