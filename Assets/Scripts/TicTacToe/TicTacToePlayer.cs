using System;
using UnityEngine;
using UnityEngine.Events;

namespace TicTacToe
{
    public abstract class TicTacToePlayer : MonoBehaviour
    {
        [SerializeField] private Cell _cellType = Cell.Empty;
        [SerializeField] private TicTacToeGame _currentGame = null;

        public TicTacToeGame PlayableGame { get => _currentGame; }

        public Cell CellType 
        {
            get 
            {
                if (_cellType == Cell.Empty)
                    throw new Exception($"{nameof(_cellType)} should not be equal {Cell.Empty}. Initialize the player before use.");

                return _cellType;
            }
        }  

        public void Initialize(Cell cell) 
        {
            _cellType = cell;
        }

        public void JoinTheGame(TicTacToeGame ticTacToeGame)
        {
            if (_currentGame != null)
                throw new Exception("This player is already in the game. To enter a new game, you need to exit the current one.");

            _currentGame = ticTacToeGame;
            _currentGame.CurrentPlayerChanged += OnCurrentPlayerChanged;
            _currentGame.GameEnded += OnGameEnded;
        }

        public void LeaveTheGame()
        {
            if (_currentGame == null)
                throw new Exception("This player is not attached to any game.");

            _currentGame.CurrentPlayerChanged -= OnCurrentPlayerChanged;
            _currentGame.GameEnded -= OnGameEnded;
            _currentGame = null;
        }

        protected abstract void OnCurrentPlayerChanged(TicTacToeGame game, TicTacToePlayer player);

        protected virtual void OnEnable()
        {
            if (_currentGame == null)
                return;

            _currentGame.CurrentPlayerChanged += OnCurrentPlayerChanged;
            _currentGame.GameEnded += OnGameEnded;
        }

        protected virtual void OnDisable()
        {
            if (_currentGame == null)
                return;

            _currentGame.CurrentPlayerChanged -= OnCurrentPlayerChanged;
            _currentGame.GameEnded -= OnGameEnded;
        }

        private void OnGameEnded(TicTacToeGame game, TicTacToePlayer player)
        {
            LeaveTheGame();
        }
    }
}
