using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation_script : MonoBehaviour
{
    [SerializeField] private Vector3 _rotation;
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(_rotation * Time.deltaTime);
    }
}
