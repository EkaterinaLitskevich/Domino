using System.Collections.Generic;
using DragAndDrop;
using Installers;
using UniRx;
using UnityEngine;

namespace Domino
{
    public class DominoController : MonoBehaviour
    {
        [SerializeField] private Half _halfPrefab;
        [SerializeField] private HandlerClick _handlerClick;
        [SerializeField] private int _amountHalfs;
        [SerializeField] private float _plusSize;
        [SerializeField] private float _distanceBetweenHalf;
    
        private List<Half> _halfs = new List<Half>();

        private CompositeDisposable _disposable = new CompositeDisposable();
        private Half _halfForCompare;
        private Vector3 _defaultSize;
        private Vector2 _defaultSizeDelta;
        private float _sideSizeHalf;
        public HandlerClick HandlerClick => _handlerClick;
        public Half HalfForCompare => _halfForCompare;
        public int HalfsCount => _amountHalfs;
        public float DistanceBetweenHalf => _distanceBetweenHalf;
        public float SideSizeHalf 
        { 
            get => _sideSizeHalf; 
            set => _sideSizeHalf = value ; 
        }
        public bool IsStand { get; set; }

        private void OnEnable()
        {
            HandlerClick.Trigger.Where(result => result.Key.Equals(KeysStorage.BeginDrag)).Subscribe(SetSize).AddTo(_disposable);
            HandlerClick.Trigger.Where(result => result.Key.Equals(KeysStorage.EndDrag)).Subscribe(SetSize).AddTo(_disposable);
        }

        private void OnDisable()
        {
            _disposable.Dispose();
        }

        private void Start()
        {
            _defaultSize = transform.localScale;
        }

        public void Initial(List<int> halfs)
        {
            FillArray(halfs);
            SetHalfForCompare();
            CalculateDefaultSizeDelta();
        }

        private void CalculateDefaultSizeDelta()
        {
            _defaultSizeDelta = Vector2.zero;
            
            for (int i = 0; i < _halfs.Count; i++)
            {
                _defaultSizeDelta += new Vector2(0, _halfs[i].transform.localScale.y);
            }
        }

        private void FillArray(List<int> halfs)
        {
            int index = 0;

            for (int i = 0; i < _amountHalfs; i++)
            {
                Half half = Instantiate(_halfPrefab, transform);
                CoreSceneInstallers.Context.Container.Inject(half);

                half.transform.localScale = Vector3.one * _sideSizeHalf; 
                SetValueHalf(half, halfs);
                
                half.gameObject.name = "Half " + index;
                
                _halfs.Add(half);

                PlacementHalf(half);
                
                index++;
            }
        }

        private void PlacementHalf(Half half)
        {
            if (_halfs.Count > 0 && _halfs.Count < 2)
            {
                Vector3 startPosition = _halfs[0].transform.position;
                _halfs[0].transform.position += new Vector3(0, startPosition.y + _distanceBetweenHalf +  _halfs[0].transform.localScale.y / 2);
                
                half.transform.position  += new Vector3(0, startPosition.y - _distanceBetweenHalf - half.transform.localScale.y / 2);
            }
            else if(_halfs.Count > 2)
            {
                _halfs[0].transform.position = transform.position;
            }
            else
            {
                half.transform.position  += 
                    new Vector3(0, _halfs[_halfs.Count - 1].transform.position.y - _distanceBetweenHalf - _halfs[_halfs.Count - 1].transform.localScale.y / 2 - half.transform.localScale.y / 2);
            }
        }
        
        public void SetValueHalfs(List<int> halfs)
        {
            for (int i = 0; i < _halfs.Count; i++)
            {
                SetValueHalf(_halfs[i], halfs);
            }
        }

        private void SetValueHalf(Half half, List<int> halfs)
        {
            half.SetValue(halfs[0]);

            halfs.RemoveAt(0);
        }
        
        public void SetHalfForCompare()
        {
            if (IsStand)
            {
                _halfForCompare = _halfs[_halfs.Count - 1];
            }
            else
            {
                _halfForCompare = _halfs[0];
            }
        }

        public bool RemoveHalf()
        {
            for (int i = 0; i < _halfs.Count; i++)
            {
                if (_halfs[i] == _halfForCompare)
                {
                    Destroy(_halfs[i].gameObject);
                    _halfs.RemoveAt(i);

                    if (_halfs.Count == 1)
                    {
                        _halfs[0].transform.localPosition = Vector3.zero;
                    }

                    break;
                }
            }
            
            _amountHalfs = _halfs.Count;

            if ( _halfs.Count == 0)
            {
                return true;
            }

            CalculateDefaultSizeDelta();
            SetHalfForCompare();

            return false;
        }

        private void SetSize(CallBackDrag callBackDrag)
        {
            if (!IsStand)
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
        }
        
        public void Rotate()
        {
            SetElementForArray();
            SetHalfForCompare();
        }

        private void SetElementForArray()
        {
            for (int i = 0; i < _halfs.Count; i++)
            {
                _halfs[i].transform.SetParent(null);
            }
            
            _halfs.Reverse();

            for (int i = 0; i < _halfs.Count; i++)
            {
                _halfs[i].transform.SetParent(transform);
            }
        }
    }
}
