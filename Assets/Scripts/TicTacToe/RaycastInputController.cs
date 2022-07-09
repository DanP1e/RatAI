using UnityEngine;
using UnityEngine.Events;

namespace TicTacToe
{
    public class RaycastInputController : MonoBehaviour, IGameInputController
    {
        public event UnityAction<Vector2Int> CellPressed;

        private void Update() 
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit))
                {
                    IMatrixElement matrixElement 
                        = (IMatrixElement)hit.transform.GetComponent(typeof(IMatrixElement));

                    if (matrixElement != null)
                    {
                        CellPressed?.Invoke(matrixElement.PositionInMatrix);                     
                    }
                }
            }
        }
    }
}
