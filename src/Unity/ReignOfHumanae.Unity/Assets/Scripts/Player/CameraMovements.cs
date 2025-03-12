using UnityEngine;

namespace Assets.Scripts.Player
{
    public class CameraMovements : MonoBehaviour
    {
        public Vector3 offset;
        public float sensitivity = 100f;
        public float zoomSpeed = 4f;
        public float minZoom = -15f;
        public float maxZoom = 15f;
        public float pitchMin = -20f;
        public float pitchMax = 60f;

        public Transform player;
        private float currentZoom = 10f;
        private float initialMouseX;
        private float initialMouseY;
        private float pitch = 0f;
        private bool isRotating = false;

        void Start()
        {
            if (player == null)
            {
                Debug.LogError("Player not found! Make sure the camera is a child of the player.");
            }
        }

        void Update()
        {
            Zoom();
            Rotate();
        }

        void LateUpdate()
        {
            if (player != null)
            {
                // Calculate desired position based on offset and zoom
                Vector3 desiredPosition = player.position - transform.forward * currentZoom + Vector3.up * offset.y;
                transform.position = desiredPosition;

                transform.LookAt(player.position + Vector3.up * offset.y);
            }
        }

        void Zoom()
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            currentZoom -= scroll * zoomSpeed;
            currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);
        }

        void Rotate()
        {
            if (Input.GetMouseButtonDown(1)) // Right mouse button pressed
            {
                initialMouseX = Input.mousePosition.x;
                initialMouseY = Input.mousePosition.y;
                isRotating = true;
            }

            if (Input.GetMouseButton(1) && isRotating) // Right mouse button held down
            {
                float currentMouseX = Input.mousePosition.x;
                float currentMouseY = Input.mousePosition.y;
                float xDelta = currentMouseX - initialMouseX;
                float yDelta = initialMouseY - currentMouseY; // Invert Y for natural pitch control

                float rotationAmountX = xDelta * sensitivity * Time.deltaTime;
                float rotationAmountY = yDelta * sensitivity * Time.deltaTime;

                // Rotate around the player's local Y axis (yaw)
                transform.RotateAround(player.position, player.up, rotationAmountX);

                // Rotate around the player's local X axis (pitch)
                transform.RotateAround(player.position, transform.right, rotationAmountY);

                pitch = Mathf.Clamp(pitch - yDelta * sensitivity * Time.deltaTime, pitchMin, pitchMax); // Clamp the pitch angle

                initialMouseX = currentMouseX; // Update initialMouseX to allow continuous rotation
                initialMouseY = currentMouseY; // Update initialMouseY to allow continuous rotation
            }

            if (Input.GetMouseButtonUp(1)) // Right mouse button released
            {
                isRotating = false;
            }
        }
    }
}
