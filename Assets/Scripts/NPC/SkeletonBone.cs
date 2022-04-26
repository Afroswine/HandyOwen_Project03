using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SkeletonBone : MonoBehaviour
{
    //[SerializeField] float _massModifier;

    private Rigidbody _rb;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void OnJointBreak()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
