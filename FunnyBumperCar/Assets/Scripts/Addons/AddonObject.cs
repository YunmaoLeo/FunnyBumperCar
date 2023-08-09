using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class AddonObject : MonoBehaviour
{
    [Serializable]
    public enum AddonObjectEnum
    {
        CannonTower,
        CircleThruster,
        SpringPillar,
        Shovel,
        EMagnet,
    }

    [SerializeField] public AddonObjectEnum AddonType;
    [SerializeField] public string AddonName;
    protected Rigidbody basePlatformRigidbody;
    protected NodeGraphHandler graphHandler;

    public List<ConfigSlideRangeCommand<float>> ConfigFloatSlideRangeCommandsList;

    [Serializable]
    public struct ConfigSlideRangeCommand<T> where T : IComparable<T>
    {
        public string description;
        public T min;
        public T max;
        public Action<T> OnValueLegallyChanged;
        private bool IsInputValid(T input)
        {
            return input.CompareTo(min) >=0 && input.CompareTo(max) <= 0;
        }

        public void CheckAndTriggerChangeEvent(T input)
        {
            if (IsInputValid(input))
            {
                OnValueLegallyChanged?.Invoke(input);
            }
        }
    }
    
    private void Awake()
    {
    }

    private void Start()
    {
        OnInitialState();
    }

    public virtual void OnEquipOnCar(CarBody carBody)
    {
        
    }

    public virtual void OnRemoveFromCar(CarBody carBody)
    {
        
    }

    public virtual void TriggerAddon(InputAction.CallbackContext context)
    {
    }

    
    public virtual void InitializeBasePlatformRigidbody(Rigidbody rigidbody)
    {
        basePlatformRigidbody = rigidbody;
    }
    
    public virtual void OnInitialState()
    {
        InitializeConfigSlideRangeCommands();
    }

    public virtual void InitializeConfigSlideRangeCommands()
    {
        
    }

    public virtual void SetEnable(bool enable)
    {
        if (TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.isKinematic = !enable;
        }

        if (TryGetComponent<Collider>(out Collider collider))
        {
            collider.enabled = enable;
        }

        this.enabled = enable;
    }
}
