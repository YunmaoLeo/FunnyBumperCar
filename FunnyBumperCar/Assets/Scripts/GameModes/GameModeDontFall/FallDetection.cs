using System;
using UnityEngine;

public class FallDetection : MonoBehaviour
{
    public Action<Transform> OnCarFallDetected;
    [SerializeField] private Transform CarFallAnimatonPrefeb;
    [SerializeField] private float fallAnimationScale = 10;
    [SerializeField] private float animationDuration = 0.5f;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            var fallAnimation = Instantiate(CarFallAnimatonPrefeb, other.transform.position, other.transform.rotation);
            fallAnimation.localScale = Vector3.one * fallAnimationScale;
            CarsAndCameraManager.Instance.RegisterVisualEffect(fallAnimation, animationDuration);
            Destroy(fallAnimation.gameObject, animationDuration);
            OnCarFallDetected.Invoke(other.transform);
        }
    }
}