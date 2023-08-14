
    using Cinemachine;
    using UnityEngine;
    using UnityEngine.InputSystem;

    public class FreeLookCameraInputHandler : MonoBehaviour, AxisState.IInputAxisProvider
    {
        public bool isMouse = false;
        public InputAction horizontal;
        [HideInInspector] public InputAction vertical;
        public float XFactor = 192f;
        public float YFactor = 108f;

        public float GetAxisValue(int axis)
        {
            switch (axis)
            {
                case 0: return horizontal.ReadValue<Vector2>().x / (isMouse ? XFactor : 1f);
                case 1: return horizontal.ReadValue<Vector2>().y / (isMouse ? YFactor : 1f);
                case 2: return vertical.ReadValue<float>();
            }

            return 0;
        }
    }
