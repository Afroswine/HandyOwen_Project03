using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class SkeletonAnimIdle : MonoBehaviour
{
    //[SerializeField] State
    [SerializeField] float _timeMin = 0f;
    [SerializeField] float _timeMax = 25f;
    //[SerializeField] string _subStateMachine;
    private float _waitTime;

    private Animator _animator;

    IEnumerator Start()
    {
        _animator = GetComponent<Animator>();

        while (true)
        {
            _waitTime = Random.Range(_timeMin, _timeMax);
            yield return new WaitForSeconds(_waitTime);
            _animator.SetInteger("IdleIndex", Random.Range(0, 1));
            _animator.SetTrigger("IdleVariate");
        }
    }

}
