using System;
using UnityEngine;

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
}