using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;

public class LevelController : MonoBehaviour
{
    public UnityEvent ToggleMenu;
    public UnityEvent RespawnEnemy;

    [SerializeField] KeyCode _pauseKey;
    [SerializeField] KeyCode _respawnKey;

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
}
