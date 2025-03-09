using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    public static Action OnDropClick;
    
    private InputSystem_Actions _control;

    private void Start()
    {
        _control = new InputSystem_Actions();
        _control.Player.Enable();

        _control.Player.Click.started += OnClick;
        _control.Player.Tap.started += OnClick;
    }

    private void OnDestroy()
    {
        _control.Player.Click.started -= OnClick;
        _control.Player.Tap.started -= OnClick;
        _control.Player.Disable();
    }

    private void OnClick(InputAction.CallbackContext obj)
    {
        OnDropClick?.Invoke();
    }
}
