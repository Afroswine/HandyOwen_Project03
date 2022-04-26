using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SkeletonSpawner : MonoBehaviour
{

    [SerializeField] GameObject _skeletonPrefab;
    private GameObject _currentSkeleton;

    //public static SkeletonSpawner Instance = null;

    private void Awake()
    {
        RespawnSkeleton();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            RespawnSkeleton();
        }
    }

    public void RespawnSkeleton()
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
