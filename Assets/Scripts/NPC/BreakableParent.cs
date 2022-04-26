using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class BreakableParent : MonoBehaviour
{
    [SerializeField] float _massMultiplier = 1f;

    [SerializeField] List<GameObject> _allBreakableParts = new List<GameObject>();
    [SerializeField] List<GameObject> _partialBreakParts = new List<GameObject>();

    
    private void Awake()
    {
        // give all gameObjects in the list the SkeletonBone script
        // i feel like this is the wrong way to do this
        foreach(GameObject gameObject in _allBreakableParts)
        {
            gameObject.AddComponent<SkeletonBone>();
            gameObject.GetComponent<Rigidbody>().mass *= _massMultiplier;
            gameObject.GetComponent<SkeletonBone>().enabled = false;
            
        }
    }

    void FullBreak()
    {
        foreach (GameObject gameObject in _allBreakableParts)
        {
            Break(gameObject);
        }
    }

    void PartialBreak()
    {
        foreach (GameObject gameObject in _partialBreakParts)
        {
            Break(gameObject);
        }
    }

    void Break(GameObject gameObject)
    {
        SkeletonBone _currentPart = gameObject.GetComponent<SkeletonBone>();
        _currentPart.enabled = true;

    }

}
