using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField,Range(1,100)] float baseMoveSpeed = 10f;

    CharacterController characterController;
    InputMaster controls;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();

        controls = new InputMaster();
        controls.PlayerMovement.Move.performed += context => PlayerMove(context.ReadValue<Vector2>());
    }

    void PlayerMove(Vector2 _input)
    {
        characterController.SimpleMove(_input * baseMoveSpeed * Time.deltaTime);
    }

    private void OnEnable()
    {
        controls.Enable();
    }
    private void OnDisable()
    {
        controls.Disable();

        //this can be moved to a method that disables/enables player movement
        //this could be controlled by a "Game State Manager"
        controls.PlayerMovement.Move.performed -= context => PlayerMove(context.ReadValue<Vector2>());
    }
}
