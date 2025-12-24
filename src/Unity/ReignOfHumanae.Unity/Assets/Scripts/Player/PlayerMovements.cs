using Assets.Scripts.Models.Character;

using Unity.Mathematics;

using UnityEngine;
using UnityEngine.AI;
using UnityEngine.ProBuilder;

namespace Assets.Scripts.Player
{
    public class PlayerMovements : MonoBehaviour
    {
        public float moveSpeed = 5f;
        public float runSpeed = 10f;
        public float jumpForce = 5f;
        public float flySpeed = 7f;
        public CharacterController controller;
        public Transform cameraTransform;
        public Transform playerModel;
        public Camera mainCamera;

        private Animator animator;
        private NavMeshAgent agent;
        private float yVelocity = 0f;
        private bool isGrounded;
        private bool inWater;
        private bool isFlying;
        private bool usingManualInput;

        public PlayerAnimationState currentAnimState;
        private PlayerAnimationState lastAnimState;

        public enum MovementMode { Ground, Swimming, Flying }
        public MovementMode currentMode = MovementMode.Ground;

        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponentInChildren<Animator>();
            if (mainCamera == null)
                mainCamera = Camera.main;

            if (agent != null && agent.isOnNavMesh)
            {
                agent.updatePosition = false;
                agent.updateRotation = false;
                agent.isStopped = true;
            }

        }

        void Update()
        {
            float speed = 0;
            float moveX = Input.GetAxis("Horizontal");
            float moveZ = Input.GetAxis("Vertical");
            bool jumpPressed = Input.GetButton("Jump");

            usingManualInput = Mathf.Abs(moveX) > 0.1f || Mathf.Abs(moveZ) > 0.1f || jumpPressed;

            isGrounded = controller.isGrounded;

            // Se estiver nadando
            if (currentMode == MovementMode.Swimming)
            {
                if (jumpPressed)
                    yVelocity = 2f;
                else if (Input.GetKey(KeyCode.LeftControl))
                    yVelocity = -2f;
                else
                    yVelocity = 0f;
            }

            // Se estiver voando
            if (currentMode == MovementMode.Flying)
            {
                float flyY = 0f;
                if (Input.GetKey(KeyCode.E)) flyY = 5f;
                else if (Input.GetKey(KeyCode.Q)) flyY = -5f;

                Vector3 flyDir = new Vector3(moveX, flyY, moveZ);
                MoveCharacter(flyDir, flySpeed);
                return;
            }

            if (currentMode == MovementMode.Ground
                && isGrounded
                && !usingManualInput
                && agent.isOnNavMesh)
                agent.enabled = true;

            if (Input.GetMouseButtonDown(0) && agent.isOnNavMesh)
            {
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    agent.SetDestination(hit.point);
                }
                agent.isStopped = false;
            }


            // Se input manual
            if (usingManualInput)
            {
                MoveCharacter(new Vector3(moveX, 0, moveZ), WalkingOrRun(moveSpeed, runSpeed));

                if (agent.enabled && isGrounded)
                {
                    agent.isStopped = true;
                    agent.nextPosition = transform.position;
                }
            }


            // Movimento automático via agent
            else if (agent.enabled && !agent.isStopped && agent.hasPath)
            {
                Vector3 velocity = agent.desiredVelocity;
                velocity.y = yVelocity;
                controller.Move(velocity * Time.deltaTime);
                RotateTowards(agent.desiredVelocity);
                agent.nextPosition = transform.position;
            }

            // Pular no modo terrestre
            if (jumpPressed && isGrounded && currentMode == MovementMode.Ground)
            {
                yVelocity = jumpForce;
            }

            // Aplica gravidade
            if (!isFlying && !inWater)
                yVelocity += Physics.gravity.y * Time.deltaTime;

            Vector3 gravityMove = Vector3.up * yVelocity;
            controller.Move(gravityMove * Time.deltaTime);


            if (agent.enabled)
                CheckAgentRecovery();

