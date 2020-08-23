using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Albarnie.InputManager;
using UnityEngine.InputSystem;

public class PlayerController2D : CharacterController2D
{
    private void Start()
    {
        InputManager.manager.AddInput("Movement", OnMove, InputType.OnPerformed);
        InputManager.manager.AddInput("Movement", OnMove, InputType.OnCancelled);
        InputManager.manager.AddEvent("Jump", OnJump, InputType.OnStarted);
    }

    void OnMove (InputAction.CallbackContext ctx)
    {
        movementInput.x = ctx.ReadValue<float>();
    }

    void OnJump ()
    {
        if (grounded)
            Jump();
    }
}
