using System;
using System.Collections.Generic;
using DragAndDrop;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Domino
{
    public class DominoPlacement : MonoBehaviour
    {
        [SerializeField] private List<RectTransform> _pointsPositionUp = new List<RectTransform>();
        [SerializeField] private List<RectTransform> _pointsPositionDown = new List<RectTransform>();
        [SerializeField] private float _offsetBetweenDomino;
        [SerializeField] private float _dominoVisibilityRange;
        [SerializeField] private int _maxHalfsInColumn;

        private List<List<DominoController>> _columnsDomino = new List<List<DominoController>>();
        private DominoController _dominoCintrollerUsed;
        
        private Subject<CallBackDominoPlacement> listeners = new Subject<CallBackDominoPlacement>();
        private Vector2 _startPositionDomino;
        private int _indexElement;

        public IObservable<CallBackDominoPlacement> Trigger => listeners;
        
        public void PlaceDomino(DominoController domino, bool isStand)
        {
            SubscribeDrag(domino);
            
            RectTransform point;

            if (isStand)
            {
                if (_indexElement >= _pointsPositionUp.Count)
                {
                    _indexElement = 0;
                }
                point = GetPointPosition(_pointsPositionUp);

                CreateListColumn(domino);
            }
            else
            {
                if (_indexElement >= _pointsPositionDown.Count)
                {
                    _indexElement = 0;
                }
                point = GetPointPosition(_pointsPositionDown);
            }
            
            domino.RectTransform.anchoredPosition = point.anchoredPosition;
        }

        private void CreateListColumn(DominoController domino)
        {
            List<DominoController> dominoControllers = new List<DominoController>();
            dominoControllers.Add(domino);
            _columnsDomino.Add(dominoControllers);
        }

        private void SubscribeDrag(DominoController dominoController)
        {
            dominoController.HandlerClick.Trigger.Where(result =>
                result.Key.Equals(KeysStorage.BeginDrag)).Subscribe(BeginDragDomino);

            dominoController.HandlerClick.Trigger.Where(result
                => result.Key.Equals(KeysStorage.Drag)).Subscribe(MoveDomino);

            dominoController.HandlerClick.Trigger.Where(result =>
                result.Key.Equals(KeysStorage.EndDrag)).Subscribe(EndDragDomino);
        }

        private void MoveDomino(CallBackDrag callBackDrag)
        {
            if (_dominoCintrollerUsed != null)
            {
                _dominoCintrollerUsed.transform.position =
                    Vector2.Lerp(_dominoCintrollerUsed.transform.position, callBackDrag.PointerData.position, 1f);
            }
        }

        private void EndDragDomino(CallBackDrag callBackDrag)
        {
            if (_dominoCintrollerUsed != null)
            {
                PlacementDominoNextToDomino(callBackDrag.PointerData);
                _dominoCintrollerUsed = null;
            }
        }
        
        private void BeginDragDomino(CallBackDrag callBackDrag)
        {
            if (callBackDrag.PointerData.pointerCurrentRaycast.gameObject == null)
            {
                return;
            }

            if (callBackDrag.PointerData.pointerCurrentRaycast.gameObject.transform.parent.TryGetComponent(out DominoController dominoController))
            {
                if (!dominoController.IsStand)
                {
                    _dominoCintrollerUsed = dominoController;
                    _startPositionDomino =  dominoController.RectTransform.anchoredPosition;
                }
            }
        }

        private void PlacementDominoNextToDomino(PointerEventData eventData)
        {
            RaycastHit2D[] colliders =
                Physics2D.CircleCastAll(_dominoCintrollerUsed.transform.position, _dominoVisibilityRange, Vector2.zero);

            _dominoCintrollerUsed.RectTransform.anchoredPosition = _startPositionDomino;

            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].collider.TryGetComponent(out DominoController dominoControllerStand))
                {
                    if (dominoControllerStand.IsStand && dominoControllerStand.IsLast)
                    {
                        bool isFit = CompareDomino(_dominoCintrollerUsed, dominoControllerStand);

                        if (isFit)
                        {
                            _dominoCintrollerUsed.RemoveHalf();
                            bool isEmptyDominoStand = dominoControllerStand.RemoveHalf();

                            if (isEmptyDominoStand)
                            {
                                DestroyDomino(ref dominoControllerStand, null);
                            }
                        }

                        CalculatePosition(dominoControllerStand);
                        AddToArray(dominoControllerStand);
                        
                        dominoControllerStand.IsLast = false;
                        _dominoCintrollerUsed.IsStand = true;
                        _dominoCintrollerUsed.IsLast = true;

                        listeners.OnNext(new CallBackDominoPlacement(KeysStorage.DominoPlacement, _dominoCintrollerUsed, null));

                        return;
                    }
                }
            }
        }

        private void DestroyDomino(ref DominoController dominoStand, DominoController dominoGame)
        {
            RemoveFromArray(ref dominoStand);
            listeners.OnNext(new CallBackDominoPlacement(KeysStorage.DominoDestroy, null, dominoStand));
        }

        private bool CompareDomino(DominoController dominoGame, DominoController dominoStand)
        {
            if (dominoGame.HalfForCompare.Value == dominoStand.HalfForCompare.Value)
            {
                return true;
            }

            return false;
        }

        private void RemoveFromArray(ref DominoController dominoStand)
        {
            for (int i = 0; i < _columnsDomino.Count; i++)
            {
                for (int j = 0; j < _columnsDomino[i].Count; j++)
                {
                    if (_columnsDomino[i][j] == dominoStand)
                    {
                        Destroy(_columnsDomino[i][j].gameObject);
                        _columnsDomino[i].RemoveAt(j);

                         int previousIndex = j - 1;
                         if (_columnsDomino[i].Count > previousIndex && previousIndex >= 0)
                         {
                             _columnsDomino[i][previousIndex].IsLast = true;
                             dominoStand = _columnsDomino[i][previousIndex];
                         } 
                         
                         return;
                    }
                }
            }
        }
        
        
        private void AddToArray(DominoController standDominoController)
        {
            for (int i = 0; i < _columnsDomino.Count; i++)
            {
                for (int j = 0; j < _columnsDomino[i].Count; j++)
                {
                    if (standDominoController == _columnsDomino[i][j])
                    {
                        _columnsDomino[i].Add(_dominoCintrollerUsed);
                        CheckAmountHalfs(_columnsDomino[i]);
                        return;
                    }
                }
            }
        }

        private void CheckAmountHalfs(List<DominoController> column)
        {
            int halfs = 0;
            
            for (int i = 0; i < column.Count; i++)
            {
                halfs += column[i].HalfsCount;

                if (halfs >= _maxHalfsInColumn)
                {
                    DestroyArrays();
                    
                    return;
                }
            }
        }

        private void DestroyArrays()
        {
            for (int i = _columnsDomino.Count - 1; i >= 0; i--)
            {
                for (int j = _columnsDomino[i].Count - 1; j >= 0 ; j--)
                {
                    Destroy(_columnsDomino[i][j].gameObject);
                    _columnsDomino[i].RemoveAt(j);
                }
                _columnsDomino.RemoveAt(i);
            }
            
            listeners.OnNext(new CallBackDominoPlacement(KeysStorage.EmptyRowDown, _dominoCintrollerUsed, null));
        }

        private void CalculatePosition(DominoController standDominoController)
        {
            float halfStandDominoSize = standDominoController.DefaultSizeDelta.y / 2;
            float halfDominoControllerSize = _dominoCintrollerUsed.DefaultSizeDelta.y / 2;

            Vector2 anchoredPosition = standDominoController.RectTransform.anchoredPosition;
            float newPositionY = anchoredPosition.y - halfStandDominoSize - _offsetBetweenDomino - halfDominoControllerSize;
            float newPositionX = anchoredPosition.x;

            _dominoCintrollerUsed.RectTransform.anchoredPosition = new Vector2(newPositionX ,newPositionY);
        }

        private RectTransform GetPointPosition(List<RectTransform> points)
        {
            RectTransform point;

            point = points[_indexElement];
            _indexElement++;

            return point;
        }
    }
}
