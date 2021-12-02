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
        
        wolfAnimation = GetComponent<Animator>();
        wolfNa = GetComponent<NavMeshAgent>();
        wolfController = GetComponent<Wolf>();
        
    }

   

    // Update is called once per frame
    void Update()
    {
        wolfAnimation.SetFloat("Velocity", GetNormalVelocity());

        if(wolfController.hasFoundTarget) wolfAnimation.SetBool("Attack", true);
    }

    private float GetNormalVelocity()
    {
        velocity = wolfNa.velocity.magnitude;
        normalizedVelocity = velocity / wolfNa.desiredVelocity.magnitude;
        return normalizedVelocity;
    }
}
