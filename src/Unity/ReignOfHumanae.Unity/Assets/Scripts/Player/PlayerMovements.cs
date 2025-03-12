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

        void Update()
        {
            Move();
        }

        void Move()
        {
            isGrounded = controller.isGrounded;
            if (isGrounded && yVelocity < 0)
            {
                yVelocity = 0f;
            }

            float moveX = Input.GetAxis("Horizontal");
            float moveZ = Input.GetAxis("Vertical");

            // Calculate movement direction relative to the camera
            Vector3 cameraForward = Vector3.Scale(cameraTransform.forward, new Vector3(1, 0, 1)).normalized; // Flattened forward
            Vector3 cameraRight = cameraTransform.right;

            moveDirection = cameraForward * moveZ + cameraRight * moveX;

            if (moveDirection.magnitude >= 0.1f)
            {
                // Rotate the player's model to face the direction of movement
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
                playerModel.rotation = Quaternion.Lerp(playerModel.rotation, targetRotation, Time.deltaTime * 10f); // Smooth rotation
            }

            // Move the player
            controller.Move(moveSpeed * Time.deltaTime * moveDirection);

            // Handle jump logic
            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                yVelocity = jumpForce;
            }

            // Gravity
            yVelocity += Physics.gravity.y * Time.deltaTime;
            controller.Move(new Vector3(0, yVelocity, 0) * Time.deltaTime);
        }
    }
}
