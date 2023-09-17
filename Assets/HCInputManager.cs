using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HCInputManager : MonoBehaviour
{
    [Header("Input Actions")]
    public InputActionReference SwitchSelectedHuman;
    public InputActionReference MoveSelectedHuman;

    [SerializeField] private LevelManager levelManager;
    // Start is called before the first frame update
    void Start()
    {
        SwitchSelectedHuman.action.Enable();
        MoveSelectedHuman.action.Enable();

        SwitchSelectedHuman.action.performed += SwitchSelectedHumanEvent;
        MoveSelectedHuman.action.performed += MoveSelectedHumanEvent;
    }

    private void SwitchSelectedHumanEvent(InputAction.CallbackContext context)
    {
        levelManager.SwitchSelectedHuman();
    }

    private void MoveSelectedHumanEvent(InputAction.CallbackContext context)
    {
        Vector2 movement = context.ReadValue<Vector2>();
        movement.y *= -1;

        levelManager.SelectedHuman.Move(Vector2Int.RoundToInt(movement));
    }

    private void OnDestroy()
    {
        SwitchSelectedHuman.action.performed -= SwitchSelectedHumanEvent;
        MoveSelectedHuman.action.performed -= MoveSelectedHumanEvent;
    }
}
