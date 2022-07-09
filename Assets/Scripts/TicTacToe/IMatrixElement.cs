using UnityEngine;

namespace TicTacToe
{
    public interface IMatrixElement
    {
        Vector2Int PositionInMatrix { get; }
    }
}