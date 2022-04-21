using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : MonoBehaviour, IWeaponTypePistol
{
    #region Local Variable Initialization
    [Header("Main Weapon Transforms")]
    [SerializeField] Camera _cameraController;
    [SerializeField] Transform _pivotPoint;
    [SerializeField] Transform _rayOrigin;

    //Primary Action Stats
    [Header("Primary Action Stats")]
    [SerializeField] float _pDamage = 15f;
    [SerializeField] float _pActionSpeed = 6f;
    [SerializeField] int _pAmmoCurrent = 40;
    [SerializeField] int _pAmmoMax = 40;
    [SerializeField] float _pFalloffRange = 15f;
    [SerializeField] float _pMaxRange = 40f;
    [SerializeField] float _pAccuracyVariance = 0.035f;

    //Primary Action Visuals
    [Header("Primary Action Visuals")]
    [SerializeField] ParticleSystem _pFiredParticle;
    [SerializeField] ParticleSystem _pHitParticle;

    [Header("Collision Filter")]
    [SerializeField] LayerMask _hitLayers;
    RaycastHit _objectHit;

    //Primary Action States
    bool _pReady = false;
    bool _pIsReloading = false;
    bool _pAmmoFull = false;
    #endregion Local Variable Initialization End   (⌒ー⌒)ﾉ


    #region IWeaponTypePistol
    public Camera CameraController { get { return _cameraController; } }
    public Transform PivotPoint { get { return _pivotPoint; } }
    public Transform RayOrigin { get { return _rayOrigin; } }
    public LayerMask HitLayers { get { return _hitLayers; } }
    public RaycastHit ObjectHit { get { return _objectHit; } }
    #endregion IWeaponTypePistol (⌒ー⌒)ﾉ

    #region IPrimaryActionable
    //Stats
    public float PDamage { get { return _pDamage; } }
    public float PActionSpeed { get { return _pActionSpeed; } }
    public float PFalloffRange { get { return _pFalloffRange; } }
    public float PMaxRange { get { return _pMaxRange; } }
    public float PAccuracyVariance { get { return _pAccuracyVariance; } }

    //Ammo
    public int PCurrentAmmo { get { return _pAmmoCurrent; } }
    public int PMaxAmmo { get { return _pAmmoMax; } }

    //States
    public bool PReady { get { return _pReady; } private set { } }
    public bool PIsReloading { get { return _pIsReloading; } private set { } }
    public bool PAmmoFull { get { return _pAmmoFull; } private set { } }

    //Visuals
    public ParticleSystem PFiredParticle { get { return _pFiredParticle; } }
    public ParticleSystem PHitParticle { get { return _pHitParticle; } private set { } }
    #endregion IPrimaryActionable End   (⌒ー⌒)ﾉ

    void Update()
    {
        #region Weapon States
        /*
        // ReadyToFire Status
        if (!PIsReloading && PCurrentAmmo > 0)
        {
            PReady = true;
        }
        else
        {
            PReady = false;
        }

        // Maximum Ammo Status
        if(PCurrentAmmo >= PMaxAmmo)
        {
            PAmmoFull = true;
        }
        else
        {
            PAmmoFull = false;
        }
        */
        #endregion Weapon States End    (⌒ー⌒)ﾉ

        if (PReady)
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                StartCoroutine(PrimaryActionTimer());
            }
        }
    }

    public IEnumerator PrimaryActionTimer()
    {
        PReady = false;
        PrimaryAction();
        yield return new WaitForSeconds(1 / PActionSpeed);
        //PReady = true;
    }
    
    public void PrimaryAction()
    {

    }

    /*
    public void PrimaryAction()
    {
        PFiredParticle.Play();

        #region Raycast Collision Detection
        // calculate direction to shoot the ray with applied inaccuracy
        Vector3 deviation = new Vector3(
            Mathf.Lerp(-PAccuracyVariance, PAccuracyVariance, Random.Range(0f, 1f)),
            Mathf.Lerp(-PAccuracyVariance, PAccuracyVariance, Random.Range(0f, 1f)),
            Mathf.Lerp(-PAccuracyVariance, PAccuracyVariance, Random.Range(0f, 1f)));
        Vector3 rayDirection = CameraController.transform.forward + deviation;
        // cast a debug ray
        Debug.DrawRay(RayOrigin.position, rayDirection * PMaxRange, Color.blue, 1f);
        // do the raycast
        if (Physics.Raycast(RayOrigin.position, rayDirection, out ObjectHit, PMaxRange, HitLayers))
        {
            #region Hit Particle System (Visual Feedback)

            //Instantiate a new _hitPS
            _currentHitPS = Instantiate(_hitPSPrefab);

            // Create variables to change _hitPS's colorBySpeed gradient
            ParticleSystem particleSystem = _currentHitPS;
            var colorBySpeed = particleSystem.colorBySpeed;
            colorBySpeed.enabled = true;
            Gradient gradient = new Gradient();
            Color hitColor = Color.white;   //default value

            // Used to alter rotation of _hitPS based on normals and player.forward
            var psShape = particleSystem.shape;
            psShape.enabled = true;

            // Transform _currentHitPS
            _currentHitPS.transform.position = _objectHit.point;
            _currentHitPS.transform.forward = _objectHit.normal;
            psShape.rotation -= _cameraController.transform.forward / 2;  //rotate PS shape separately for decals

            // If possible, change hitColor to _objectHit's color
            Renderer renderer = _objectHit.transform.gameObject.GetComponent(typeof(Renderer)) as Renderer;
            if (renderer != null)
            {
                hitColor = renderer.material.color;
            }
            else
            {
                //If the parent had no color, get the first color from its children
                renderer = _objectHit.transform.gameObject.GetComponentInChildren(typeof(Renderer)) as Renderer;
                if (renderer != null)
                {
                    hitColor = renderer.material.color;
                }
            }

            //Adjust _hitPS's colorBySpeed gradient
            gradient.SetKeys
                (new GradientColorKey[] {
                new GradientColorKey(hitColor, 0.25f),
                new GradientColorKey(Color.yellow, 1.0f) },
                new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f) });
            colorBySpeed.color = gradient;

            //Play that darn Particle System!!!
            _currentHitPS.Play();
            #endregion

            // if enemy, apply damage
            if (_objectHit.transform.tag == "Enemy")
            {
                NPCHealth npcHealth = _objectHit.transform.gameObject.GetComponent<NPCHealth>();
                if (npcHealth != null)
                {
                    npcHealth.TakeDamage(_weaponDamage);
                }
            }
        }
        else
        {
            //you missed, oopsie
        }
        #endregion
    }
    */
}
