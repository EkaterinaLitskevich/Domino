using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Domino
{
    public class ColumnSpawner : MonoBehaviour
    {
        [SerializeField] private ColumnDomino _columnDominoPrefab;

        private List<ColumnDomino> _columnsDomino = new List<ColumnDomino>();
        public List<ColumnDomino> ColumnsDomino => _columnsDomino;

        [Inject] private ColumnPlacement _columnPlacement;
    
        public void InitialColumn(int amountColumn)
        {
            _columnPlacement.CalculateGameField(amountColumn);
            CreateColumn(amountColumn);
        }

        private void CreateColumn(int amountColumn)
        {
            for (int i = 0; i < amountColumn; i++)
            {
                ColumnDomino columnDomino = Instantiate(_columnDominoPrefab, transform);
                columnDomino.RectTransform.anchoredPosition = _columnPlacement.GetPositionColumn();
                columnDomino.SetFirstPointPositionDomino(new Vector3(columnDomino.RectTransform.anchoredPosition.x, Screen.height / 2 - 200, 0));
                
                _columnsDomino.Add(columnDomino);
            }
        }
    }
}
