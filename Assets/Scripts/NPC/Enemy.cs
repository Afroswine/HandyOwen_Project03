using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    //[SerializeField] private NPCHealth _health;
    [Header("Visuals")]
    //[SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private GameObject _spawnFX;
    [SerializeField] private GameObject _deathFX;
    [SerializeField] private Transform _deathFXOrigin;
    //[SerializeField] private Color _dyingColor;
    [Header("Player Detection")]
    //[SerializeField] private Transform _target;
    [SerializeField] private float _detectionRadius;

    private NPCHealth _health;
    private GameObject _currentSpawnFX;
    private GameObject _currentDeathFX;
    private bool _inFullShatterState = false;
    private bool _inPartialShatterState = false;

    private void Awake()
    {
        _health = GetComponent<NPCHealth>();
        _currentSpawnFX = Instantiate(_spawnFX, transform.position, transform.rotation);
    }

    private void Update()
    {
        PlayerDetection();
    }

    private void OnEnable()
    {
        _health.TookDamage.AddListener(TookDamage);
        _health.Died.AddListener(Died);
    }

    private void OnDisable()
    {
        _health.TookDamage.RemoveListener(TookDamage);
        _health.Died.RemoveListener(Died);
    }

    private void PlayerDetection()
    {
        Transform target = GameObject.FindWithTag("Player").transform;

        float distance = Vector3.Distance(transform.position, target.position);

        if(distance <= _detectionRadius)
        {
            transform.LookAt(target);
        }
    }

    private void TookDamage()
    {
        FullShatter();
    }

    private void FullShatter()
    {
        _inFullShatterState = true;
    }

    private void PartialShatter()
    {
        _inPartialShatterState = true;
    }

    private void Died()
    {
        _currentDeathFX = Instantiate(_deathFX, _deathFXOrigin.position, _deathFXOrigin.rotation);


        Destroy(gameObject);
    }
}
