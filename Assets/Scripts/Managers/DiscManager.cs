using UnityEngine;

namespace Managers
{
    public class DiscManager : MonoBehaviour
    {
        [SerializeField] private DiscPooler discPooler;
        [SerializeField] private Board board;

        public void DropDisc(Slot closest)
        {
            if (closest.isFull) return;

            var spawnPosition = new Vector3(closest.transform.position.x, board.GetSpawnHeight(), 0);

            var disc = discPooler.GetDisc();
            board.DropDisc(closest.column, disc);
            disc.Drop(GameManager.Instance.GetCurrentPlayer());
            GameManager.Instance.SwitchPlayer();
            disc.transform.position = spawnPosition;
        }
    }
}