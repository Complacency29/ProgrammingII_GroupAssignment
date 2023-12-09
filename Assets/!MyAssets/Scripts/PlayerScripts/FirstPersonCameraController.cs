using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FirstPersonCameraController : MonoBehaviour
{
    [SerializeField, Range(.1f, 100f)] float mouseSensitivity = 2f;
    [SerializeField, Range(0, 90)] float verticalClampUpLimit = 75f;
    [SerializeField, Range(0, 90)] float verticalClampDownLimit = 75f;
    [SerializeField] Transform body;
    private float xRotation;

    private PhotonView _photonView;
    [SerializeField]private Camera _camera;

    InputMaster controls;

    private void Awake()
    {
        controls = new InputMaster();

        controls.PlayerMovement.Look.performed += context => MouseLook(context.ReadValue<Vector2>());
    }

    // Start is called before the first frame update
    void Start()
    {
        _photonView = GetComponentInParent<PhotonView>();
        Cursor.lockState = CursorLockMode.Locked;


        if (!_photonView.IsMine)
        {
            _camera.enabled = false;
        }
    }
    private void Update()
    {
        if (PauseMenu.isPaused == true)
        {
            Cursor.lockState = CursorLockMode.Confined;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        //Quaternion targetRotation = Quaternion.Euler(-21.894f, 180f, 0f);
    }

    void MouseLook(Vector2 _input)
    {
        //Debug.Log("Mouse look called.");

        float mouseX = _input.x * mouseSensitivity * Time.deltaTime;
        float mouseY = _input.y * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -verticalClampUpLimit, verticalClampDownLimit); //Prevents infinite rotation

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
