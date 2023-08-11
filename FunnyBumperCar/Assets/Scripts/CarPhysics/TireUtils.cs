using UnityEngine;

public class TireUtils
{
    public struct HitPointInfo
    {
        public RaycastHit raycastHit;
        public Vector3 hitPointForward;
        public Vector3 hitPointSide;
        public Vector3 hitVelocity;
        public Rigidbody rb;
    }
}