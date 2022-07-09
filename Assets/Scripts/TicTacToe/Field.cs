using System;
using System.Collections;
using UnityEngine;

namespace TicTacToe
{
    [Serializable]
    public class Field : IEnumerable
    {
        [SerializeField] private Vector2Int _size = new Vector2Int(3, 3);

        private Cell[,] _field;
        private int _fieldElementsCount = 0;
        private int _filledElements = 0;
        private bool _isInitialized = false;

        public int FieldElementsCount { get => _fieldElementsCount; }
        public Vector2Int Size { get => _size; }

        public Field(Vector2Int fieldSize)
        {
            Initialize(fieldSize);
        }
        public void Initialize(Vector2Int fieldSize) 
        {           
            _size = fieldSize;
            _filledElements = 0;
            _fieldElementsCount = _size.x * _size.y;
            RestField();
            _isInitialized = true;
        }
        public Cell this[int x, int y]
        {
            get 
            {
                if (!_isInitialized)
                    Initialize(_size);

               return _field[x, y];
            }
        }
        public virtual void RestField() 
        {
            _field = new Cell[_size.x, _size.y];
        }
        public virtual bool TrySetCell(Vector2Int pointer, Cell cellType)
        {
            if (cellType == Cell.Empty)
                throw new ArgumentException($"{nameof(cellType)} should not be equal {cellType}. " +
                    $"Use the {nameof(RestField)} function to clear the field.");

            if (!IsPointInField(pointer))
                return false;

            if (_field[pointer.x, pointer.y] == Cell.Empty)
            {
                _field[pointer.x, pointer.y] = cellType;
                _filledElements++;
                return true;
            }

            return false;
        }
        public bool IsFilled()
        {
            if (_fieldElementsCount == _filledElements)
                return true;
            else
                return false;
        }
        public bool IsPointInField(Vector2Int point) 
        {
            if (point.x >= _size.x
                || point.x < 0
                || point.y >= _size.y
                || point.y < 0)
                return false;

            return true;           
        }
        public IEnumerator GetEnumerator() => _field.GetEnumerator();

    }
}
