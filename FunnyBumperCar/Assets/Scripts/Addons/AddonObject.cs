using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AddonObject : MonoBehaviour
{
    protected Rigidbody carRigidbody;

    public virtual void TriggerAddon(InputAction.CallbackContext context)
    {
    }
    
    public virtual void InitializeCarRigidbody(Rigidbody rigidbody)
    {
        carRigidbody = rigidbody;
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
