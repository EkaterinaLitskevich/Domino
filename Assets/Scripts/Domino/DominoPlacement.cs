using System.Collections.Generic;
using UnityEngine;

namespace Domino
{
    public class DominoPlacement : MonoBehaviour
    {
        [SerializeField] private List<Transform> _pointsPositionUp = new List<Transform>();
        [SerializeField] private List<Transform> _pointsPositionDown = new List<Transform>();
        
        public void PlaceDomino(DominoController domino, bool isStand)
        {
            Transform point;
            
            if (isStand)
            {
                point = GetPointPosition(_pointsPositionUp);
            }
            else
            {
                point = GetPointPosition(_pointsPositionDown);
            }

            domino.transform.localPosition = point.position * 100;
        }

        private Transform GetPointPosition(List<Transform> points)
        {
            Transform transform;

            if (points[0] != null)
            {
                transform = points[0];
                points.RemoveAt(0);
            }
            else
            {
                transform = null; //getTransform near domino
            }
            return transform;
        }
    }
}
