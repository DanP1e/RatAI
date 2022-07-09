using System;
using TMPro;
using UnityEngine;

namespace TicTacToe
{
    public class GameStatePresenter : MonoBehaviour
    {
        [SerializeField] private TicTacToeGame _ticTacToeGame;
        [SerializeField] private TextCellAssociations _consoleCellsDataBase;
        [SerializeField] private TextMeshPro _textMesh;
        [SerializeField] private string _showText = "The winner is {symbol} - {player}!";

        private void Start()
        {
            _textMesh.text = "";
            _ticTacToeGame.GameEnded += OnTicTacToeGameEnded;
        }

        private void OnDestroy()
        {
            if(_ticTacToeGame != null)
                _ticTacToeGame.GameEnded -= OnTicTacToeGameEnded;
        }

        private void OnTicTacToeGameEnded(TicTacToeGame game, TicTacToePlayer player)
        {
            if (player != null)
            {
                var result = _showText.Replace(
                    "{symbol}",
                    _consoleCellsDataBase.GetResource(player.CellType).Data);

                result = result.Replace("{player}", player.name);

                _textMesh.text = result;
            }
            else
            {
                _textMesh.text = "The game ended the draw!";
            }
        }
    }
}
