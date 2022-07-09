using System;
using UnityEngine;

namespace TicTacToe
{
    public class HumanPlayer : TicTacToePlayer
    {
        [SerializeField] private Component _iGameInputController;

        private IGameInputController _gameInputController;

        private bool _isMyMove = false;

        protected override void OnEnable()
        {
            base.OnEnable();

            if (_iGameInputController == null)
            {
                Debug.LogError($"{nameof(_iGameInputController)} field should be initialized in inspector");
                return;
            }
            _gameInputController.CellPressed += OnCellPressed;
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            if (_iGameInputController == null)
            {
                throw new Exception($"{nameof(_iGameInputController)} field should be initialized in inspector");
                return;
            }
            _gameInputController.CellPressed -= OnCellPressed;
        }

        protected override void OnCurrentPlayerChanged(TicTacToeGame game, TicTacToePlayer player)
            => _isMyMove = player == this;

        private void OnCellPressed(Vector2Int positionInMatrix)
        {
            if (_isMyMove)
            {
                if (PlayableGame == null)
                    throw new Exception($"The {name} is not connected to the game!");

                PlayableGame.MakeMove(this, positionInMatrix);
            }
        }

        private void OnValidate()
        {
            _gameInputController = _iGameInputController as IGameInputController;
            if (_gameInputController == null)
            {
                _iGameInputController = null;
                throw new InvalidCastException($"The component in field " +
                    $"{nameof(_iGameInputController)} must implement " +
                    $"\"{nameof(IGameInputController)}\" interface");               
            }

        }
    }
}
