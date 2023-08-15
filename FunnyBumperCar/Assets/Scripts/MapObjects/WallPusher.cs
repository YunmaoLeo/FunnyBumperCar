
    using System;
    using System.Security.Cryptography;
    using UnityEngine;
    using Random = System.Random;

    public class WallPusher: MonoBehaviour
    {
        private MeshFilter meshFilter;
        private float pusherHeight;
        private Rigidbody rb;
        [SerializeField] private float initialHeightRate = 0.2f;
        [SerializeField] private float springMinLengthRate = 0f;
        [SerializeField] private float springMaxLengthRate = 0.35f;
        [SerializeField] private float pushVelocity;

        [SerializeField] private bool isUpward = true;
        private void Awake()
        {
            // initialHeightRate = LocalRandom.Instance.GetRandomFloat(-initialHeightRate, initialHeightRate);
            // springMinLengthRate = 0f;
            // springMaxLengthRate = LocalRandom.Instance.GetRandomFloat(0.15f, springMaxLengthRate);
            // isUpward = LocalRandom.Instance.GetRandomFloat(-1f, 1f) > 0;
            // pushVelocity = LocalRandom.Instance.GetRandomFloat(pushVelocity / 2f, pushVelocity * 2f);
            
            rb = GetComponent<Rigidbody>();
            meshFilter = GetComponent<MeshFilter>();
            pusherHeight = meshFilter.mesh.bounds.size.z;

            transform.localPosition = new Vector3(0, 0, pusherHeight * initialHeightRate);
        }

        private void FixedUpdate()
        {
            var deltaY = pushVelocity * Time.fixedDeltaTime;
            float currentHeight = transform.parent.InverseTransformPoint(transform.position).z;

            var initialHeight = pusherHeight * initialHeightRate;
            var maxHeight = pusherHeight * springMaxLengthRate;
            var minHeight = pusherHeight * springMinLengthRate;

            if (isUpward && currentHeight >= maxHeight)
            {
                isUpward = !isUpward;
                currentHeight = maxHeight;
            }

            if (!isUpward && currentHeight <= minHeight)
            {
                isUpward = !isUpward;
                currentHeight = minHeight;
            }
            
            if (isUpward)
            {
                currentHeight += deltaY;
            }
            else
            {
                currentHeight -= deltaY;
            }

            var newLocalPosition = new Vector3(0, 0 ,currentHeight);
            rb.MovePosition(transform.parent.TransformPoint(newLocalPosition));
        }
    }
