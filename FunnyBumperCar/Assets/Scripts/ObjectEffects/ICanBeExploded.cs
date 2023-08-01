    using UnityEngine;

    public interface ICanBeExploded
    {
        void BeExploded(Vector3 explosionCenter, float explosionIntensity, float explosionRadius);
    }
