using UnityEngine;
using UnityEngine.InputSystem;

public class ShovelAddon : AddonObject
{
    [SerializeField] private float targetHeightMinOffset = -0.15f;
    [SerializeField] private float targetHeightMaxOffset = -1f;

    private ConfigurableJoint joint;

    private void Start()
    {
        joint = GetComponent<ConfigurableJoint>();
        joint.targetPosition = new Vector3(0, targetHeightMinOffset, 0);
    }

    public override void InitializeBasePlatformRigidbody(Rigidbody rigidbody)
    {
        base.InitializeBasePlatformRigidbody(rigidbody);
        GetComponent<ConfigurableJoint>().connectedBody = rigidbody;
    }

    public override void TriggerAddon(InputAction.CallbackContext context)
    {
        float targetOffset = joint.targetPosition.y < targetHeightMinOffset
            ? targetHeightMinOffset
            : targetHeightMaxOffset;
        joint.targetPosition = new Vector3(0, targetOffset, 0);
    }
}