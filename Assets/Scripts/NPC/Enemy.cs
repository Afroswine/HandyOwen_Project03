﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NPCHealth))]
public class Enemy : MonoBehaviour
{

    //[SerializeField] private NPCHealth _health;
    [Header("Visuals")]
    //[SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private GameObject _spawnFX;
    [SerializeField] private GameObject _deathFX;
    [SerializeField] private Transform _deathFXOrigin;
    //[SerializeField] private Color _dyingColor;

    private NPCHealth _health;
    //private GameObject _currentSpawnFX;
    //private GameObject _currentDeathFX;

    private void Awake()
    {
        _health = GetComponent<NPCHealth>();
        if(_spawnFX != null)
        {
            Instantiate(_spawnFX, transform);
        }
    }

    private void Update()
    {

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

    private void TookDamage()
    {
        // nothing
    }

    private void Died()
    {
        if(_deathFX != null)
        {
            Instantiate(_deathFX, _deathFXOrigin.position, _deathFXOrigin.rotation);
        }

        Destroy(gameObject);
    }
}
