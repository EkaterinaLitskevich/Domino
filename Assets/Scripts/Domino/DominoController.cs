using System.Collections.Generic;
using DragAndDrop;
using Installers;
using UniRx;
using UnityEngine;

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

        private CompositeDisposable _disposable = new CompositeDisposable();
        private Half _halfForCompare;
        private Vector3 _defaultSize;
        private Vector2 _defaultSizeDelta;
        
        public RectTransform RectTransform => _rectTransform;
        public HandlerClick HandlerClick => _handlerClick;
        public Half HalfForCompare => _halfForCompare;
        public Vector2 DefaultSizeDelta => _defaultSizeDelta;
        public int HalfsCount => _amountHalfs;
        public bool IsStand { get; set; }
        public bool IsFirst { get; set; }

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
            SetSizeCollider();
            CalculateDefaultSizeDelta();
        }

        private void CalculateDefaultSizeDelta()
        {
            _defaultSizeDelta = Vector2.zero;
            
            for (int i = 0; i < _halfs.Count; i++)
            {
                _defaultSizeDelta += new Vector2(0, _halfs[i].SizeDelta.y);
            }
        }

        private void FillArray(List<int> halfs)
        {
            Half half;
            int index = 0;

            for (int i = 0; i < _amountHalfs; i++)
            {
                half = Instantiate(_halfPrefab, transform);
                CoreSceneInstallers.Context.Container.Inject(half);

                SetValueHalf(half, halfs);
                
                half.gameObject.name = "Half " + index;
                
                _halfs.Add(half);
                
                index++;
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
            Vector2 oldPosition = _rectTransform.anchoredPosition;
            
            for (int i = 0; i < _halfs.Count; i++)
            {
                if (_halfs[i] == _halfForCompare)
                {
                    Destroy(_halfs[i].gameObject);
                    _halfs.RemoveAt(i);
                    
                    break;
                }
            }
            
            _amountHalfs = _halfs.Count;

            if ( _halfs.Count == 0)
            {
                return true;
            }

            float halfHalfY = _halfs[0].SizeDelta.y / 2;
            _rectTransform.anchoredPosition = oldPosition + new Vector2(0, halfHalfY);
            
            CalculateDefaultSizeDelta();
            SetSizeCollider();
            SetHalfForCompare();

            return false;
        }

        private void SetSizeCollider()
        {
            Vector2 sizeCollider = _collider.size;
            _collider.size = new Vector2(sizeCollider.x, sizeCollider.y * _amountHalfs);
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
