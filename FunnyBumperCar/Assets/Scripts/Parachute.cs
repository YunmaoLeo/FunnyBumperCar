using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parachute : MonoBehaviour
{
    private Rigidbody carRigidbody;
    private CarSimulation carSimulation;

    private Animator animator;

    [SerializeField] [Range(0.1f, 2f)]
    private float upliftForceCoefficient = 0.5f;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        gameObject.SetActive(false);
    }

    public void OpenParachute()
    {
        
       
        gameObject.SetActive(true);
    }


    public void CloseParachute()
    {
        gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        // var forwardVelocity = Vector3.Dot(carRigidbody.transform.forward, carRigidbody.velocity);
        var velocity = carRigidbody.velocity.magnitude;
        var maxUpliftForce = carRigidbody.mass * -Physics.gravity;


        var upDirectionFactor = Math.Clamp(Vector3.Dot(carRigidbody.transform.up, Vector3.up),0,1);

        var currentUpliftForce = maxUpliftForce * (Math.Abs(velocity) / carSimulation.MaxEngineVelocity * upliftForceCoefficient * upDirectionFactor);
        carRigidbody.AddForce(currentUpliftForce);
        
        Debug.DrawLine(transform.position, transform.position + (currentUpliftForce / carRigidbody.mass / 2f), Color.red);
    }

    public void SetCarRigidbody(Rigidbody rigidbody)
    {
        carRigidbody = rigidbody;
    }
    
    public void SetCarSimulation(CarSimulation simulation)
    {
        carSimulation = simulation;
    }

    
    
}
