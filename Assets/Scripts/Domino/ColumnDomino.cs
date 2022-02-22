using System.Collections.Generic;
using UnityEngine;

namespace Domino
{
    public class ColumnDomino : MonoBehaviour
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private int _maxHalfsInColumn;
        
        private List<DominoController> _dominoControllers = new List<DominoController>();
        private Vector3 _firstPointPosition;

        public List<DominoController> DominoControllers => _dominoControllers;
        public RectTransform RectTransform => _rectTransform;
        public Vector3 FirstPointPosition => _firstPointPosition;

        public void SetFirstPointPositionDomino(Vector3 firstPointPosition)
        {
            _firstPointPosition = firstPointPosition;
        }
        
        public bool AddToList(DominoController dominoController)
        {
            _dominoControllers.Add(dominoController);
            bool isFullColumn = CheckAmountHalfs();

            return isFullColumn;
        }

        public void RemoveToList(DominoController dominoController)
        {
            _dominoControllers.Remove(dominoController);
        }

        public void ClearList()
        {
            for (int i =  _dominoControllers.Count - 1; i >= 0; i--)
            {
                Destroy(_dominoControllers[i].gameObject);
                _dominoControllers.RemoveAt(i);
            }
        }
        
        private bool CheckAmountHalfs()
        {
            int halfs = 0;
            
            for (int i = 0; i < _dominoControllers.Count; i++)
            {
                halfs += _dominoControllers[i].HalfsCount;

                if (halfs >= _maxHalfsInColumn)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
