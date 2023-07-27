using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class JumpPlatformTrigger : MonoBehaviour
{
    [SerializeField] private Transform JumpPlatform;
    [SerializeField] private Transform TriggerMesh;

    [SerializeField] private float onPressYOffset = 0.3f;
    [SerializeField] private float triggerCoolDownTime = 2f;
    [SerializeField] private float pressDuration = 0.2f;
    [SerializeField] private float triggerDelayTime = 2f;

    private float coolDownTimer = 0f;
    private JumpPlatform jumpPlatform;
    private List<GameObject> objectsOnTriggerList = new List<GameObject>();

    private float triggerMeshInitialY;
    
    private void Awake()
    {
        jumpPlatform = JumpPlatform.GetComponent<JumpPlatform>();
        triggerMeshInitialY = TriggerMesh.transform.position.y;
    }

    private void FixedUpdate()
    {
        coolDownTimer -= Time.fixedDeltaTime;
    }

    private void TryTrigger()
    {
        if (objectsOnTriggerList.Count == 1 && coolDownTimer <= 0f)
        {
            coolDownTimer = triggerCoolDownTime;
            StartCoroutine(TriggerJumpPlatformAfterCertainTime(triggerDelayTime));
        }
    }

    IEnumerator TriggerJumpPlatformAfterCertainTime(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        jumpPlatform.Trigger();
    }

    private void HandleColliderEnter(Collider other)
    {
        if (other.CompareTag("Tire") || other.CompareTag("Car"))
        {
            if (!objectsOnTriggerList.Contains(other.gameObject))
            {
                objectsOnTriggerList.Add(other.gameObject);
            }
        }
        
        TriggerMesh.DOMoveY(triggerMeshInitialY - onPressYOffset, pressDuration);
        TryTrigger();
    }

    private void HandleColliderExit(Collider other)
    {
        if (other.CompareTag("Tire") || other.CompareTag("Car"))
        {
            if (objectsOnTriggerList.Contains(other.gameObject))
            {
                objectsOnTriggerList.Remove(other.gameObject);
            }
        }

        if (objectsOnTriggerList.Count == 0)
        {
            TriggerMesh.DOMoveY(triggerMeshInitialY, pressDuration);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        HandleColliderEnter(other);
    }

    private void OnTriggerExit(Collider other)
    {
        HandleColliderExit(other);
    }

    private void OnCollisionEnter(Collision collision)
    {
        HandleColliderEnter(collision.collider);
    }

    private void OnCollisionExit(Collision collision)
    {
        HandleColliderExit(collision.collider);
    }
}
