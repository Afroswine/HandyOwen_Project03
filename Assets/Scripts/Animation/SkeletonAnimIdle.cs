using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class SkeletonAnimIdle : MonoBehaviour
{
    //[SerializeField] State
    [SerializeField] NPCHealth _health;
    [SerializeField] float _timeMin = 2f;
    [SerializeField] float _timeMax = 15f;
    //[SerializeField] string _subStateMachine;
    private float _waitTime;

    private Animator _animator;

    IEnumerator Start()
    {
        _animator = GetComponent<Animator>();
        //_health = GetComponentInParent<NPCHealth>();

        while (true)
        {
            _waitTime = Random.Range(_timeMin, _timeMax);
            yield return new WaitForSeconds(_waitTime);
            _animator.SetInteger("IdleIndex", Random.Range(0, 1));
            _animator.SetTrigger("IdleVariate");
        }
    }

    private void OnEnable()
    {
        _health.TookDamage.AddListener(TookDamage);
    }

    private void OnDisable()
    {
        _health.TookDamage.RemoveListener(TookDamage);
    }

    private void TookDamage()
    {
        _animator.SetTrigger("Hurt");
    }

}
