using System;
using UnityEngine;

public class TireUtils
{
    public struct HitPointInfo
    {
        public RaycastHit raycastHit;
        public Vector3 hitPointForwardDirection;
        public Vector3 hitPointLateralDirection;
        public Vector3 hitPointVelocityOnTire;
        public Vector3 hitPointVelocityOnOther;
        public Rigidbody rb;
        public float hitPointLateralSpeed;
        public float hitPointForwardSpeed;
    }
}