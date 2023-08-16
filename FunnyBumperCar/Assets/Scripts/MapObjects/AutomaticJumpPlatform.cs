
    using System;
    using UnityEngine;

    public class AutomaticJumpPlatform : MonoBehaviour
    {
        private Rigidbody rb;
        [SerializeField] private Vector3 RotateAxis;
        [SerializeField] private float maxAngle = 30f;
        [SerializeField] private float ejectVelocity = 2f;
        [SerializeField] private float revertVelocity = -0.5f;
        [SerializeField] private float ejectCD = 10f;

        private float ejectCDTimer = 5f;
        private bool isEjecting = false;
        private bool isReverting = false;
        private float maxEjectTime;
        private float currentEjectTime;

        private float maxRevertTime;
        private float currentRevertTime;
        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        private void StartEject()
        {
            isReverting = false;
            isEjecting = true;
            maxEjectTime = maxAngle / (ejectVelocity * 180 / Mathf.PI);
            currentEjectTime = 0f;
            ejectCDTimer = 0f;
        }

        private void StartRevert()
        {
            isEjecting = false;
            isReverting = true;
            maxRevertTime = maxAngle / (Mathf.Abs(revertVelocity) * 180 / Mathf.PI);
            currentRevertTime = 0f;
        }

        private void FixedUpdate()
        {

            if (!isEjecting && !isReverting)
            {
                
                ejectCDTimer += Time.fixedDeltaTime;
                if (ejectCDTimer > ejectCD)
                {
                    StartEject();
                }
            }
            
            if (isEjecting)
            {
                currentEjectTime += Time.fixedDeltaTime;
                if (currentEjectTime >= maxEjectTime)
                {
                    StartRevert();
                }
            }

            if (isReverting)
            {
                currentRevertTime += Time.fixedDeltaTime;
                if (currentRevertTime >= maxRevertTime)
                {
                    rb.MoveRotation(transform.parent.rotation);
                    isReverting = false;
                }
            }

            if (isEjecting)
            {
                ComputeTargetRotation(ejectVelocity);
            }

            if (isReverting)
            {
                ComputeTargetRotation(revertVelocity);
            }
        }

        private void ComputeTargetRotation(float velocity)
        {
            var rotateAngle = (float)(360f / (2f * Math.PI) * velocity * Time.fixedDeltaTime);

            var deltaQ = Quaternion.AngleAxis(rotateAngle, RotateAxis);
            var targetRotation = transform.rotation * deltaQ;
            rb.MoveRotation(targetRotation);
        }
    }
