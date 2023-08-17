using System;
using Cinemachine;
using DG.Tweening;
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
    [SerializeField] private Transform HookMesh;

    private FixedJoint fixedJoint;
    private Vector3 currentNormal;
    
    private void OnCollisionEnter(Collision collision)
    {
        if (!isActive || isAttaching)
        {
            return;
        }

        if (collision.transform.GetComponentInParent<CarBody>() == GetComponentInParent<CarBody>())
        {
            return;
        }

        attachedRb = collision.rigidbody;
        isAttaching = true;
        fixedJoint = transform.AddComponent<FixedJoint>();
        fixedJoint.connectedBody = attachedRb;
        InsertOrDrawHookMesh(0.2f, collision.contacts[0].normal);
    }

    private void InsertOrDrawHookMesh(float rate, Vector3 hitNormal)
    {
        var insetPos = HookMesh.position - hitNormal * rate * 0.4f;
        HookMesh.DOMove(insetPos, 0.1f);
    }

    private void FixedUpdate()
    {
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
        HookMesh.localPosition = Vector3.zero;
        Destroy(fixedJoint);
        attachedRb = null;
        // rb.isKinematic = false;
        isAttaching = false;
        isActive = false;
    }
}