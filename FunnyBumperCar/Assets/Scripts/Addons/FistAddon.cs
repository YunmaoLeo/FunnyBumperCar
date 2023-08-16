
    using System;
    using UnityEngine;
    using UnityEngine.InputSystem;

    public class FistAddon : AddonObject
    {
        [Header("Properties")] [SerializeField] private float maxDistance = 0.5f;
        
        [Space]
        [Header("BaseRb&Joints")]
        [SerializeField] private ConfigurableJoint fistJoint;
        private FixedJoint fixedJoint;
        [SerializeField] private Rigidbody baseRb;
        [SerializeField] private Rigidbody fistRb;
        [SerializeField] private float minConnectLengthScale = 0f;
        [SerializeField] private float maxConnectLengthScale = 1f;
        [SerializeField] private Transform ConnectionTransform;


        private float initialFistDistance = 0f;
        private float TOLERANCE = 0.0001f;
        public override void InitializeBasePlatformRigidbody(Rigidbody rigidbody)
        {
            base.InitializeBasePlatformRigidbody(rigidbody);
            fixedJoint = GetComponent<FixedJoint>();
            fixedJoint.connectedBody = rigidbody;

            initialFistDistance = CalculateFistDistance();
        }

        private float CalculateFistDistance()
        {
            return Mathf.Abs(Vector3.Dot(transform.forward, fistRb.transform.position - baseRb.transform.position));
        }
        
        

        private void FixedUpdate()
        {
            var currentDistance = CalculateFistDistance();
            var currentScale = (currentDistance - initialFistDistance) 
                               / (initialFistDistance + maxDistance) 
                               * (maxConnectLengthScale - minConnectLengthScale) 
                               + minConnectLengthScale;
            var oldScaleVector = ConnectionTransform.localScale;
            oldScaleVector.z = currentScale;
            ConnectionTransform.localScale = oldScaleVector;
        }

        private void DoPunch()
        {
            if (Math.Abs(fistJoint.targetPosition.z - maxDistance) < TOLERANCE)
            {
                fistJoint.targetPosition = Vector3.zero;
            }
            else
            {
                fistJoint.targetPosition = new Vector3(0, 0, maxDistance);
            }
        }

        public override void TriggerAddon(InputAction.CallbackContext context)
        {
            base.TriggerAddon(context);
            DoPunch();
        }
    }
