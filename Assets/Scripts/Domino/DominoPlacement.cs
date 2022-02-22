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
        [SerializeField] private List<RectTransform> _pointsPositionDown = new List<RectTransform>();
        [SerializeField] private float _offsetBetweenDomino;
        [SerializeField] private float _dominoVisibilityRange;
        
        private Subject<CallBackDominoPlacement> listeners = new Subject<CallBackDominoPlacement>();
        private DominoController _dominoCintrollerUsed;
        private Vector2 _startPositionDomino;
        private int _indexElement;

        public IObservable<CallBackDominoPlacement> Trigger => listeners;
        
        public void PlaceDomino(DominoController domino, bool isStand, ColumnDomino columnDomino)
        {
            SubscribeDrag(domino);
            
            RectTransform point = new RectTransform();

            if (isStand)
            {
                domino.RectTransform.anchoredPosition = columnDomino.FirstPointPosition;

                CreateListColumn(domino);
                columnDomino.AddToList(domino);
            }
            else
            {
                if (_indexElement >= _pointsPositionDown.Count)
                {
                    _indexElement = 0;
                }
                point = GetPointPosition(_pointsPositionDown);
                domino.RectTransform.anchoredPosition = point.anchoredPosition;
            }
        }

        private void CreateListColumn(DominoController domino)
        {
            List<DominoController> dominoControllers = new List<DominoController>();
            dominoControllers.Add(domino);
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
                if (colliders[i].collider.TryGetComponent(out ColumnDomino columnDomino))
                {
                    DominoController dominoController =
                        columnDomino.DominoControllers[columnDomino.DominoControllers.Count - 1];

                    bool isFit = CompareDomino(_dominoCintrollerUsed, dominoController);

                    if (isFit)
                    {
                        _dominoCintrollerUsed.RemoveHalf();
                        bool isEmptyDominoStand = dominoController.RemoveHalf();
                        
                        if (isEmptyDominoStand)
                        {
                            columnDomino.RemoveToList(dominoController);
                            DestroyDomino(ref dominoController);
                            dominoController = columnDomino.DominoControllers[columnDomino.DominoControllers.Count - 1];
                        }
                    }

                    CalculatePosition(dominoController);
                    bool isFullColumn = columnDomino.AddToList(_dominoCintrollerUsed);

                    if (isFullColumn)
                    {
                        listeners.OnNext(new CallBackDominoPlacement(KeysStorage.EmptyRowUp, _dominoCintrollerUsed, null));
                    }

                    _dominoCintrollerUsed.IsStand = true;

                    listeners.OnNext(new CallBackDominoPlacement(KeysStorage.DominoPlacement, _dominoCintrollerUsed, null));

                    return;
                }
            }
        }

        private void DestroyDomino(ref DominoController dominoStand)
        {
            Destroy(dominoStand.gameObject);
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
