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
    
    void Start()
    {
        wolfAnimation = GetComponent<Animator>();
        wolfNa = GetComponent<NavMeshAgent>();
        
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
