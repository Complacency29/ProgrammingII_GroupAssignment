using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
  public float moveSpeed;                //The current speed of the player
  public float walkSpeed = 5;            // You're not running
  public float runSpeed = 8;             // You're running
  public float jumpHeight = 1000;
  private int dirCorrection = -1; //Corrects for movement in a negative direction (backwards, left)
  private bool isJumping = false;

  public Rigidbody controller;

  // Start is called before the first frame update
  void Start()
  {
    controller = GetComponent<Rigidbody>();
  }

  // Update is called once per frame
  void Update()
  {
    //Determine if player character is sprinting or not
    if (Input.GetKey(KeyCode.LeftShift))
    {
      moveSpeed = runSpeed;
    }
    else
    {
      moveSpeed = walkSpeed;
    }

    Movement();
  }

  //Determines if and how the player character moves through world
  private void Movement()
  {
    if (Input.GetKey("w"))
    {
      controller.AddForce(transform.forward * moveSpeed);
    }

    if (Input.GetKey("s"))
    {
      controller.AddForce((transform.forward * dirCorrection) * moveSpeed);
    }

    if (Input.GetKey("a"))
    {
      controller.AddForce((transform.right * dirCorrection) * moveSpeed);
    }

    if (Input.GetKey("d"))
    {
      controller.AddForce(transform.right * moveSpeed);
    }

    if (Input.GetKeyDown("space"))
    {
      controller.AddForce(transform.up * moveSpeed * jumpHeight);
    }
  }
}

