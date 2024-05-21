using UnityEngine;

namespace Managers
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private DiscManager discManager;
        [SerializeField] private Board board;
        [SerializeField] private Camera mainCamera;
        [SerializeField] private float targetDepth = 0.0f;

        void Update()
        {
            if (GameManager.Instance.IsGameOver()) return;

            var worldPosition = GetMouseWorldPosition(targetDepth);
            var closestSlot = FindClosestSlot(worldPosition);

            if (Input.GetMouseButtonDown(0) && closestSlot != null)
            {
                discManager.DropDisc(closestSlot);
            }

            board.ShowSpawnGuide(closestSlot?.column ?? -1);
        }

        private Slot FindClosestSlot(Vector3 worldPosition)
        {
            Slot closestSlot = null;
            var minDistance = float.MaxValue;

            foreach (var slot in board.GetSlots())
            {
                var distance = Vector3.Distance(worldPosition, slot.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestSlot = slot;
                }
            }

            return closestSlot;
        }

        private Vector3 GetMouseWorldPosition(float depth)
        {
            var mouseScreenPosition = Input.mousePosition;
            var ray = mainCamera.ScreenPointToRay(mouseScreenPosition);
            var distanceToDepth = (depth - mainCamera.transform.position.z) / ray.direction.z;
            return ray.GetPoint(distanceToDepth);
        }
    }
}