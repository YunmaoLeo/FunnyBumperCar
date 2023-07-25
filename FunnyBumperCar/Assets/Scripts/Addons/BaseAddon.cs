using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class BaseAddon : MonoBehaviour
{
    [SerializeField] public Transform Calibrator;

    public Rigidbody carRigidbody;

    public virtual void TriggerAddon(InputAction.CallbackContext context)
    {
    }

    public virtual void OnInitialState()
    {
        
    }

    private void Start()
    {
        OnInitialState();
    }
}
