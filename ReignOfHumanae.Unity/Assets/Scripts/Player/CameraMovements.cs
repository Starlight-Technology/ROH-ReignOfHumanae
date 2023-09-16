using UnityEngine;

namespace Assets.Scripts.Player
{
    public class CameraMovements : MonoBehaviour
    {
        private const float YMin = -50.0f;
        private const float YMax = 50.0f;
        private float currentX = 0.0f;
        private float currentY = 0.0f;

        public Transform LookAt { get; set; }
        public Transform Player { get; set; }
        public float Distance { get; set; } = 10.0f;
        public float Sensitivity { get; set; } = 4.0f;

        // Update is called once per frame
        private void Update()
        {
            if (Input.GetKey(KeyCode.Mouse1))
            {
                MoveCamera();
            }

            Vector3 Direction = new(0, 0, Distance);
            Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
            transform.position = LookAt.position + (rotation * Direction);
            transform.LookAt(LookAt.position);
        }

        private void MoveCamera()
        {
            currentX += Input.GetAxis("Mouse X") * Sensitivity * Time.deltaTime;
            currentY += Input.GetAxis("Mouse Y") * Sensitivity * Time.deltaTime;
            currentY = Mathf.Clamp(currentY, YMin, YMax);
        }
    }
}