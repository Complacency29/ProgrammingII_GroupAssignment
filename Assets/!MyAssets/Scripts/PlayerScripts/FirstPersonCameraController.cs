using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCameraController : MonoBehaviour
{
    [SerializeField, Range(.1f, 100f)] float mouseSensitivity = 2f;
    [SerializeField] Transform body;
    private float xRotation;

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
        //Debug.Log("Mouse look called.");

        float mouseX = _input.x * mouseSensitivity * Time.deltaTime;
        float mouseY = _input.y * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); //Prevents infinite rotation

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        body.Rotate(Vector3.up * mouseX);
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
