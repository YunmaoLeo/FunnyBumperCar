using UnityEngine;

public class EjectObject : StateNode
{
    [Input] public Transform EjectObjectPrefab;
    [Input] public Transform EjectPoint;
    [Input] public float EjectObjectInitialVelocity;
    [Output] public Transform EjectedObjectTransform;
}
