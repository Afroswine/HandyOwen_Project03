using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : MonoBehaviour
{

    [SerializeField] NPCHealth _health;
    [SerializeField] GameObject _looseBones;

    [SerializeField] Transform _explosionOrigin;
    [SerializeField] float _explosionForce = 150f;
    [SerializeField] float _explosionRadius = 5f;

    private void OnEnable()
    {
        _health.Died.AddListener(Break);
    }

    private void OnDisable()
    {
        _health.Died.RemoveListener(Break);
    }

    private void Break()
    {
        Instantiate(_looseBones, transform.position, transform.rotation);
        CreateExplosiveForce();
        //Destroy(gameObject);
    }

    void CreateExplosiveForce()
    {
        Vector3 variance = new Vector3(Random.Range(-2f, 2f), Random.Range(-2f, 2f), Random.Range(-2f, 2f));

        Collider[] colliders = Physics.OverlapSphere(_explosionOrigin.position + variance, _explosionRadius);
        //Collider[] colliders = _looseBones.GetComponentsInChildren<Collider>();
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.AddExplosionForce(_explosionForce, _explosionOrigin.position + variance, _explosionRadius, 1.0f);
                //Debug.Log("ExplosionForce applied");
            }
        }

        Destroy(gameObject);
    }
}
