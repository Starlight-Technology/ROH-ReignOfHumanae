using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Player
{
    public class PlayerMovements : MonoBehaviour
    {
        public float moveSpeed = 5f;
        public float jumpForce = 5f;
        public CharacterController controller;
        public Transform cameraTransform;
        public Transform playerModel;
        public Camera mainCamera;

        private NavMeshAgent agent;
        private float yVelocity = 0f;
        private bool isGrounded;

        private bool usingManualInput = false;

        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            if (mainCamera == null)
                mainCamera = Camera.main;

            // Impede que o NavMeshAgent mova ou rotacione o personagem
            agent.updatePosition = false;
            agent.updateRotation = false;
            agent.isStopped = true;
        }

        void Update()
        {
            // Verifica se há entrada manual
            float moveX = Input.GetAxis("Horizontal");
            float moveZ = Input.GetAxis("Vertical");
            bool jumpPressed = Input.GetButtonDown("Jump");

            usingManualInput = Mathf.Abs(moveX) > 0.1f || Mathf.Abs(moveZ) > 0.1f || jumpPressed;

            if (Input.GetMouseButtonDown(0))
            {

                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    agent.isStopped = false;
                    agent.SetDestination(hit.point);
                }
            }

            if (usingManualInput)
            {
                MoveManually(moveX, moveZ, jumpPressed);
                agent.isStopped = true;

                // Sincroniza o agente com o personagem
                agent.nextPosition = transform.position;
                agent.transform.position = transform.position;
            }
            // sincroniza o personagem com o agente
            if (!agent.isStopped && agent.hasPath && agent.remainingDistance > agent.stoppingDistance)
            {
                Vector3 velocity = agent.desiredVelocity;
                velocity.y = yVelocity;
                controller.Move(velocity * Time.deltaTime);
                RotateTowards(agent.desiredVelocity);
                agent.nextPosition = transform.position; // mantém sincronizado após o movimento
                agent.transform.position = transform.position;
                agent.transform.rotation = transform.rotation;
            }

        }

        private void LateUpdate()
        {
            yVelocity += Physics.gravity.y * Time.deltaTime;
            CheckAgentRecovery();
        }

        void MoveManually(float moveX, float moveZ, bool jumpPressed)
        {
            isGrounded = controller.isGrounded;

            if (isGrounded && yVelocity < 0)
                yVelocity = -10f;

            Vector3 cameraForward = Vector3.Scale(cameraTransform.forward, new Vector3(1, 0, 1)).normalized;
            Vector3 cameraRight = cameraTransform.right;

            Vector3 moveDir = (cameraForward * moveZ + cameraRight * moveX).normalized;

            if (moveDir.sqrMagnitude > 0.01f)
            {
                RotateTowards(moveDir);
            }

            if (jumpPressed && isGrounded)
            {
                yVelocity = jumpForce;
            }



            Vector3 finalMove = moveDir * moveSpeed + Vector3.up * yVelocity;
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

        void CheckAgentRecovery()
        {
            if (!NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
            {
                // Fora do NavMesh — não há nada a fazer por enquanto
                return;
            }

            if (!agent.isOnNavMesh)
            {
                // Se voltou a uma área válida, 'warp' o agente para lá
                agent.Warp(hit.position);
                agent.nextPosition = transform.position;
                agent.transform.position = transform.position;
            }
        }

    }
}
