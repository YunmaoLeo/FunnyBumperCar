using UnityEngine;

public class AddonContainer : MonoBehaviour
{
    
    [SerializeField] public int ContainerID;
    [SerializeField] protected AddonObject Addon;


    public void SetEnable(bool enable)
    {
        if (Addon != null)
        {
            Addon.SetEnable(enable);
        }
    }

    public void AssignRigidbody(Rigidbody rigidbody)
    {
        if (Addon != null)
        {
            Addon.InitializeBasePlatformRigidbody(rigidbody);
        }
    }
}