using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    public InputPad InputPad { get; private set; }
    public InputPad.PlayerActions PlayerAction { get; private set; }

    private void Awake()
    {
        InputPad = new InputPad();
        InputPad.Enable();
        PlayerAction = InputPad.Player;
    }

    private void OnDestroy()
    {
        InputPad.Disable();
    }

    public void LockInput(bool isLocked)
    {
        if (isLocked)
        {
            InputPad.Disable();
        }
        else
        {
            InputPad.Enable();
        }
    }

    public void DisableActionFor(InputAction action, float seconds)
    {
        StartCoroutine(DisableAction(action, seconds));
    }

    private IEnumerator DisableAction(InputAction action, float seconds)
    {
        action.Disable();
        yield return new WaitForSeconds(seconds);
        action.Enable();
    }
}