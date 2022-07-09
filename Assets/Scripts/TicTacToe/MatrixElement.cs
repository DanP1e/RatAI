using UnityEngine;

namespace TicTacToe
{
    public class MatrixElement : MonoBehaviour, IMatrixElement
    {
        [SerializeField] private Vector2Int _positionInMatrix;

        public Vector2Int PositionInMatrix { get => _positionInMatrix; }

        public void SetPositionInMatrix(Vector2Int positionInMatrix)
        {
            _positionInMatrix = positionInMatrix;
        }
    }
}
