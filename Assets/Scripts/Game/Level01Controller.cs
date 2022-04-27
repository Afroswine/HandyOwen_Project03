using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;

public class Level01Controller : MonoBehaviour
{
    // Events
    public UnityEvent ToggleMenu;

    // Vars

    // Update is called once per frame
    private void Update()
    {
        // Exit Level
        //TODO bring up popup menu for navigation
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleMenu.Invoke();
        }
    }

}
