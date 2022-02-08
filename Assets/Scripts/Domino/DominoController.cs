using System.Collections.Generic;
using UnityEngine;

namespace Domino
{
    public class DominoController : MonoBehaviour
    {
        [SerializeField] private Half _halfPrefab;
        [SerializeField] private BoxCollider2D _collider;
        [SerializeField] private int _amountHalfs;
    
        private List<Half> _halfs = new List<Half>();
        private List<Half> _halfForCompare = new List<Half>();

        public bool IsStand { get; set; }

        private void Start()
        {
            FillArray();
            FillGrid();
            SetCollider();
        }

        private void FillArray()
        {
            Half half;
        
            for (int i = 0; i < _amountHalfs; i++)
            {
                half = Instantiate(_halfPrefab, transform.position, transform.rotation, null);
                _halfs.Add(half);
            }
        }

        private void FillGrid()
        {
            int halfArray = _amountHalfs / 2;

            for (int i = 0; i < halfArray; i++)
            {
                _halfs[i].transform.SetParent(transform);
                SetHalfsForCompare(true, i);
            }

            for (int i = _halfs.Count - 1; i >= halfArray; i--)
            {
                _halfs[i].transform.SetParent(transform);
                SetHalfsForCompare(false, i);
            }
        }

        private void SetHalfsForCompare(bool isStand, int i)
        {
            if (IsStand == isStand)
            {
                _halfForCompare.Add(_halfs[i]);
            }
        }

        private void SetCollider()
        {
            Vector2 sizeCollider = _collider.size;
            _collider.size = new Vector2(sizeCollider.x, sizeCollider.y * _amountHalfs);
        }
        
        private void Rotate()
        {
            
        }
    }
}
