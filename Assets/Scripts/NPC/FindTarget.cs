using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

[RequireComponent(typeof(AimConstraint))]
public class FindTarget : MonoBehaviour
{

    [SerializeField] string _targetTag = "Player";
    [SerializeField] float _searchRadius = 5f;

    private AimConstraint _aimConstraint;
    private ConstraintSource _constraintSource;

    private void Start()
    {
        // find the aim constraint we required
        _aimConstraint = GetComponent<AimConstraint>();

        // define the constraint source
        _constraintSource.sourceTransform = GameObject.FindWithTag(_targetTag).transform;
        _constraintSource.weight = 1f;

        // add the source
        _aimConstraint.constraintActive = true;
        //_aimConstraint.SetSource(0, _constraintSource);
        _aimConstraint.AddSource(_constraintSource);
        
    }

    private void Update()
    {
        float distance = Vector3.Distance(_constraintSource.sourceTransform.transform.position, transform.position);

        if (distance < _searchRadius)
        {
            _aimConstraint.enabled = true;
        }
        else
        {
            _aimConstraint.enabled = false;
        }
    }
}
