using System.Collections.Generic;
using UnityEngine;

namespace Domino
{
    public class ColumnDomino : MonoBehaviour
    {
        [SerializeField] private RectTransform _rectTransform;
        
        private List<DominoController> _dominoControllers = new List<DominoController>();
        private Vector3 _firstPointPosition;

        public List<DominoController> DominoControllers => _dominoControllers;
        public RectTransform RectTransform => _rectTransform;
        public Vector3 FirstPointPosition => _firstPointPosition;

        public void SetFirstPointPositionDomino(Vector3 firstPointPosition)
        {
            _firstPointPosition = firstPointPosition;
        }
    }
}
