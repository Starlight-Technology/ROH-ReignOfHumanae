using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerMovements : MonoBehaviour
    {
        public float moveSpeed = 5f;
        public float jumpForce = 5f;
        public CharacterController controller;
        public Transform cameraTransform; // Drag the Camera transform here in the Inspector
        public Transform playerModel;    // Assign the player's visual model here in the Inspector

        private Vector3 moveDirection;
        private float yVelocity = 0f;
        private bool isGrounded;

        private void Start()
        {
            //controller.skinWidth = 0.5f; // Ajusta a espessura da pele do controlador de personagem
        }

        void Update()
        {
            Move();
        }

        void Move()
        {
            isGrounded = controller.isGrounded;

            if (isGrounded && yVelocity < 0)
            {
                yVelocity = -10f; // Evita flutuação. Um valor levemente negativo mantém o personagem no chão.
            }

            float moveX = Input.GetAxis("Horizontal");
            float moveZ = Input.GetAxis("Vertical");

            // Direção relativa à câmera (sem inclinação vertical)
            Vector3 cameraForward = Vector3.Scale(cameraTransform.forward, new Vector3(1, 0, 1)).normalized;
            Vector3 cameraRight = cameraTransform.right;

            Vector3 horizontalMove = (cameraForward * moveZ + cameraRight * moveX).normalized;

            if (horizontalMove.magnitude >= 0.1f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(horizontalMove, Vector3.up);
                playerModel.rotation = Quaternion.Lerp(playerModel.rotation, targetRotation, Time.deltaTime * 10f);
            }

            // Pulo
            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                yVelocity = jumpForce;
            }

            // Gravidade
            yVelocity += Physics.gravity.y * Time.deltaTime;

            // Movimento final
            Vector3 finalMove = horizontalMove * moveSpeed + Vector3.up * yVelocity;
            controller.Move(finalMove * Time.deltaTime);
        }

    }
}
