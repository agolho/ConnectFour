using Constants;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI playerText;
        [SerializeField] private TextMeshProUGUI redWinsText, yellowWinsText;
        [SerializeField] private Image topBar;
    
        private void Start()
        {
            InitializePlayerText();
        }

        void InitializePlayerText()
        {
            playerText.text = "Red Player's Turn";
        }
    
        public void SwitchPlayerText(CurrentPlayer currentPlayer)
        {
            playerText.text = currentPlayer == CurrentPlayer.Player1 ? "Red Player's Turn" : "Yellow Player's Turn";
        }

        public void EndGame(CurrentPlayer player)
        {
            playerText.text = player == CurrentPlayer.Player1 ? "Red Player Wins!" : "Yellow Player Wins!";
            topBar.color = player == CurrentPlayer.Player1 ? Color.red : Color.yellow;
        }

        public void UpdateWinCounts(int redWins, int yellowWins)
        {
            redWinsText.text = "Red Win Count: "+ redWins;
            yellowWinsText.text = "Yellow Win Count: "+ yellowWins;
        }
    }
}
