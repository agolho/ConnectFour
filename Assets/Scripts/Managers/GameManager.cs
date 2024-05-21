using Constants;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        [SerializeField] private UIManager uiManager;
        private CurrentPlayer _currentPlayer;
        private bool _gameOver;
    
        private int _redWins, _yellowWins;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            InitializeFirstPlayer();
            Load();
        }

        private void InitializeFirstPlayer()
        {
            _currentPlayer = CurrentPlayer.Player1;
        }

        public void SwitchPlayer()
        {
            if(_gameOver) return;
            _currentPlayer = _currentPlayer == CurrentPlayer.Player1 ? CurrentPlayer.Player2 : CurrentPlayer.Player1;
            uiManager.SwitchPlayerText(_currentPlayer);
        }
    
        public CurrentPlayer GetCurrentPlayer()
        {
            return _currentPlayer;
        }
    
        public void SetCurrentPlayer(CurrentPlayer player)
        {
            _currentPlayer = player;
        }

        public bool IsGameOver()
        {
            return _gameOver;
        }
    
        public void EndGame(CurrentPlayer player)
        {
            _gameOver = true;
            Board.Instance.EndGame();
            uiManager.EndGame(player);
        
            IncrementWinCount(player);
        }

        private void IncrementWinCount(CurrentPlayer player)
        {
            if(player == CurrentPlayer.Player1) _redWins++;
            else _yellowWins++;
        
            Save();
            Invoke(nameof(RestartLevel),3);
        }

        void Save()
        {
            PlayerPrefs.SetInt("RedWins", _redWins);
            PlayerPrefs.SetInt("YellowWins", _yellowWins);
            uiManager.UpdateWinCounts(_redWins, _yellowWins);
        }

        void Load()
        {
            _redWins = PlayerPrefs.GetInt("RedWins");
            _yellowWins = PlayerPrefs.GetInt("YellowWins");
            uiManager.UpdateWinCounts(_redWins, _yellowWins);
        }

        public void RestartLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
