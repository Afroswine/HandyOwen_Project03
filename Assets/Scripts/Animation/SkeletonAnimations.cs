using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class SkeletonAnimations : MonoBehaviour
{
    //[SerializeField] State
    [Header("References")]
    [SerializeField] Animator _anim;
    [SerializeField] SkeletonInterface _skeleton;
    //[SerializeField] NPCHealth _health;
    [Header("Idle Variation")]
    [SerializeField] float _timeMin = 3f;
    [SerializeField] float _timeMax = 9f;
    [Header("Hurt Event")]
    [SerializeField] ParticleSystem _dustDragPS;
    [SerializeField] ParticleSystem _dustPulsePS;

    private void OnEnable()
    {
        //_health.TookDamage.AddListener(TookDamage);
        _skeleton.TookDamage.AddListener(TookDamage);
    }

    private void OnDisable()
    {
        //_health.TookDamage.RemoveListener(TookDamage);
        _skeleton.TookDamage.RemoveListener(TookDamage);
    }

    IEnumerator Start()
    {
        while (true)
        {
            float waitTime = Random.Range(_timeMin, _timeMax);
            yield return new WaitForSeconds(waitTime);
            _anim.SetInteger("IdleIndex", Random.Range(0, 1));
            _anim.SetTrigger("IdleVariate");
        }
    }

    public void HurtAnimationEventStart()
    {

        var emission = _dustDragPS.emission;
        emission.enabled = true;
        _dustDragPS.Play();
    }

    public void HurtAnimationEventPulse()
    {
        _dustPulsePS.Play();
    }

    public void HurtAnimationEventEnd()
    {
        var emission = _dustDragPS.emission;
        _dustDragPS.Stop();
        emission.enabled = false;
    }

    private void TookDamage()
    {
        _anim.SetTrigger("Hurt");
    }

}
