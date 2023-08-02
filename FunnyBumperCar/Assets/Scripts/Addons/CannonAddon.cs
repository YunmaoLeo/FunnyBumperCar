using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class CannonAddon : AddonObject
{
    [SerializeField] private Transform cannonRotatePlatform;
    [SerializeField] private Transform cannonBarrel;

    [SerializeField] private Transform missileEjectTransform;
    [SerializeField] private float barrelXAngleMin = -20f;
    [SerializeField] private float barrelXAngleMax = 20f;
    [SerializeField] private float missileCoolDownTime = 0.5f;

    [SerializeField] private float aimTargetTimeConsumed = 0.5f;

    [SerializeField] private Transform missilePrefab;

    [SerializeField] private float recoilForceFactor;

    [SerializeField] private Ease aimRotateEase;

    private float missileCDTimer = 0f;

    private FixedJoint fixedJoint;

    private bool isAimingTarget = false;
    private Transform targetCar;

    public override void InitializeCarRigidbody(Rigidbody rigidbody)
    {
        base.InitializeCarRigidbody(rigidbody);
        fixedJoint = GetComponent<FixedJoint>();
        fixedJoint.connectedBody = rigidbody;
    }

    private void FixedUpdate()
    {
        missileCDTimer -= Time.fixedDeltaTime;

        if (isAimingTarget)
        {
            AimingAtTargetPerFrame(targetCar);
        }
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
        // var targetCar = CarsManager.Instance.GetHostileCar(carRigidbody.transform);
        targetCar = CarsAndCameraManager.Instance.GetHostileCar(carRigidbody.transform);

        //2. Rotate cannon platform and barrel;
        isAimingTarget = true;

        //3. Send Missile & Add recoil force on car rigidbody;
        StartCoroutine(EjectMissileCoroutine(aimTargetTimeConsumed));
    }

    IEnumerator EjectMissileCoroutine(float aimTimeConsumed)
    {
        yield return new WaitForSeconds(aimTimeConsumed);
        EjectMissile();

        isAimingTarget = false;
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
            missile.GetComponent<Missile>().AssignHomingTarget(targetCar.GetComponent<Rigidbody>());
        }


        // add recoil force;
        carRigidbody.AddForceAtPosition(cannonBarrel.forward * recoilForceFactor,
            carRigidbody.position + fixedJoint.connectedAnchor, ForceMode.Impulse);
        Debug.DrawLine(carRigidbody.position + fixedJoint.connectedAnchor,
            carRigidbody.position + fixedJoint.connectedAnchor + cannonBarrel.forward * recoilForceFactor, Color.red,
            duration: 0.5f);
    }


    public override void TriggerAddon(InputAction.CallbackContext context)
    {
        StartFireMissile();
    }
}