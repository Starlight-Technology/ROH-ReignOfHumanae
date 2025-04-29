using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Player
{
    public class ClickToMove : MonoBehaviour
    {
        public Camera mainCamera;
        private NavMeshAgent agent;

        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            if (mainCamera == null)
                mainCamera = Camera.main;
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    agent.SetDestination(hit.point);
                }
            }
        }
    }
}