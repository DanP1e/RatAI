using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Events;

namespace TicTacToe
{
    public class TicTacToeGame : MonoBehaviour
    {
        [SerializeField] private Field _field;
        [SerializeField] private int _winComboLength = 3;
        [SerializeField] private List<TicTacToePlayer> _players = new List<TicTacToePlayer>();

        private ReadOnlyCollection<TicTacToePlayer> _playersReadOnlyCollection;
        private int _currentPlayerId = 0;
        private TicTacToePlayer _winner = null;

        public event UnityAction<TicTacToePlayer, Vector2Int> MoveMade;
        public event UnityAction<TicTacToeGame, TicTacToePlayer> CurrentPlayerChanged;
        public event UnityAction<TicTacToeGame, TicTacToePlayer> GameEnded;

        public ReadOnlyCollection<TicTacToePlayer> Players => _playersReadOnlyCollection;
        public TicTacToePlayer CurrentPlayer { get => _players[_currentPlayerId]; }
        public Vector2Int FieldSize { get => _field.Size; }

        private void Awake()
        {
            if (_players.Count == 0)
                Debug.LogError($"You passed in {nameof(_players)} field empty collection, there must be at least one element!");
            _playersReadOnlyCollection = new ReadOnlyCollection<TicTacToePlayer>(_players);
            _field.RestField();
            SetCurrentPlayer(0);
        }

        public void StartNewGame(List<TicTacToePlayer> playersList, Vector2Int fieldSize)
        {
            if (playersList == null)
                throw new ArgumentNullException();
            else if (playersList.Count == 0)
                throw new ArgumentException($"You passed in an empty collection, it doesn't make sense!");

            _field.Initialize(fieldSize);
            _field.RestField();
            _players = playersList;
            SetCurrentPlayer(0);
        }        

        public Cell GetFieldCell(int x, int y) => _field[x, y];

        public bool MakeMove(TicTacToePlayer player, Vector2Int movePosition) 
        {
            if (player != CurrentPlayer)
                return false;
            if (!_field.TrySetCell(movePosition, player.CellType))
                return false;
            
            MoveMade?.Invoke(player, movePosition);

            if (TryFindWinner(movePosition) || _field.IsFilled())
                GameEnded?.Invoke(this, _winner);

            SetCurrentPlayer(_currentPlayerId + 1);

            return true;
        }

        protected virtual bool TryFindWinner(Vector2Int cellPoint)
        {         
            if (IsHorizontalCombo(cellPoint)
                || IsVerticalCombo(cellPoint)
                || IsDiagonalCombo(cellPoint))
            {
                _winner = CurrentPlayer;
                return true;
            }
                                       
            return false;
        }

        private void SetCurrentPlayer(int playerId)
        {
            if (playerId >= _players.Count)
                _currentPlayerId = 0;
            else
                _currentPlayerId = playerId;

            CurrentPlayerChanged?.Invoke(this, CurrentPlayer);
        }

        private bool IsVerticalCombo(Vector2Int cellPoint)
        {
            Cell currentCell = _field[cellPoint.x, cellPoint.y];

            int combo = -1;

            for (int y = cellPoint.y; y >= 0; y--)
            {
                if (_field[cellPoint.x, y] != currentCell)
                    break;
                combo++;
            }
            for (int y = cellPoint.y; y < _field.Size.y; y++)
            {
                if (_field[cellPoint.x, y] != currentCell)
                    break;
                combo++;
            }

            return combo >= _winComboLength;
        }

        private bool IsHorizontalCombo(Vector2Int cellPoint) 
        {
            Cell currentCell = _field[cellPoint.x, cellPoint.y];

            int combo = -1;

            for (int x = cellPoint.x; x >= 0; x--)
            {
                if (_field[x, cellPoint.y] != currentCell)
                    break;
                combo++;
            }
            for (int x = cellPoint.x; x < _field.Size.x; x++)
            {
                if (_field[x, cellPoint.y] != currentCell)
                    break;
                combo++;
            }
            
            return combo >= _winComboLength;
        }

        private bool IsDiagonalCombo(Vector2Int cellPoint) 
        {
            Cell currentCell = _field[cellPoint.x, cellPoint.y];

            int combo = -1;

            for (int x = cellPoint.x, y = cellPoint.y; y >= 0 && x >= 0; y--, x--)
            {
                if (_field[x, y] != currentCell)
                    break;
                combo++;            
            }
            for (int x = cellPoint.x, y = cellPoint.y; y < _field.Size.y && x < _field.Size.x; y++, x++)
            {
                if (_field[x, y] != currentCell)
                    break;
                combo++;
            }

            return combo >= _winComboLength;
        }
    }
}
