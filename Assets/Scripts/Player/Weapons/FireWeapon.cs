using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWeapon : MonoBehaviour
{
    [Header("Transform Information for Raycast")]
    [Tooltip("The raycast's forward direction will be set to this.")]
    [SerializeField] Camera _cameraController;
    [SerializeField] Transform _rayOrigin;
    [SerializeField] Transform _barrelLocator;

    [Header("Weapon Stats")]
    [SerializeField] float _shootDistance = 10f;
    [SerializeField] int _weaponDamage = 25;
    [SerializeField] float _roundsPerSecond = 8f;
    [SerializeField] float _maxAngleOfInnacuracy = 0.0f;

    [Header("Audio/Visual Feedback")]
    [SerializeField] Animator _pistolAnimator;
    [SerializeField] GameObject _fireFX;
    [SerializeField] GameObject _enemyHitFX;
    [SerializeField] GameObject _environmentHitFX;
    GameObject _currentFireFX;
    GameObject _currentHitFX;

    [Header("Collision Filter")]
    [SerializeField] LayerMask _hitLayers;


    private bool _readyToFire = true;

    RaycastHit _objectHit;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (_readyToFire == true)
            {
                StartCoroutine(ShootRoutine());
            }
        }
    }

    // Rounds Per Second Timer, paces weapon rate of fire
    IEnumerator ShootRoutine()
    {
        _readyToFire = false;
        Shoot();
        yield return new WaitForSeconds(1 / _roundsPerSecond);
        _readyToFire = true;
    }

    IEnumerator DestroyRoutine(GameObject gameObject, float timer)
    {
        yield return new WaitForSeconds(timer);
        Destroy(gameObject);
    }

    // fire the weapon
    void Shoot()
    {
        //Play FX
        _pistolAnimator.SetTrigger("GunFired");
        NewFireFX();

        // calculate direction to shoot the ray with applied inaccuracy
        Vector3 deviation = new Vector3(
            Mathf.Lerp(-_maxAngleOfInnacuracy, _maxAngleOfInnacuracy, Random.Range(0f, 1f)),
            Mathf.Lerp(-_maxAngleOfInnacuracy, _maxAngleOfInnacuracy, Random.Range(0f, 1f)),
            Mathf.Lerp(-_maxAngleOfInnacuracy, _maxAngleOfInnacuracy, Random.Range(0f, 1f)));
        Vector3 rayDirection = _cameraController.transform.forward + deviation;
        
        // cast a debug ray
        Debug.DrawRay(_rayOrigin.position, rayDirection * _shootDistance, Color.blue, 1f);

        // do the raycast
        if (Physics.Raycast(_rayOrigin.position, rayDirection, out _objectHit, _shootDistance, _hitLayers))
        {

            if (_objectHit.transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                NewHitFX(_enemyHitFX);

                NPCHealth npcHealth = _objectHit.transform.gameObject.GetComponent<NPCHealth>();
                if (npcHealth != null)
                {
                    npcHealth.TakeDamage(_weaponDamage);
                }
            }
            if (_objectHit.transform.gameObject.layer == LayerMask.NameToLayer("Environment"))
            {
                NewHitFX(_environmentHitFX);
            }

        }
    }

    void NewFireFX()
    {
        float destroyTimer = 0;
        _currentFireFX = Instantiate(_fireFX, _barrelLocator.position, _barrelLocator.rotation);
        //_currentFireFX.transform.position = _barrelLocator.position;
        //_currentFireFX.transform.forward = _barrelLocator.forward;

        if(_currentFireFX.TryGetComponent<AudioSource>(out AudioSource audioSource))
        {
            audioSource.Play();

            if (destroyTimer < audioSource.clip.length)
            {
                destroyTimer = audioSource.clip.length;
            }
        }
        if(_currentFireFX.TryGetComponent<ParticleSystem>(out ParticleSystem particleSystem))
        {
            particleSystem.Play();

            if (destroyTimer < particleSystem.main.duration)
            {
                destroyTimer = particleSystem.main.duration;
            }
        }

        StartCoroutine(DestroyRoutine(_currentFireFX, destroyTimer));

    }

    void NewHitFX(GameObject hitFX)
    {
        _currentHitFX = Instantiate(hitFX);
        _currentHitFX.transform.position = _objectHit.point;
        _currentHitFX.transform.forward = _objectHit.normal;
        Color colorRemap = Color.magenta;

        if (_currentHitFX.TryGetComponent<AudioSource>(out AudioSource audioSource))
        {
            audioSource.Play();
        }
        if (_currentHitFX.TryGetComponent<ParticleSystem>(out ParticleSystem particleSystem))
        {
            Gradient cbsOriginalGradient = particleSystem.colorBySpeed.color.gradient;
            Gradient cbsNewGradient = new Gradient();
            var colorBySpeed = particleSystem.colorBySpeed;
            var shape = particleSystem.shape;

            colorRemap = cbsOriginalGradient.Evaluate(0f);
            colorBySpeed.enabled = true;
            shape.enabled = true;

            shape.rotation += _cameraController.transform.forward / 2;

            // change hitColor to match the hit object
            Renderer renderer = _objectHit.transform.gameObject.GetComponent(typeof(Renderer)) as Renderer;
            if (renderer != null)
            {
                //colorRemap = renderer.material.color;
            }
            //If the parent had no color, get the first color from its children
            else
            {
                renderer = _objectHit.transform.gameObject.GetComponentInChildren(typeof(Renderer)) as Renderer;
                if (renderer != null)
                {
                    //colorRemap = renderer.material.color;
                    //Debug.Log("FirePS using color of children");
                }
            }

            //Adjust _hitPS's colorBySpeed gradient
            cbsNewGradient.SetKeys
                (new GradientColorKey[] {
                new GradientColorKey(colorRemap, 0.25f),
                new GradientColorKey(cbsOriginalGradient.Evaluate(1f), 0.7f) },
                new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f) });

            colorBySpeed.color = cbsNewGradient;

            //Play the effect
            particleSystem.Play();
        }
        if(_currentHitFX.TryGetComponent<SpriteRenderer>(out SpriteRenderer spriteRenderer))
        {
            //Create a replacement SpriteRenderer to be parented, thus avoiding scaling issues
            SpriteRenderer currentSprite = Instantiate(spriteRenderer);
            float offset = 0.02f;
            spriteRenderer.enabled = false;
            currentSprite.enabled = true;

            currentSprite.transform.localPosition += spriteRenderer.transform.forward * offset;
            currentSprite.color = colorRemap;
            currentSprite.transform.SetParent(_objectHit.transform);
        }
        //_currentHitFX.TryGetComponent<Animator>(out Animator animator);

    }

}
