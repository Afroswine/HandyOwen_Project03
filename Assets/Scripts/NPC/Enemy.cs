﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NPCHealth))]
public class Enemy : MonoBehaviour
{
    [Header("References")]
    [SerializeField] LevelController _levelController;
    [Header("Visuals")]
    [SerializeField] private GameObject _spawnFX;
    [SerializeField] private GameObject _deathFX;
    [SerializeField] private Transform _deathFXOrigin;

    private NPCHealth _health;


    private void Awake()
    {
        _levelController = GameObject.FindWithTag("LevelController").GetComponent<LevelController>();

        _health = GetComponent<NPCHealth>();
        if(_spawnFX != null)
        {
            Instantiate(_spawnFX, transform);
        }
    }

    private void OnEnable()
    {
        _health.TookDamage.AddListener(TookDamage);
        _health.Died.AddListener(Died);
        _levelController.RespawnEnemy.AddListener(Died);
    }

    private void OnDisable()
    {
        
        _health.TookDamage.RemoveListener(TookDamage);
        _health.Died.RemoveListener(Died);
        _levelController.RespawnEnemy.RemoveListener(Died);
    }

    private void TookDamage()
    {
        
    }

    private void Died()
    {
        _levelController.IncreaseKillCount();

        if(_deathFX != null)
        {
            Instantiate(_deathFX, _deathFXOrigin.position, _deathFXOrigin.rotation);
        }


        Destroy(gameObject);
    }
}
