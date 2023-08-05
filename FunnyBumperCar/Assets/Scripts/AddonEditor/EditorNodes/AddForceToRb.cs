using UnityEngine;

public class AddForceToRb : StateNode
{
    [Input] public Rigidbody targetRb;
    [Input] public Vector3 Force;
}