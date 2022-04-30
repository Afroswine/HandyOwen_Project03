using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class SkeletonBone : MonoBehaviour
{
    //[SerializeField] float _massModifier;

    private float _timer = 3f;
    private Rigidbody _rb;
    private MeshCollider _mc;
    private Transform _rootBone;


    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _mc = GetComponent<MeshCollider>();

        StartCoroutine(DeactivatePhysics());
    }

    IEnumerator DeactivatePhysics()
    {
        yield return new WaitForSeconds(_timer);
        Destroy(_rb);
        Destroy(_mc);
    }

}
