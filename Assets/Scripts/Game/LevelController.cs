using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;

public class LevelController : MonoBehaviour
{
    [SerializeField] Text _currentKillCountTextView;
    
    public UnityEvent ToggleMenu;
    public UnityEvent RespawnEnemy;

    //int _currentKillCount;
    int _killCount = 0;

    [SerializeField] KeyCode _pauseKey;
    [SerializeField] KeyCode _respawnKey;

    private void Start()
    {
        _killCount = PlayerPrefs.GetInt("KillCount");
        _currentKillCountTextView.text = "SKELETONS SLAIN: " + _killCount.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        // Open in-game menu
        if (Input.GetKeyDown(_pauseKey))
        {
            ToggleMenu.Invoke();
        }

        if (Input.GetKeyDown(_respawnKey))
        {
            InvokeRespawnEnemy();
        }
    }

    public void InvokeRespawnEnemy()
    {
        RespawnEnemy.Invoke();
    }

    public void IncreaseKillCount()
    {
        _killCount++;
        PlayerPrefs.SetInt("KillCount", _killCount);
        _currentKillCountTextView.text = "SKELETONS SLAIN: " + _killCount.ToString();
    }

    public void ResetLevel()
    {
        SceneManager.LoadScene("SkeletonShatter");
    }
}
