using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private InputActions inputAction;
    public InputActions.OnFootActions onFoot;
    private PlayerMovement playerMovement;
    private PlayerLook playerLook;
    private PlayerInventory _playerInventory;

    private void Awake()
    {
        inputAction = new InputActions();
        onFoot = inputAction.OnFoot;
        playerMovement = GetComponent<PlayerMovement>();
        playerLook = GetComponent<PlayerLook>();
        onFoot.Jump.performed += ctx => playerMovement.Jump();
        onFoot.Interact.performed += ctx => playerLook.FireInteractRay();
        //onFoot.PlaceObject.performed += GetComponent<ProjectileThrow>().ThrowObject;
        onFoot.Look.canceled += ctx => playerLook.ProcessLook(Vector2.zero);
        ProjectileThrow projectileThrow = GetComponent<ProjectileThrow>();
        onFoot.AimObject.performed += ctx => projectileThrow.StartAiming();
        onFoot.AimObject.canceled += ctx => projectileThrow.ThrowObject(ctx);
        onFoot.Sprint.performed += ctx => playerMovement.SprintMode(true);
        onFoot.Sprint.canceled += ctx => playerMovement.SprintMode(false);
        onFoot.Crouch.performed += ctx => playerMovement.CrouchingMode(true);
        onFoot.Crouch.canceled += ctx => playerMovement.CrouchingMode(false);
        onFoot.ExitGame.performed += ExitGame;
    }
    
    private void ExitGame(InputAction.CallbackContext ctx)
    {
        Debug.Log("Quitting game");
        onFoot.Disable();
        Application.Quit();

    }

    private void FixedUpdate()
    {
        playerMovement.ProcessMove(onFoot.Movement.ReadValue<Vector2>());
    }

    private void LateUpdate()
    {
        playerLook.ProcessLook(onFoot.Look.ReadValue<Vector2>());
    }
    private void OnEnable()
    {
        onFoot.Enable();
    }

    private void OnDisable()
    {
        onFoot.Disable();
    }
}
