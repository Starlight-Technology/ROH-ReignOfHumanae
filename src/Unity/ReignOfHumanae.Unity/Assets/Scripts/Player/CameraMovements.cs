using UnityEngine;

namespace Assets.Scripts.Player
{
    public class CameraMovements : MonoBehaviour
    {
        public Vector3 offset = new Vector3(0, 2f, 0);
        public float sensitivity = 100f;
        public float zoomSpeed = 4f;
        public float minZoom = 2f;
        public float maxZoom = 15f;
        public float pitchMin = -20f;
        public float pitchMax = 60f;

        public Transform player;
        public LayerMask collisionMask;

        private float currentZoom = 10f;
        private float pitch = 20f;
        private float yaw = 0f;

        void LateUpdate()
        {
            if (player == null) return;

            // Zoom
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            currentZoom -= scroll * zoomSpeed;
            currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);

            // Rotation input
            if (Input.GetMouseButton(1))
            {
                yaw += Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
                pitch -= Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;
                pitch = Mathf.Clamp(pitch, pitchMin, pitchMax);
            }

            // Calculate desired camera direction and position
            Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
            Vector3 desiredCameraDir = rotation * Vector3.back;
            Vector3 targetPosition = player.position + offset;
            Vector3 desiredPosition = targetPosition + desiredCameraDir * currentZoom;

            // Collision check (raycast from target to camera)
            RaycastHit hit;
            float adjustedZoom = currentZoom;
            if (Physics.Raycast(targetPosition, desiredCameraDir, out hit, currentZoom, collisionMask))
            {
                adjustedZoom = hit.distance - 0.1f;
                adjustedZoom = Mathf.Clamp(adjustedZoom, minZoom, maxZoom);
                desiredPosition = targetPosition + desiredCameraDir * adjustedZoom;
            }

            transform.position = desiredPosition;
            transform.LookAt(targetPosition);
        }
    }
}
