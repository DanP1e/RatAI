using System;
using UnityEngine;

namespace TicTacToe
{
    public abstract class Cell3D : MatrixElement
    {
        [SerializeField] private Cell _data;

        public Cell Data { get => _data; }

        public void SetData(Cell data) 
        {
            _data = data;
            OnDataSet(data);
        }

        protected abstract void OnDataSet(Cell data);
    }
}
