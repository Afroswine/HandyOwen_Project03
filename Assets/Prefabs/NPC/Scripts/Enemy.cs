using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    //[SerializeField] private NPCHealth _health;
    [Header("Visuals")]
    //[SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private GameObject _deathFX;
    //[SerializeField] private Color _dyingColor;
    [Header("Player Detection")]
    [SerializeField] private Transform _target;
    [SerializeField] private float _detectionRadius;
    /*
    [Header("Attack Stats")]
    [SerializeField] private EnemyProjectile _projectile;
    [SerializeField] private float _projectileSpeed;
    [SerializeField] private float _roundsPerSecond;
    */


    private NPCHealth _health;
    //private Color _startingColor;
    //private Color _currentColor;
    private EnemyProjectile _currentProjectile;
    private ParticleSystem _currentDeathPS;
    private AudioSource _currentDeathAS;
    //private bool _isReady = true;

    private void Awake()
    {
        _health = GetComponent<NPCHealth>();
        //_startingColor = _meshRenderer.material.color;
        //_currentColor = _startingColor;
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

    /*
    IEnumerator AttackRoutine()
    {
        _isReady = false;
        Attack();
        yield return new WaitForSeconds(1 / _roundsPerSecond);
        _isReady = true;
    }
    */

    private void PlayerDetection()
    {
        float distance = Vector3.Distance(transform.position, _target.position);

        if(distance <= _detectionRadius)
        {
            transform.LookAt(_target);
            /*
            if (_isReady)
            {
                StartCoroutine(AttackRoutine());
            }
            */
        }
    }

    /*
    private void Attack()
    {
        _currentProjectile = Instantiate(_projectile);
        _currentProjectile.transform.position = transform.position;
        _currentProjectile.transform.forward = transform.forward;
        _currentProjectile.Fire(_currentProjectile.transform.forward * _projectileSpeed);
    }
    */

    private void TookDamage()
    {
        #region Update Material Color
        /*
        //Change enemy's material color as they take damage
        float healthPercent = (float)_health.CurrentHealth / (float)_health.MaxHealth;
        healthPercent = Mathf.Clamp(healthPercent, 0.1f, 1f);

        _currentColor.r = Mathf.Lerp(_dyingColor.r, _startingColor.r, healthPercent);
        _currentColor.g = Mathf.Lerp(_dyingColor.g, _startingColor.g, healthPercent);
        _currentColor.b = Mathf.Lerp(_dyingColor.b, _startingColor.b, healthPercent);
        _currentColor.a = Mathf.Lerp(_dyingColor.a, _startingColor.a, healthPercent);

        _meshRenderer.material.color = _currentColor;
        */
        #endregion

    }
    
    private void Died()
    {
        //_currentDeath
        _currentDeathPS = Instantiate(_deathFX.GetComponent<ParticleSystem>());
        _currentDeathPS.transform.position = transform.position;
        _currentDeathPS.transform.localScale = transform.localScale;
        _currentDeathPS.Play();

        _currentDeathAS = Instantiate(_deathFX.GetComponent<AudioSource>());
        _currentDeathAS.transform.position = transform.position;
        _currentDeathAS.transform.localScale = transform.localScale;
        _currentDeathAS.Play();

        Destroy(gameObject);
    }
}
