using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Domino
{
    public class DominoController : MonoBehaviour
    {
        private const int ValueRotateZ = 180;
        
        [SerializeField] private Half _halfPrefab;
        [SerializeField] private BoxCollider2D _collider;
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private HandlerClick _handlerClick;
        [SerializeField] private int _amountHalfs;
        [SerializeField] private float _plusSize;
    
        private List<Half> _halfs = new List<Half>();
        private List<Half> _halfForCompare = new List<Half>();
        
        private Vector3 _defaultSize;
        private Vector2 _defaultSizeDelta;

        private int _halfArray;
        public bool IsStand { get; set; }
        public RectTransform RectTransform 
        { 
            get => _rectTransform; 
            set => _rectTransform = value; 
        }
        public HandlerClick HandlerClick 
        { 
            get => _handlerClick; 
            set => _handlerClick = value; 
        }
        
        public Vector2 DefaultSizeDelta 
        { 
            get => _defaultSizeDelta; 
            set => _defaultSizeDelta = value; 
        }

        private void OnEnable()
        {
            _handlerClick.OnBeginDragObj += SetSize;
            _handlerClick.OnEndDragObj += SetSize;
        }
        
        private void OnDisable()
        {
            _handlerClick.OnBeginDragObj -= SetSize;
            _handlerClick.OnEndDragObj -= SetSize;
        }

        private void Start()
        {
            _halfArray = _amountHalfs / 2;
            _defaultSize = transform.localScale;
        }

        public void Initial(int randomValue)
        {
            FillArray();
            FillGrid();
            SetSizeCollider();
            //CalculateDefoltSizeDelta();
        }

        private void CalculateDefoltSizeDelta()
        {
            for (int i = 0; i < _halfs.Count; i++)
            {
                _defaultSizeDelta += new Vector2(0, _halfs[i].RectTransform.sizeDelta.y);
            }
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
            for (int i = 0; i < _halfArray; i++)
            {
                _halfs[i].transform.SetParent(transform);
                SetHalfsForCompare(true, i);
            }

            for (int i = _halfs.Count - 1; i >= _halfArray; i--)
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

        private void SetSizeCollider()
        {
            Vector2 sizeCollider = _collider.size;
            _collider.size = new Vector2(sizeCollider.x, sizeCollider.y * _amountHalfs);
        }

        private void SetSize(PointerEventData eventData)
        {
            if (transform.localScale == _defaultSize)
            {
                transform.localScale = _defaultSize * _plusSize;
            }
            else
            {
                transform.localScale = _defaultSize;
            }
        }
        
        private void Rotate()
        {
            transform.Rotate(0,0, ValueRotateZ);
            SetElementForArray();
        }

        private void SetElementForArray()
        {
            foreach (var t in _halfs)
            {
                Debug.Log("element: " + t);
            }
            
            Half half;

            if (_halfArray > 2)
            {
                for (int i = _halfs.Count - 1; i >= _halfArray; i--) //array in reverse order
                {
                    half = _halfs[i];

                    _halfs[i] = _halfs[_halfs.Count - i - 1];
                    _halfs[_halfs.Count - i - 1] = half;
                }

                foreach (var t in _halfs)
                {
                    Debug.Log("element: " + t);
                }
                int index = 0;
                for (int i = _halfs.Count - 1; i >= _halfArray; i--) //bottom numbers in reverse order
                {
                    half = _halfs[i];
                    _halfs[i] = _halfs[_halfArray + index];
                    _halfs[_halfArray + index] = half;
                    index++;
                }

                /*for (int i = 0; i < _halfArray / 2; i++)
                {
                    Debug.Log("_halfArray / 2 = " + _halfArray / 2);
                    Debug.Log("i = " + i);
                    
                    half = _halfs[i];

                    _halfs[i] = _halfs[_halfArray - i];
                    _halfs[_halfArray - i] = half;
                }*/
            }
            else
            {
                for (int i = 0; i < _halfArray; i++)
                {
                    half = _halfs[i];

                    _halfs[i] = _halfs[_halfArray - i];
                    _halfs[_halfArray - i] = half;
                }
            }

            foreach (var t in _halfs)
            {
                Debug.Log("element: " + t);
            }
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Rotate();
            }
        }
    }
}
