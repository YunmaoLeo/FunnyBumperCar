using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    public float effectRadius;
    [SerializeField] private float animationRadius = 5.12f;
    
    private void Start()
    {
        transform.localScale *= effectRadius / animationRadius;
        transform.LookAt(Camera.main.transform.position);
        Destroy(gameObject, animator.GetCurrentAnimatorStateInfo(0).length);
    }
}
