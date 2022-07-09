using System.Collections.Generic;
using UnityEngine;

namespace TicTacToe
{
    public class ConsoleFieldDrawer : FieldDrawer
    {
        [SerializeField] private TextCellAssociations _cellsDatabase;

        private List<GameObject> _spawnedObjectsPool;

        public override void UpdateGameField()
        {
            string fieldImage = "";
            for (int y = 0; y < TicTacToeGame.FieldSize.y; y++)
            {
                string line = "";
                for (int x = 0; x < TicTacToeGame.FieldSize.x; x++)
                {
                    var cellResource = _cellsDatabase.GetResource(TicTacToeGame.GetFieldCell(y, x));
                    line += cellResource.Data;
                }
                fieldImage += line + "\n";
            }
            Debug.Log(fieldImage);
        }
    }
}
