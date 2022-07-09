using System;
using UnityEngine;

namespace TicTacToe
{
    [Serializable]
    public class CellResource<T>
    {
        [SerializeField] private Cell _type;
        [SerializeField] private T _data;

        public CellResource(Cell cellType, T figurePrefab)
        {
            _type = cellType;
            _data = figurePrefab;
        }

        public T Data { get => _data; }
        public Cell Type { get => _type; }
    }
}
