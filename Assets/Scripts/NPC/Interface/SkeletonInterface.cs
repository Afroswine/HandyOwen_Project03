using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonInterface : EnemyInterface
{
    [Header("Skeleton")]
    [SerializeField] GameObject _skeletonBroken;
    [SerializeField] float _explosionForce = 150f;
    [SerializeField] float _explosionRadius = 5f;
    private Transform _explosionOrigin;

    public override void Die()
    {
        Instantiate(_skeletonBroken, transform.position, transform.rotation);
        CreateExplosiveForce();

        base.Die();
    }

    void CreateExplosiveForce()
    {
        Vector3 variance = new Vector3(Random.Range(-2f, 2f), Random.Range(-2f, 2f), Random.Range(-2f, 2f));

        Collider[] colliders = Physics.OverlapSphere(DeathFxOrigin.position + variance, _explosionRadius);
        //Collider[] colliders = _looseBones.GetComponentsInChildren<Collider>();
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.AddExplosionForce(_explosionForce, DeathFxOrigin.position + variance, _explosionRadius, 1.0f);
                //Debug.Log("ExplosionForce applied");
            }
        }

        Destroy(gameObject);
    }
}
