using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenMenu : MonoBehaviour
{
    [SerializeField] LevelController _levelController;
    [SerializeField] PlayerMovement _playerMovement;
    [SerializeField] MouseLook _mouseLook;
    [SerializeField] FireWeapon _weapon;

    private bool _isEnabled = false;
    private Canvas _canvas;

    private GameObject _player;

    private void Awake()
    {
        _canvas = GetComponent<Canvas>();
        _canvas.enabled = false;

    }

    private void OnEnable()
    {
        _levelController.ToggleMenu.AddListener(Toggle);
    }

    private void OnDisable()
    {
        _levelController.ToggleMenu.RemoveListener(Toggle);
    }

    private void Toggle()
    {
        _isEnabled = !_isEnabled;

        if(_isEnabled == true)
        {
            _canvas.enabled = true;

            _playerMovement.enabled = false;
            _mouseLook.enabled = false;
            _weapon.enabled = false;

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            _canvas.enabled = false;

            _playerMovement.enabled = true;
            _mouseLook.enabled = true;
            _weapon.enabled = true;

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
