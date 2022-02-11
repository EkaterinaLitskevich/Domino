using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
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
            dominoController.HandlerClick.OnBeginDragObj += BeginDragDomino;
            dominoController.HandlerClick.OnDragObj += MoveDomino;
            dominoController.HandlerClick.OnEndDragObj += EndDragDomino;
        }

        private void MoveDomino(PointerEventData eventData)
        {
            if (_dominoCintrollerUsed != null)
            {
                _dominoCintrollerUsed.transform.position =
                    Vector2.Lerp(_dominoCintrollerUsed.transform.position, eventData.position, 1f);
            }
        }

        private void EndDragDomino(PointerEventData eventData)
        {
            if (_dominoCintrollerUsed != null)
            {
                PlacementDominoNextToDomino(eventData);
                _dominoCintrollerUsed = null;
            }
        }
        
        private void BeginDragDomino(PointerEventData eventData)
        {
            if (eventData.pointerCurrentRaycast.gameObject == null)
            {
                return;
            }

            if (eventData.pointerCurrentRaycast.gameObject.transform.parent.TryGetComponent(out DominoController dominoController))
            {
                if (!dominoController.IsStand)
                {
                    _dominoCintrollerUsed = dominoController;
                    _startPositionDomino = dominoController.RectTransform.anchoredPosition;
                    Debug.Log("_startPositionDomino = " + _startPositionDomino);
                }
            }
        }

        private void PlacementDominoNextToDomino(PointerEventData eventData)
        {
            RaycastHit2D hit =
                Physics2D.CircleCast(_dominoCintrollerUsed.transform.position, 200, Vector2.zero);

            if (hit.collider == null)
            {
                _dominoCintrollerUsed.RectTransform.anchoredPosition = _startPositionDomino;
                return;
            }

            if (hit.collider.TryGetComponent(out DominoController standDominoController))
            {
                if (standDominoController.IsStand)
                {
                    CalculatePosition(_dominoCintrollerUsed, standDominoController);
                    _dominoCintrollerUsed.IsStand = true;
                }
                else
                {
                    _dominoCintrollerUsed.RectTransform.anchoredPosition = _startPositionDomino;
                }
            }
            else
            {
                _dominoCintrollerUsed.RectTransform.anchoredPosition = _startPositionDomino;
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
