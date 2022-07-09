using System;
using System.Collections;
using System.Collections.Generic;
using TicTacToe;
using UnityEngine;

public abstract class FieldDrawer : MonoBehaviour
{   
    [SerializeField] private TicTacToeGame _ticTacToeGame;

    protected TicTacToeGame TicTacToeGame { get => _ticTacToeGame; }

    private void OnEnable()
    {
        UpdateGameField();
        _ticTacToeGame.MoveMade += OnGameMoveMade;
    }
    private void OnDisable()
    {
        _ticTacToeGame.MoveMade -= OnGameMoveMade;
    }
    private void OnGameMoveMade(TicTacToePlayer arg0, Vector2Int arg1)
    {
        UpdateGameField();
    }

    public abstract void UpdateGameField();   
}
