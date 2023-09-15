using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public KeyboardControls controls;

    public static InputManager Instance => _instance;

    private static InputManager _instance;

    private bool _initialized;

    private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(_instance.gameObject);
        }

        _instance = this;

        Initialize();

        _initialized = true;
    }

    public void Initialize() {
        controls = new KeyboardControls();
        controls.Enable();
        Debug.Log("Controls initialized!");
    }

    public void ActivateMenuProfile() {
        controls.Menu.Enable();
        controls.Gameplay.Disable();
    }

    public void ActivateGameplayProfile() {
        controls.Menu.Disable();
        controls.Gameplay.Enable();
        Debug.Log("Gameplay map activated!");
    }

    public float GetMovementAxis() {
        return controls.Gameplay.Move.ReadValue<Vector2>().x;
    }

    public float GetVerticalAxis() {
        return controls.Gameplay.Move.ReadValue<Vector2>().y;
    }

    public bool GetBtnA()
    {
        return controls.Gameplay.ABtn.ReadValue<float>() > 0;
    }

    public bool GetBtnB()
    {
        return controls.Gameplay.BBtn.ReadValue<float>() > 0;
    }

    public bool GetBtnStart()
    {
        return controls.Gameplay.StartBtn.ReadValue<float>() > 0;
    }

    public bool GetBtnSelect()
    {
        return controls.Gameplay.SelectBtn.ReadValue<float>() > 0;
    }

    public bool IsDone()
    {
        return _initialized;
    }
}
