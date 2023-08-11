using System.Collections;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CannonAddon : AddonObject
{
    [SerializeField] private Transform cannonRotatePlatform;
    [SerializeField] private Transform cannonBarrel;

    [SerializeField] private Transform missileEjectTransform;
    [SerializeField] private float missileCoolDownTime = 0.5f;

    [SerializeField] private float aimTargetTimeConsumed = 0.5f;

    [SerializeField] private Transform missilePrefab;

    [SerializeField] private float recoilForceFactor;

    [SerializeField] private Ease aimRotateEase;

    private float missileCDTimer = 0f;

    private FixedJoint fixedJoint;

    private Transform targetCar;

    public override void InitializeBasePlatformRigidbody(Rigidbody rigidbody)
    {
        base.InitializeBasePlatformRigidbody(rigidbody);
        fixedJoint = GetComponent<FixedJoint>();
        fixedJoint.connectedBody = rigidbody;
    }

    private void FixedUpdate()
    {
        missileCDTimer -= Time.fixedDeltaTime;
    }

    private void AimingAtTargetPerFrame(Transform target)
    {
        var position = target.position;
        cannonRotatePlatform
            .DODynamicLookAt(new Vector3(position.x, cannonRotatePlatform.transform.position.y, position.z),
                aimTargetTimeConsumed).SetEase(aimRotateEase);

        //Barrel Rotate;
        Vector3 directionToTarget = position - cannonBarrel.position;
        cannonBarrel.DODynamicLookAt(cannonBarrel.transform.position - directionToTarget, aimTargetTimeConsumed)
            .SetEase(aimRotateEase);
    }

    private void StartFireMissile()
    {
        if (missileCDTimer > 0f)
        {
            return;
        }

        missileCDTimer = missileCoolDownTime;
        //1. Find the attack target;
        targetCar = CarsAndCameraManager.Instance.GetHostileCar(basePlatformRigidbody.transform);
        if (targetCar == null)
        {
            return;
        }


        //2. Send Missile & Add recoil force on car rigidbody;
        EjectMissile();
        targetCar = null;
    }

    IEnumerator EjectMissileCoroutine(float aimTimeConsumed)
    {
        yield return new WaitForSeconds(aimTimeConsumed);
        EjectMissile();

        targetCar = null;
    }

    private void EjectMissile()
    {
        if (missilePrefab != null)
        {
            var missile = Instantiate(missilePrefab, position: missileEjectTransform.position,
                rotation: missileEjectTransform.rotation);
            if (targetCar.GetComponent<Rigidbody>() == null)
            {
                return;
            }

            var missileComponent = missile.GetComponent<Missile>();
            missileComponent.AssignHomingTarget(targetCar.GetComponent<Rigidbody>());
            missileComponent.BasePlatformRigidbody = basePlatformRigidbody;
        }


        // add recoil force;
        basePlatformRigidbody.AddForceAtPosition(cannonBarrel.forward * recoilForceFactor,
            basePlatformRigidbody.position + fixedJoint.connectedAnchor, ForceMode.Impulse);
        Debug.DrawLine(basePlatformRigidbody.position + fixedJoint.connectedAnchor,
            basePlatformRigidbody.position + fixedJoint.connectedAnchor + cannonBarrel.forward * recoilForceFactor,
            Color.red,
            duration: 0.5f);
    }


    public override void TriggerAddon(InputAction.CallbackContext context)
    {
        base.TriggerAddon(context);
        StartFireMissile();
    }
}