using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine.InputSystem;

public class ChangeImageColorOnJoystickPress : MonoBehaviour
{
    public InputActionReference inputActionReference;
    public Color selectedColor = Color.blue;
    private bool isDefaultColor = true;
    private Color defaultColor;
    private Image image;

    void Awake()
    {
        image = GetComponent<Image>();
        defaultColor = image.color;
    }

    void OnEnable()
    {
        inputActionReference.action.performed += OnJoystickPressed;
    }

    void OnDisable()
    {
        inputActionReference.action.performed -= OnJoystickPressed;
    }

    void OnJoystickPressed(InputAction.CallbackContext context)
    {
        if (isDefaultColor)
        {
            image.color = selectedColor;
        }
        else
        {
            image.color = defaultColor;
        }
        isDefaultColor = !isDefaultColor;
    }
}