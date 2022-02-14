using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DragAndDrop;
using UniRx;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace Domino
{
    public class DominoPlacement : MonoBehaviour
    {
        [SerializeField] private List<RectTransform> _pointsPositionUp = new List<RectTransform>();
        [SerializeField] private List<RectTransform> _pointsPositionDown = new List<RectTransform>();
        [SerializeField] private float _offsetBetweenDomino;
        [SerializeField] private float _dominoVisibilityRange;

        private DominoController _dominoCintrollerUsed;

        private Vector2 _startPositionDomino;

        public void PlaceDomino(DominoController domino, bool isStand)
        {
            SubscribeDrag(domino);
            
            RectTransform point;

            if (isStand)
            {
                point = GetPointPosition(_pointsPositionUp);
            }
            else
            {
                point = GetPointPosition(_pointsPositionDown);
            }
            
            domino.RectTransform.anchoredPosition = point.anchoredPosition;
        }

        private void SubscribeDrag(DominoController dominoController)
        {
            var disposable = new CompositeDisposable();
            
            dominoController.HandlerClick.Trigger.Where(result => 
                result.Key.Equals(KeysStorage.BeginDrag)).Subscribe(BeginDragDomino).AddTo(disposable); 
            
            dominoController.HandlerClick.Trigger.Where(result 
                => result.Key.Equals(KeysStorage.Drag)).Subscribe(MoveDomino).AddTo(disposable); 
            
            dominoController.HandlerClick.Trigger.Where(result =>
                result.Key.Equals(KeysStorage.EndDrag)).Subscribe(EndDragDomino).AddTo(disposable);
        }

        private void MoveDomino(CallBack callBack)
        {
            if (_dominoCintrollerUsed != null)
            {
                _dominoCintrollerUsed.transform.position =
                    Vector2.Lerp(_dominoCintrollerUsed.transform.position, callBack.PointerData.position, 1f);
            }
        }

        private void EndDragDomino(CallBack callBack)
        {
            if (_dominoCintrollerUsed != null)
            {
                PlacementDominoNextToDomino(callBack.PointerData);
                _dominoCintrollerUsed = null;
            }
        }
        
        private void BeginDragDomino(CallBack callBack)
        {
            if (callBack.PointerData.pointerCurrentRaycast.gameObject == null)
            {
                return;
            }

            if (callBack.PointerData.pointerCurrentRaycast.gameObject.transform.parent.TryGetComponent(out DominoController dominoController))
            {
                if (!dominoController.IsStand)
                {
                    _dominoCintrollerUsed = dominoController;
                    _startPositionDomino = dominoController.RectTransform.anchoredPosition;
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
                if (colliders[i].collider.TryGetComponent(out DominoController standDominoController))
                {
                    if (standDominoController.IsStand && standDominoController.IsLast)
                    {
                        CalculatePosition(_dominoCintrollerUsed, standDominoController);
                        _dominoCintrollerUsed.IsStand = true;

                        standDominoController.IsLast = false;
                        _dominoCintrollerUsed.IsLast = true;
                        
                        return;
                    }
                }
            }
        }

        private void CalculatePosition(DominoController dominoController, DominoController standDominoController)
        {
            float halfStandDominoSize = standDominoController.DefaultSizeDelta.y / 2;
            float halfDominoControllerSize = dominoController.DefaultSizeDelta.y / 2;

            Vector2 anchoredPosition = standDominoController.RectTransform.anchoredPosition;
            float newPositionY = anchoredPosition.y - halfStandDominoSize - _offsetBetweenDomino - halfDominoControllerSize;
            float newPositionX = anchoredPosition.x;

            dominoController.RectTransform.anchoredPosition = new Vector2(newPositionX ,newPositionY);
        }

        private RectTransform GetPointPosition(List<RectTransform> points)
        {
            RectTransform point;

            if (points[0] != null)
            {
                point = points[0];
                points.RemoveAt(0);
            }
            else
            {
                point = null; //getTransform near domino
            }
            return point;
        }
    }
}
