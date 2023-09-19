using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCameraController : MonoBehaviour
{
    [SerializeField, Range(.1f, 3f)] float mouseSensitivity = 2f;
    [SerializeField] Transform Graphics;
    private float xRotation;
    private float yRotation;

    InputMaster controls;

    private void Awake()
    {
        controls = new InputMaster();

        controls.PlayerMovement.Look.performed += context => MouseLook(context.ReadValue<Vector2>());
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void MouseLook(Vector2 _input)
    {
        Debug.Log("Mouse look called.");

        float mouseX = _input.x * mouseSensitivity;
        float mouseY = _input.y * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); //Prevents infinite rotation

        yRotation += mouseX;

        //currently this just moves the transform, which would be the camera right now
        //should this be changed to rotate the graphics object or maybe the entire player object?
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }

    private void OnEnable()
    {
        controls.Enable();
    }
    private void OnDisable()
    {
        controls.Disable();
        controls.PlayerMovement.Move.performed -= context => MouseLook(context.ReadValue<Vector2>());
    }
}
