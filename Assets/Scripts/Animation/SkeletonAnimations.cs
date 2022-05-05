using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Events;
using UnityEditor;
using System.IO;

[RequireComponent(typeof(Skeleton))]
[RequireComponent(typeof(Animator))]
public class SkeletonAnimations : MonoBehaviour
{
    [Header("References")]
    private Skeleton _skeleton;
    private Animator _animController;
    private GameObject[] _bones;
    [SerializeField] GameObject _boneLocator;

    [Header("Idle Variation")]
    [SerializeField] float _timeMin = 3f;
    [SerializeField] float _timeMax = 9f;

    [Header("Hurt Event")]
    [SerializeField] ParticleSystem _dustDragPS;
    [SerializeField] ParticleSystem _dustPulsePS;

    [Header("Reformation")]
    private float _reformDuration;
    
    public UnityEvent Reformed;



    #region OnEnable / OnDisable
    private void OnEnable()
    {
        //_health.TookDamage.AddListener(TookDamage);
        _skeleton = GetComponent<Skeleton>();
        _animController = GetComponent<Animator>();
        _bones = _skeleton.Bones;
        _reformDuration = _skeleton.BrokenDuration;


        _skeleton.TookDamage.AddListener(TookDamage);
        _skeleton.Died.AddListener(Died);
        _skeleton.Reform.AddListener(Reform);
    }

    private void OnDisable()
    {
        //_health.TookDamage.RemoveListener(TookDamage);
        _skeleton.TookDamage.RemoveListener(TookDamage);
        _skeleton.Died.RemoveListener(Died);
        _skeleton.Reform.RemoveListener(Reform);
    }
    #endregion OnEnable / OnDisable END

    IEnumerator Start()
    {

        // Randome Idle Picker
        while (true)
        {
            float waitTime = Random.Range(_timeMin, _timeMax);
            yield return new WaitForSeconds(waitTime);
            _animController.SetInteger("IdleIndex", Random.Range(0, 1));
            _animController.SetTrigger("IdleVariate");
        }
    }

    private void TookDamage()
    {
        _animController.SetTrigger("Hurt");
    }

    private void Died()
    {
        //_anim.enabled = false;
    }

    #region Hurt
    ///
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
    //
    #endregion Hurt END

    #region Revival
    //
    private void Reform()
    {
        Debug.Log("Began Reformation");
        for(int i = 0; i < _bones.Length; i++)
        {
            ParentConstraint parentConstraint = _bones[i].GetComponent<ParentConstraint>();
            parentConstraint.constraintActive = true;
            GameObject start;
            start = Instantiate(_boneLocator, _bones[i].transform.position, _bones[i].transform.rotation);
            start.transform.SetParent(gameObject.transform);
            ConstraintSource constraint1 = parentConstraint.GetSource(0);
            ConstraintSource constraint2 = new ConstraintSource();
            constraint2.sourceTransform = start.transform;
            constraint1.weight = 0;
            constraint2.weight = 1;
            parentConstraint.SetSource(0, constraint1);
            parentConstraint.AddSource(constraint2);

            StartCoroutine(LerpParentConstraint(parentConstraint, 0, 0, 1));
            StartCoroutine(LerpParentConstraint(parentConstraint, 1, 1, 0));
            StartCoroutine(RegenerateHealth());
        }

        StartCoroutine(WaitForReformed());
    }

    IEnumerator LerpParentConstraint(ParentConstraint parentConstraint, int index, float a, float b)
    {

        float currentTime = 0f;
        float endTime = currentTime + _reformDuration;
        
        ConstraintSource source = parentConstraint.GetSource(index);
        
        //bool condition = true;
        while (true)
        {
            //currentTime += Time.deltaTime;

            source.weight = Mathf.Lerp(a, b, (currentTime / endTime));
            parentConstraint.SetSource(index, source);

            currentTime += Time.deltaTime;

            // end condition
            if (source.weight == b)
            {
                //Debug.Log("LerpParentConstraint Stop");
                yield break;
            }
            else
            {
                yield return new WaitForEndOfFrame();
            }
        }
    }

    IEnumerator RegenerateHealth()
    {

        //float rate = _skeleton.MaxHealth / _reformDuration;
        //float amount = rate;

        float currentTime = 0f;
        float endTime = currentTime + _reformDuration;

        float initialHealth = _skeleton.CurrentHealth;

        //bool condition = true;
        while (true)
        {
            int health = Mathf.RoundToInt(Mathf.Lerp(initialHealth, _skeleton.MaxHealth, currentTime / endTime));
            _skeleton.SetHealth(health);

            currentTime += Time.deltaTime;
            if (_skeleton.CurrentHealth >= _skeleton.MaxHealth)
            {
                //Debug.Log("RegenerateHeatlh Stop");
                yield break;
            }
            else
            {
                yield return new WaitForEndOfFrame();
            }
        }
    }

    IEnumerator WaitForReformed()
    {
        yield return new WaitForSeconds(_reformDuration);
        for (int i = 0; i < _bones.Length; i++)
        {
            ParentConstraint constraint = _bones[i].GetComponent<ParentConstraint>();
            GameObject obj = constraint.GetSource(1).sourceTransform.gameObject;
            constraint.RemoveSource(1);
            Destroy(obj);

        }
        Reformed.Invoke();
    }

    //
    #endregion Revival END

}
