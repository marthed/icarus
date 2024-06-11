using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class GlobalUserEvents : MonoBehaviour
{
    // Start is called before the first frame update

    public InputActionAsset actions;
    private InputAction thumbStickDown;
    public UnityEvent OnClick;
    void OnEnable()
    {
        Debug.Log("Enable!");

        thumbStickDown = actions.FindActionMap("XRI RightHand Interaction").FindAction("Menu");

        thumbStickDown.performed += OnThumbStickClicked;
    }

    void OnDisable() {
        thumbStickDown.performed -= OnThumbStickClicked;
    }

    private void OnThumbStickClicked(InputAction.CallbackContext context) {
        OnClick?.Invoke();
    }
}
