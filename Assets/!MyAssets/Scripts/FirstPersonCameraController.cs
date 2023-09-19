using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCameraController : MonoBehaviour
{
  public float mouseSensitivity = 2f;

  private float xRotation;
  private float yRotation;


  // Start is called before the first frame update
  void Start()
  {
    Cursor.lockState = CursorLockMode.Locked;
  }

  // Update is called once per frame
  void Update()
  {
    float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
    float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

    xRotation -= mouseY;
    xRotation = Mathf.Clamp(xRotation, -90f, 90f); //Prevents infinite rotation

    yRotation += mouseX;

    transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
  }
}
