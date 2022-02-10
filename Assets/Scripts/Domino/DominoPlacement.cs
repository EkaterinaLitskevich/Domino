using System.Collections.Generic;
using Cysharp.Threading.Tasks;
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
            dominoController.HandlerClick.OnDragObj += MoveDomino;
            dominoController.HandlerClick.OnBeginDragObj += BeginDragDomino;
            dominoController.HandlerClick.OnEndDragObj += EndDragDomino;
            //_dominoController.HandlerClick.OnEndDragObj += SetPositionDomino;
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
            _dominoCintrollerUsed = null;
        }
        
        private void BeginDragDomino(PointerEventData eventData)
        {
            if (eventData.pointerCurrentRaycast.gameObject == null)
            {
                return;
            }

            if (eventData.pointerCurrentRaycast.gameObject.transform.parent.TryGetComponent(out DominoController dominoController))
            {
                _dominoCintrollerUsed = dominoController;
            }
        }

        private void SetPositionDomino(PointerEventData eventData)
        {
            if (eventData.pointerCurrentRaycast.gameObject.transform.parent.TryGetComponent(out DominoController dominoController))
            {
                RaycastHit2D hit = Physics2D.CircleCast(dominoController.transform.position, 1, Vector2.zero);

                if (hit.collider == null)
                {
                    return;
                }

                if (hit.collider.TryGetComponent(out DominoController standDominoController))
                {
                    CalculatePosition(dominoController, standDominoController);
                }
            }
        }

        private void CalculatePosition(DominoController dominoController, DominoController standDominoController)
        {
            float halfStandDominoSize = standDominoController.DefaultSizeDelta.y / 2;
            float halfDominoControllerSize = dominoController.DefaultSizeDelta.y / 2;

            float newPositionY = standDominoController.RectTransform.anchoredPosition.y - halfStandDominoSize - _offsetBetweenDomino - halfDominoControllerSize;
            float newPositionX = standDominoController.RectTransform.anchoredPosition.x;
            Debug.Log("newPositionY = " + newPositionY);
            Debug.Log("newPositionX = " + newPositionX);

            Debug.Log("dominoController.RectTransform.anchoredPosition" + dominoController.RectTransform.anchoredPosition);
            dominoController.RectTransform.anchoredPosition = new Vector2(newPositionX ,newPositionY); 
            Debug.Log("new" + dominoController.RectTransform.anchoredPosition);
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
