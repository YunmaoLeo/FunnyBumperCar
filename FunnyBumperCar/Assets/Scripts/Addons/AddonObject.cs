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
}
