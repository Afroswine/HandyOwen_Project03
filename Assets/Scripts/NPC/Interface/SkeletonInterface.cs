using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class SkeletonInterface : EnemyInterface, ITargetFinder
{
    #region Variables
    //

    #region Skeleton Variables
    //
    [Header("Skeleton")]
    //[SerializeField] GameObject _skeletonBroken;
    [SerializeField] Collider _worldCollider;
    [SerializeField] bool _reviving = true;
    [SerializeField] float _brokenDuration = 5f;
    [Header("Targeting")]
    [SerializeField] string _targetTag = "Player";
    [SerializeField] float _searchRadius = 5f;
    [Header("Break Explosion")]
    [SerializeField] float _explosionForce = 150f;
    [SerializeField] float _explosionRadius = 5f;
    [Header("Bones")]
    [SerializeField] float _physicsDuration = 3f;
    [SerializeField] GameObject[] _bones;

    private Rigidbody _mainRB;
    private Rigidbody _currentBoneRB;
    private MeshCollider _currentBoneMC;
    private bool _isIgnoringCollisions = false;
    //
    #endregion Skeleton Variables End

    #region ITargetFinder Variables
    //
    public string TargetTag { get { return _targetTag; } private set { _targetTag = value; } }
    public float Radius { get { return _searchRadius; } private set { _searchRadius = value; } }

    private AimConstraint _aimComponent;
    public AimConstraint AimComponent { get { return _aimComponent; } private set { _aimComponent = value; } }

    private ConstraintSource _aimSource;
    public ConstraintSource AimSource { get { return _aimSource; } private set { _aimSource = value; } }
    private bool _isSeekingTarget = true;
    public bool IsSeekingTarget { get { return _isSeekingTarget; } private set { _isSeekingTarget = value; } }
    //
    #endregion ITargetFinder Variables END

    //
    #endregion Variables END

    #region Original
    /*
    public override void Die()
    {
        Instantiate(_skeletonBroken, transform.position, transform.rotation);
        CreateExplosiveForce();

        base.Die();
    }

    void CreateExplosiveForce()
    {
        Vector3 variance = new Vector3(Random.Range(-2f, 2f), Random.Range(-2f, 2f), Random.Range(-2f, 2f));

        Collider[] colliders = Physics.OverlapSphere(DeathFxOrigin.position + variance, _explosionRadius);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.AddExplosionForce(_explosionForce, DeathFxOrigin.position + variance, _explosionRadius, 1.0f);
            }
        }

        //Destroy(gameObject);
    }
    */
    #endregion Original END

    #region ITargetFinder Methods
    //
    public void InitializeITargetFinder()
    {
        AimComponent = GetComponent<AimConstraint>();

        //AimSource.sourceTransform = GameObject.FindWithTag(TargetTag).transform;
        AimSource = _aimSource;
        _aimSource.sourceTransform = GameObject.FindWithTag(TargetTag).transform;
        _aimSource.weight = 1f;

        AimComponent.constraintActive = true;
        AimComponent.AddSource(AimSource);
        AimComponent.enabled = true;

    }

    public void SeekTarget()
    {
        float distance = Vector3.Distance(AimSource.sourceTransform.transform.position, transform.position);

        if (distance < _searchRadius)
        {
            AimComponent.enabled = true;
        }
        else
        {
            AimComponent.enabled = false;
        }
    }
    //
    #endregion ITargetFinder Methods END

    private void Start()
    {
        InitializeITargetFinder();
        _mainRB = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (IsSeekingTarget)
        {
            SeekTarget();
        }
    }

    #region Death & Revival
    //

    #region Death
    //
    public override void Die()
    {
        _worldCollider.enabled = false;

        BoneBreak();

        _isSeekingTarget = false;
        _mainRB.isKinematic = true;
        AimComponent.enabled = false;

        if (_reviving == true)
        {
            DestroyOnDeath = false;
            _reviving = false;
        }
        base.Die();

        Debug.Log(gameObject.name + " died");
    }

    private void CreateExplosiveForce()
    {
        Vector3 variance = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));

        Collider[] colliders = Physics.OverlapSphere(DeathFxOrigin.position + variance, _explosionRadius);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.AddExplosionForce(_explosionForce, DeathFxOrigin.position + variance, _explosionRadius, 1.0f);
            }
        }
    }

    private void BoneBreak()
    {
        ToggleLayerCollision();
        StartCoroutine(BrokenDurationRoutine());

        for (int i = 0; i < _bones.Length; i++)
        {
            _bones[i].GetComponent<MeshCollider>().isTrigger = false;

            if (_bones[i].TryGetComponent<ParentConstraint>(out ParentConstraint parentConstraint))
            {
                parentConstraint.constraintActive = false;
            }

            _bones[i].AddComponent<Rigidbody>();
            if(_bones[i].TryGetComponent<Rigidbody>(out Rigidbody rigidbody))
            {
                rigidbody.mass = 0.25f;
            }

            StartCoroutine(PhysicsDurationRoutine(_bones[i]));
        }

        CreateExplosiveForce();

        Debug.Log("BoneBreak()");
    }
    //
    #endregion Death END

    #region Revival
    //
    private void Revive()
    {
        //_worldCollider.enabled = true;
        Heal(MaxHealth);
        AimComponent.enabled = true;
        _isSeekingTarget = true;
        _mainRB.isKinematic = false;
        _worldCollider.enabled = true;
        //ToggleLayerCollision();

    }
    //
    #endregion Revival END

    #region IEnumerator Timers
    //
    IEnumerator PhysicsDurationRoutine(GameObject bone)
    {
        yield return new WaitForSeconds(_physicsDuration);
        Destroy(bone.GetComponent<Rigidbody>());
        bone.GetComponent<MeshCollider>().enabled = false;
    }

    IEnumerator BrokenDurationRoutine()
    {
        yield return new WaitForSeconds(_brokenDuration);
        Revive();
    }
    //
    #endregion IEnumerator Timers END

    private void ToggleLayerCollision()
    {
        //_isIgnoringCollisions = !_isIgnoringCollisions;

        // Do Environment Collisions only
        //Physics.IgnoreLayerCollision(0, 10, _isIgnoringCollisions);
        //Physics.IgnoreLayerCollision(8, 8, !_isIgnoringCollisions);

        Physics.IgnoreLayerCollision(0, 10, true);
        Physics.IgnoreLayerCollision(8, 8, false);
    }

    //
    #endregion Death & Revival END




}
