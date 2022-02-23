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
        [SerializeField] private Camera _camera;
        [SerializeField] private List<Transform> _pointsPositionDown = new List<Transform>();// create dynamic place down domino
        [SerializeField] private float _offsetBetweenDomino;
        [SerializeField] private Vector2 _dominoVisibilityRange;
        
        private Subject<CallBackDominoPlacement> listeners = new Subject<CallBackDominoPlacement>();
        private DominoController _dominoCintrollerUsed;
        private Vector2 _startPositionDomino;
        private int _indexElement;

        public IObservable<CallBackDominoPlacement> Trigger => listeners;
        
        public void PlaceDomino(DominoController domino, bool isStand, ColumnDomino columnDomino)
        {
            SubscribeDrag(domino);
            
            Vector2 point = new Vector2();

            if (isStand)
            {
                columnDomino.AddToList(domino);
                domino.transform.position = columnDomino.FirstPointPosition;
            }
            else
            {
                if (_indexElement >= _pointsPositionDown.Count)
                {
                    _indexElement = 0;
                }
                point = GetPointPosition(_pointsPositionDown);
                domino.transform.position = point;
            }
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
                Vector2 positionPointerData = _camera.ScreenToWorldPoint(callBackDrag.PointerData.position);
                
                _dominoCintrollerUsed.transform.position =
                    Vector2.Lerp(_dominoCintrollerUsed.transform.position, positionPointerData, 1f);
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
                    _startPositionDomino =  dominoController.transform.position;
                }
            }
        }

        private void PlacementDominoNextToDomino(PointerEventData eventData)
        {
            RaycastHit2D[] colliders =
                Physics2D.CapsuleCastAll(_dominoCintrollerUsed.transform.position, _dominoVisibilityRange, CapsuleDirection2D.Vertical, 0, Vector2.zero);

            _dominoCintrollerUsed.transform.position = _startPositionDomino;

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
            float halfStandDominoSize = standDominoController.SideSizeHalf * standDominoController.HalfsCount / 2;
            float halfDominoControllerSize = _dominoCintrollerUsed.SideSizeHalf * _dominoCintrollerUsed.HalfsCount / 2;

            float distanceBetweenHalfs = 0;

            if (_dominoCintrollerUsed.HalfsCount == 1)
            {
                distanceBetweenHalfs = standDominoController.DistanceBetweenHalf;
            }

            Vector2 position = standDominoController.transform.position;
            float newPositionY = position.y - halfStandDominoSize - _offsetBetweenDomino - halfDominoControllerSize + distanceBetweenHalfs;
            float newPositionX = position.x;
            
            Debug.Log("halfStandDominoSize = " + halfStandDominoSize);
            Debug.Log("halfDominoControllerSize = " + halfDominoControllerSize);
            Debug.Log("position.y = " + position.y);
            Debug.Log("newPositionY = " + newPositionY);

            _dominoCintrollerUsed.transform.position = new Vector2(newPositionX ,newPositionY);
        }

        private Vector2 GetPointPosition(List<Transform> points)
        {
            Transform point;

            point = points[_indexElement];
            _indexElement++;

            return point.position;
        }
    }
}
