using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] float _duration;
    //[SerializeField] LayerMask _hitLayers;

    private Rigidbody _rigidBody;

    public void Fire(Vector3 force)
    {
        StartCoroutine(DestroyRoutine());
        _rigidBody = GetComponent<Rigidbody>();
        _rigidBody.AddForce(force, ForceMode.Force);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            Destroy(gameObject);
        }
        if(other.transform.tag == "Environment")
        {
            Destroy(gameObject);
        }
    }

    IEnumerator DestroyRoutine()
    {
        yield return new WaitForSeconds(_duration);
        Destroy(gameObject);
    }
}