            bool hasInput = usingManualInput;
            bool isRunning = Input.GetKey(KeyCode.LeftShift);
            bool isJumping = !isGrounded && currentMode == MovementMode.Ground;

            UpdateAnimationState(
                hasInput,
                isRunning,
                isJumping,
                isGrounded
            );
        }

        private static float WalkingOrRun(float walking = 0.5f, float running = 1f) => Input.GetKey(KeyCode.LeftShift) ? running : walking;

        void MoveCharacter(Vector3 inputDir, float speed)
        {
            Vector3 cameraForward = Vector3.Scale(cameraTransform.forward, new Vector3(1, 0, 1)).normalized;
            Vector3 cameraRight = cameraTransform.right;
            Vector3 moveDir = (cameraForward * inputDir.z + cameraRight * inputDir.x).normalized;
            float vertical = inputDir.y;

            if (moveDir.sqrMagnitude > 0.01f)
                RotateTowards(moveDir);

            Vector3 finalMove = (moveDir * speed + Vector3.up * vertical * speed + Vector3.up * yVelocity);
            controller.Move(finalMove * Time.deltaTime);

        }

        void RotateTowards(Vector3 direction)
        {
            direction.y = 0;
            if (direction.sqrMagnitude > 0.001f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                playerModel.rotation = Quaternion.Lerp(playerModel.rotation, targetRotation, Time.deltaTime * 10f);
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Water") && currentMode != MovementMode.Swimming)
            {
                inWater = true;
                currentMode = MovementMode.Swimming;
                agent.enabled = false;
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Water") && currentMode == MovementMode.Swimming)
            {
                inWater = false;
                currentMode = isFlying ? MovementMode.Flying : MovementMode.Ground;
                if (agent.isOnNavMesh)
                    agent.enabled = true;
            }
        }

        void CheckAgentRecovery()
        {
            if (!NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
                return;

            if (!agent.isOnNavMesh)
            {
                agent.Warp(hit.position);
                agent.nextPosition = transform.position;
            }
        }


        void UpdateAnimationState(
        bool hasInput,
        bool isRunning,
        bool isJumping,
        bool isGrounded
    )
        {
            lastAnimState = currentAnimState;

            switch (currentMode)
            {
                case MovementMode.Ground:
                    if (isJumping)
                        currentAnimState = PlayerAnimationState.GroundJump;
                    else if (!hasInput)
                        currentAnimState = PlayerAnimationState.GroundIdle;
                    else if (isRunning)
                        currentAnimState = PlayerAnimationState.GroundRun;
                    else
                        currentAnimState = PlayerAnimationState.GroundWalk;
                    break;

                case MovementMode.Swimming:
                    currentAnimState = hasInput
                        ? PlayerAnimationState.SwimmingMove
                        : PlayerAnimationState.SwimmingIdle;
                    break;

                case MovementMode.Flying:
                    currentAnimState = hasInput
                        ? PlayerAnimationState.FlyingMove
                        : PlayerAnimationState.FlyingIdle;
                    break;
            }

            if (currentAnimState != lastAnimState)
                ApplyAnimatorState(currentAnimState);
        }

        void ApplyAnimatorState(PlayerAnimationState state)
        {
            animator.SetBool("IsJumping", state == PlayerAnimationState.GroundJump);

            animator.SetBool("IsSwimming",
                state == PlayerAnimationState.SwimmingIdle ||
                state == PlayerAnimationState.SwimmingMove);

            animator.SetBool("IsFlying",
                state == PlayerAnimationState.FlyingIdle ||
                state == PlayerAnimationState.FlyingMove);

            float speed = state switch
            {
                PlayerAnimationState.GroundIdle => 0,
                PlayerAnimationState.GroundWalk => 0.5f,
                PlayerAnimationState.GroundRun => 1f,
                PlayerAnimationState.SwimmingIdle => 0,
                PlayerAnimationState.SwimmingMove => 0.6f,
                PlayerAnimationState.FlyingIdle => 0,
                PlayerAnimationState.FlyingMove => 0.8f,
                _ => 0
            };

            animator.SetFloat("Speed", speed);
        }


    }
}
