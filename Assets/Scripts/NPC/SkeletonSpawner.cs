using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SkeletonSpawner : MonoBehaviour
{
    //[SerializeField] LevelController _levelController;
    [SerializeField] GameObject _skeletonPrefab;
    [SerializeField] Transform _origin;
    [SerializeField] float _delay = 0f;

    private GameObject _currentSkeleton;
    private LevelController _levelController;

    private void Awake()
    {
        if (GameObject.FindWithTag("LevelController").TryGetComponent<LevelController>(out LevelController levelController))
        {
            _levelController = levelController;
        }

        Spawn();
    }

    private void OnEnable()
    {
        _levelController.RespawnEnemy.AddListener(Spawn);
    }

    private void OnDisable()
    {
        _levelController.RespawnEnemy.RemoveListener(Spawn);
    }

    IEnumerator RespawnRoutine()
    {
        yield return new WaitForSeconds(_delay);
        _currentSkeleton = Instantiate(_skeletonPrefab, _origin.position, _origin.rotation);
    }

    public void Spawn()
    {
        StartCoroutine(RespawnRoutine());
    }

}
