using System;
using UnityEngine;

namespace TicTacToe
{
    public class Cell3DWithText : Cell3D
    {
        [SerializeField] private TextCellAssociations _textCellsAssociations;

        protected override void OnDataSet(Cell data)
        {
            throw new NotImplementedException();
        }
    }
}
