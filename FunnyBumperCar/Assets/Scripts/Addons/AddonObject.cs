using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AddonObject : MonoBehaviour
{
    [SerializeField] public string AddonName;
    protected Rigidbody basePlatformRigidbody;
    protected NodeGraphHandler graphHandler;

    private void Awake()
    {
    }

    public virtual void TriggerAddon(InputAction.CallbackContext context)
    {
    }

    
    public virtual void InitializeBasePlatformRigidbody(Rigidbody rigidbody)
    {
        basePlatformRigidbody = rigidbody;
    }
    
    public virtual void OnInitialState()
    {
        
    }

    public virtual void SetEnable(bool enable)
    {
        if (TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.isKinematic = !enable;
        }

        if (TryGetComponent<Collider>(out Collider collider))
        {
            collider.enabled = enable;
        }

        this.enabled = enable;
    }
}
