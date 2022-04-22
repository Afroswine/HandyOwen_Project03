using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    //[SerializeField] private NPCHealth _health;
    [Header("Visuals")]
    //[SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private GameObject _deathFX;
    [SerializeField] private Transform _deathFXOrigin;
    //[SerializeField] private Color _dyingColor;
    [Header("Player Detection")]
    //[SerializeField] private Transform _target;
    [SerializeField] private float _detectionRadius;

    private NPCHealth _health;
    private GameObject _currentDeathFX;
    private bool _inFullShatterState = false;
    private bool _inPartialShatterState = false;

    private void Awake()
    {
        _health = GetComponent<NPCHealth>();
        if(_deathFXOrigin == null)
        {
            _deathFXOrigin = transform;
        }
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
        #region Death FX
        _currentDeathFX = Instantiate(_deathFX);
        _deathFX.transform.position = _deathFXOrigin.position;
        _deathFX.transform.rotation = _deathFXOrigin.rotation;
        _deathFX.transform.localScale = _deathFXOrigin.localScale;
        #endregion Death FX End

        Destroy(gameObject);
    }
}
