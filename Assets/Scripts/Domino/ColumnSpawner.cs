using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Domino
{
    public class ColumnSpawner : MonoBehaviour
    {
        [SerializeField] private ColumnDomino _columnDominoPrefab;

        private List<ColumnDomino> _columnsDomino = new List<ColumnDomino>();
        
        [Inject] private ColumnPlacement _columnPlacement;
        
        public List<ColumnDomino> ColumnsDomino => _columnsDomino;

        public float InitialColumn(int amountColumn)
        {
            float sideSizeDomino = _columnPlacement.CalculateGameField(amountColumn);
            CreateColumn(amountColumn);

            return sideSizeDomino;
        }

        public void ClearColumns()
        {
            for (int i = 0; i < _columnsDomino.Count; i++)
            {
                _columnsDomino[i].ClearList();
            }
        }

        private void CreateColumn(int amountColumn)
        {
            for (int i = 0; i < amountColumn; i++)
            {
                ColumnDomino columnDomino = Instantiate(_columnDominoPrefab, transform);
                columnDomino.transform.position = _columnPlacement.GetPositionColumn();

                Vector2 fullScreen = _columnPlacement.Camera.ScreenToWorldPoint(new Vector3(0, Screen.height, 0));
                Vector3 positionColumn = new Vector3(columnDomino.transform.position.x, fullScreen.y - _columnPlacement.IndentDown, 0);

                columnDomino.SetFirstPointPositionDomino(positionColumn);
                
                _columnsDomino.Add(columnDomino);
            }
        }
    }
}
