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

        public enum MovementMode { Ground, Swimming, Flying }
        public MovementMode currentMode = MovementMode.Ground;

        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponentInChildren<Animator>();
            if (mainCamera == null)
                mainCamera = Camera.main;

            agent.updatePosition = false;
            agent.updateRotation = false;
            agent.isStopped = true;
        }

        void Update()
        {
            float moveX = Input.GetAxis("Horizontal");
            float moveZ = Input.GetAxis("Vertical");
            bool jumpPressed = Input.GetButton("Jump");

            usingManualInput = Mathf.Abs(moveX) > 0.1f || Mathf.Abs(moveZ) > 0.1f || jumpPressed;

            isGrounded = controller.isGrounded;

            // Alternar modo de voo (tecla V)
            if (Input.GetKeyDown(KeyCode.V))
            {
                isFlying = !isFlying;
                currentMode = isFlying ? MovementMode.Flying : (inWater ? MovementMode.Swimming : MovementMode.Ground);
                animator.SetBool("IsFlying", isFlying);
            }

            // Alternar animações com base no modo
            animator.SetBool("IsSwimming", currentMode == MovementMode.Swimming);
            animator.SetBool("IsFlying", currentMode == MovementMode.Flying);

            // Se estiver nadando
            if (currentMode == MovementMode.Swimming)
            {
                if (jumpPressed)
                    yVelocity = 2f;
                else if (Input.GetKey(KeyCode.LeftControl))
                    yVelocity = -2f;
                else
                    yVelocity = 0f;

                animator.SetFloat("SwimVerticalSpeed", math.abs(yVelocity));
            }

            // Se estiver voando
            if (currentMode == MovementMode.Flying)
            {
                float flyY = 0f;
                if (Input.GetKey(KeyCode.E)) flyY = 1f;
                else if (Input.GetKey(KeyCode.Q)) flyY = -1f;

                Vector3 flyDir = new Vector3(moveX, flyY, moveZ);
                MoveCharacter(flyDir, flySpeed);
                animator.SetFloat("Speed", flyDir.magnitude);
                return;
            }

            // Se input manual
            if (usingManualInput)
            {
                MoveCharacter(new Vector3(moveX, 0, moveZ), Input.GetKey(KeyCode.LeftShift) ? runSpeed : moveSpeed);

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
                animator.SetBool("IsJumping", true);
            }

            // Aplica gravidade
            if (!isFlying && !inWater)
                yVelocity += Physics.gravity.y * Time.deltaTime;

            Vector3 gravityMove = Vector3.up * yVelocity;
            controller.Move(gravityMove * Time.deltaTime);

            if (isGrounded && yVelocity < 0.1f)
                animator.SetBool("IsJumping", false);

            // Atualiza velocidade para animação
            float speed = new Vector2(moveX, moveZ).magnitude;
            animator.SetFloat("Speed", speed < 0.1 ? 0 : Input.GetKey(KeyCode.LeftShift) ? runSpeed : moveSpeed);

            if (agent.enabled)
                CheckAgentRecovery();
        }

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
    }
}
