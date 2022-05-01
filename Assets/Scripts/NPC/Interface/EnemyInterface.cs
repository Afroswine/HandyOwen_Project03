using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class EnemyInterface : MonoBehaviour, IHealth
{
    [Header("Stats")]
    [SerializeField] int _maxHealth;
    [SerializeField] int _startingHealth;
    [Header("Visuals")]
    [SerializeField] private GameObject _spawnFX;
    [SerializeField] private GameObject _deathFX;
    [SerializeField] private Transform _deathFXOrigin;
    public Transform DeathFxOrigin => _deathFXOrigin;

    public UnityEvent TookDamage;
    public UnityEvent Died;
    private LevelController _levelController;

    #region IHealth
    // IHealth Begin
    public int MaxHealth { get; private set; }
    public int StartingHealth { get; private set; }
    public int CurrentHealth { get; private set; }

    public virtual void ApplyDamage(int damage)
    {
        CurrentHealth -= damage;
        TookDamage.Invoke();

        if(CurrentHealth <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        _levelController.IncreaseKillCount();
        Died.Invoke();

        if (_deathFX != null)
        {
            Instantiate(_deathFX, _deathFXOrigin.position, _deathFXOrigin.rotation);
            Debug.Log("_deathFX instansiated");
        }

        Destroy(gameObject);
    }
    // IHealth End
    #endregion IHealth ***End***

    private void OnEnable()
    {
        _levelController.RespawnEnemy.AddListener(Die);
    }

    private void OnDisable()
    {
        _levelController.RespawnEnemy.RemoveListener(Die);
    }

    private void Awake()
    {
        MaxHealth = _maxHealth;
        CurrentHealth = _startingHealth;

        _levelController = GameObject.FindWithTag("LevelController").GetComponent<LevelController>();

        if (_spawnFX != null)
        {
            Instantiate(_spawnFX, transform);
        }
    }

}
