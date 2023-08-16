using System;
using Unity.VisualScripting;
using UnityEngine;

public class Hook : MonoBehaviour
{
    // private ConfigurableJoint joint;
    private Rigidbody rb;
    private Rigidbody attachedRb;

    public bool isAttaching;
    private Vector3 HookRelativePosition;
    public bool isActive = false;
    public Vector3 AttachedPosition;

    private void Awake()
    {
        // joint = GetComponent<ConfigurableJoint>();
        // rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isActive || isAttaching) return;
        // rb.isKinematic = true;
        attachedRb = other.attachedRigidbody;

        AttachedPosition = other.ClosestPoint(transform.position);
        isAttaching = true;
        HookRelativePosition = other.transform.InverseTransformPoint(transform.position);
        Debug.Log("OnHookCollisionEnter");
    }

    private void FixedUpdate()
    {
        if (isAttaching)
        {
            if (attachedRb != null)
            {
                transform.position = attachedRb.position + HookRelativePosition;
                // rb.MovePosition(attachedRb.position + HookRelativePosition);
                // attachedRb.AddForceAtPosition(joint.currentForce, rb.position);
            }
            else
            {
                transform.position = AttachedPosition;
            }
        }
    }

    public void AddForceOnHookRope(Rigidbody hookBase, float force)
    {
        if (isAttaching)
        {
            var forceVec = (transform.position - hookBase.position) * force;
            if (attachedRb != null)
            {
                attachedRb.AddForce(-forceVec);
            }
            hookBase.AddForce(forceVec);
        }
    }

    public void LoseHook()
    {
        attachedRb = null;
        // rb.isKinematic = false;
        isAttaching = false;
        isActive = false;
    }
}