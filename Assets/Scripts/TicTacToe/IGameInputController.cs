using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace TicTacToe
{
    public interface IGameInputController
    {
        event UnityAction<Vector2Int> CellPressed;
    } 
}
