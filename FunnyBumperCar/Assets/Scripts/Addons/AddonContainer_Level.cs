
using System;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
    public class AddonContainer_Level : AddonContainer
    {
        private Rigidbody rb;
        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            
            Addon.InitializeBasePlatformRigidbody(rb);
        }
    }
