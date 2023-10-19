using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace ModuleSnapping
{
<<<<<<< HEAD
    [SerializeField] string playerName = "Player_01";
    public string PlayerName { get { return playerName; } }

    [Header("Movement Settings")]
    [SerializeField, Range(1,100)] float baseMoveSpeed = 10f;
    [SerializeField, Range(.1f, 2f)] float jumpHeight = 2f;
    [SerializeField, Range(-50,0)] float gravity = -9.81f;
=======
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField, Range(1, 100)] float baseMoveSpeed = 10f;
        [SerializeField, Range(.1f, 2f)] float jumpHeight = 2f;
        [SerializeField, Range(-50, 0)] float gravity = -9.81f;
>>>>>>> origin/main

        [Header("Configuration Settings")]
        [SerializeField, Range(0, 1)] float groundCheckDistance = .4f;
        [SerializeField] LayerMask walkableLayers;

<<<<<<< HEAD
    [Header("Components")]
    [SerializeField] Transform groundChecker;
    [SerializeField] Animator graphics;
=======
        [Header("Components")]
        [SerializeField] Transform groundChecker;
        [SerializeField] Animator animator;
>>>>>>> origin/main

    CharacterController characterController;
    InputMaster controls;
    Vector3 velocity;
    bool isGrounded = true;
    private PhotonView photonView;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
        characterController = GetComponent<CharacterController>();

        controls = new InputMaster();
    }

<<<<<<< HEAD
    private void Update()
    {
        if (photonView.IsMine == false)
            return;

        photonView.RPC("MoveRPC", RpcTarget.All);
    }
    [PunRPC]
    void MoveRPC()
    {
        isGrounded = Physics.CheckSphere(groundChecker.position, groundCheckDistance, walkableLayers);
        if (isGrounded && velocity.y < 0)
=======
        private void Start()
>>>>>>> origin/main
        {
            /*Generator gen = FindObjectOfType<Generator>();
            PhotonView view = GetComponent<PhotonView>();

            gen.OnPhotonPlayerConnected(view.Owner);*/
        }

        private void Update()
        {
            isGrounded = Physics.CheckSphere(groundChecker.position, groundCheckDistance, walkableLayers);
            if (isGrounded && velocity.y < 0)
            {
                //we are grounded
                velocity.y = -10f;
            }
            else
            {
                //we are not grounded
            }

            //Movement gubbins
            Vector2 input = controls.PlayerMovement.Move.ReadValue<Vector2>();
            PlayerMove(input);
            AnimationController(input);

            //Jump gubbins
            //if we are pressing the jump button AND we are grounded
            if (controls.PlayerMovement.Jump.ReadValue<float>() > 0 && isGrounded)
            {
                Debug.Log("Jumping");
                //set the y velocity to the amount required to reach the specified jump height based on gravity
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }

            velocity.y += gravity * Time.deltaTime;
            characterController.Move(velocity * Time.deltaTime);
        }

<<<<<<< HEAD
        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }
    void PlayerMove(Vector2 _input)
    {
        Vector3 movement = transform.right * _input.x + transform.forward * _input.y;

        characterController.Move(movement * baseMoveSpeed * Time.deltaTime);
    }
    void AnimationController(Vector2 _input)
    {
        bool moving;

        if (photonView.IsMine == false)
            return;

        if(_input.x > .1f || _input.x <= -.1f || _input.y > .1f || _input.y <= -.1f)
        {
            moving = true;
            graphics.SetBool("moving", true);
            graphics.SetFloat("inputX", _input.x);
            graphics.SetFloat("inputY", _input.y);
=======
        void PlayerMove(Vector2 _input)
        {
            Vector3 movement = transform.right * _input.x + transform.forward * _input.y;

            characterController.Move(movement * baseMoveSpeed * Time.deltaTime);
>>>>>>> origin/main
        }
        void AnimationController(Vector2 _input)
        {
<<<<<<< HEAD
            moving = false;
            graphics.SetBool("moving", false);
            graphics.SetFloat("inputX", 0);
            graphics.SetFloat("inputY", 0);
        }
        if (photonView.IsMine)
        {
            photonView.RPC("UpdateAnimation", RpcTarget.Others, moving, _input.x, _input.y);
        }
    }
    [PunRPC]
    private void UpdateAnimation(bool moving, float inputX, float inputY)
    {
        if (moving)
        {
            graphics.SetBool("moving", true);
            graphics.SetFloat("inputX", inputX);
            graphics.SetFloat("inputY", inputY);
        }
        else
        {
            graphics.SetBool("moving", false);
            graphics.SetFloat("inputX", 0);
            graphics.SetFloat("inputY", 0);
        }
    }
    private void OnEnable()
    {
        controls.Enable();
    }
    private void OnDisable()
    {
        controls.Disable();
    }
=======
            if (_input.x > .1f || _input.x <= -.1f || _input.y > .1f || _input.y <= -.1f)
            {
                animator.SetBool("moving", true);
                animator.SetFloat("inputX", _input.x);
                animator.SetFloat("inputY", _input.y);
            }
            else
            {
                animator.SetBool("moving", false);
                animator.SetFloat("inputX", 0);
                animator.SetFloat("inputY", 0);
            }
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
            //controls.PlayerMovement.Move.performed -= context => PlayerMove(context.ReadValue<Vector2>());
        }
>>>>>>> origin/main

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundChecker.position, groundCheckDistance);
        }
    }
}

