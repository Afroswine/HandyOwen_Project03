using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class SkeletonBone : SkeletonInterface
{
    #region Original Version
    /*
    private float _timer = 3f;
    private Rigidbody _rb;
    private MeshCollider _mc;


    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _mc = GetComponent<MeshCollider>();
        
        // Do Environment Collisions only
        Physics.IgnoreLayerCollision(0, 10, true);
        Physics.IgnoreLayerCollision(8, 8, false);
    }


    IEnumerator DeactivatePhysics()
    {
        yield return new WaitForSeconds(_timer);
        Destroy(_rb);
        Destroy(_mc);
    }
    private void OnCollisionStay(Collision collision)
    {
        StartCoroutine(DeactivatePhysics());
    }
    */
    #endregion Original Version END


    private float _timer = 3f;
    private Rigidbody _rb;
    private MeshCollider _mc;

    private void Awake()
    {
        gameObject.AddComponent<Rigidbody>();

        _rb = GetComponent<Rigidbody>();
        _mc = GetComponent<MeshCollider>();

        // Do Environment Collisions only
        Physics.IgnoreLayerCollision(0, 10, true);
        Physics.IgnoreLayerCollision(8, 8, false);
    }

    IEnumerator DeactivatePhysics()
    {
        yield return new WaitForSeconds(_timer);
        Destroy(_rb);
        Destroy(_mc);
    }

    private void OnCollisionStay(Collision collision)
    {
        StartCoroutine(DeactivatePhysics());
    }
}
