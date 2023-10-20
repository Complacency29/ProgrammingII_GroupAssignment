using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace ModuleSnapping
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private PhotonView view;
        [SerializeField] private Transform _camera;

        [Header("Movement Settings")]
        [SerializeField, Range(1, 100)] float baseMoveSpeed = 10f;
        [SerializeField, Range(.1f, 2f)] float jumpHeight = 2f;
        [SerializeField, Range(-50, 0)] float gravity = -9.81f;

        [Header("Configuration Settings")]
        [SerializeField, Range(0, 1)] float groundCheckDistance = .4f;
        [SerializeField] LayerMask walkableLayers;

        [Header("Components")]
        [SerializeField] Transform groundChecker;
        [SerializeField] Animator animator;

        CharacterController characterController;
        InputMaster controls;
        Vector3 velocity;
        bool isGrounded = true;

        private void Awake()
        {
            characterController = GetComponent<CharacterController>();
            //animator = GetComponent<Animator>();

            controls = new InputMaster();
            //controls.PlayerMovement.Move.performed += context => PlayerMove(context.ReadValue<Vector2>());
        }

        private void Start()
        {
            view = GetComponent<PhotonView>();

            if(!view.IsMine && _camera != null)
            {
                Destroy(_camera.gameObject);
            }
        }

        private void Update()
        {
            if(!view.IsMine)
            {
                return;
            }

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

        void PlayerMove(Vector2 _input)
        {
            Vector3 movement = transform.right * _input.x + transform.forward * _input.y;

            characterController.Move(movement * baseMoveSpeed * Time.deltaTime);
        }
        void AnimationController(Vector2 _input)
        {
            if (!view.IsMine)
            {
                return;
            }
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

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundChecker.position, groundCheckDistance);
        }
    }
}

