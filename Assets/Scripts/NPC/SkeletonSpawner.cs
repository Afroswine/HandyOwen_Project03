using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SkeletonSpawner : MonoBehaviour
{
    //[SerializeField] LevelController _levelController;
    [SerializeField] GameObject _skeletonPrefab;

    private GameObject _currentSkeleton;
    private LevelController _levelController;

    private void Awake()
    {
        //_levelController = GameObject.FindWithTag("LevelController").TryGetComponent<LevelController>(out LevelController levelController);
        if (GameObject.FindWithTag("LevelController").TryGetComponent<LevelController>(out LevelController levelController))
        {
            _levelController = levelController;
        }

        Respawn();
    }

    private void OnEnable()
    {
        _levelController.RespawnEnemy.AddListener(Respawn);
    }

    private void OnDisable()
    {
        _levelController.RespawnEnemy.RemoveListener(Respawn);
    }

    public void Respawn()
    {
        if(_currentSkeleton == null)
        {
            _currentSkeleton = Instantiate(_skeletonPrefab, transform.position, transform.rotation);
        }
        else
        {
            //Destroy(_currentSkeleton);
            //Died.Invoke();
            if(_currentSkeleton.TryGetComponent<NPCHealth>(out NPCHealth npcHealth))
            {
                npcHealth.Kill();
            }
            _currentSkeleton = Instantiate(_skeletonPrefab, transform.position, transform.rotation);
        }

        
    }
}
