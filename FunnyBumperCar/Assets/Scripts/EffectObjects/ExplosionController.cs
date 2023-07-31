using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionController : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private void Start()
    {
        transform.LookAt(Camera.main.transform.position);
        Destroy(gameObject, animator.GetCurrentAnimatorStateInfo(0).length);
    }
}
