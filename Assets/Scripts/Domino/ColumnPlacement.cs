using System.Collections.Generic;
using UnityEngine;

namespace Domino
{
    public class ColumnPlacement : MonoBehaviour
    {
        private const int PercentScreenWidth = 80;
        private const int PercentScreenHeight = 70;
        private const int DistanceBetweenColumnsX = 100;
        //private const int DistanceBetweenColumnsY = 50;
        private const int IndentDown = 80;

        [SerializeField] private Camera _camera;

        private List<Vector2> _positionsColumns = new List<Vector2>();

        private float _gameFieldWidth;
        private float _gameFieldHeight;

        public Vector2 GetPositionColumn()
        {
            Vector2 point = _positionsColumns[0];
            _positionsColumns.RemoveAt(0);
            return point;
        }
        
        public void CalculateGameField(int amountColumn)
        {
            _gameFieldWidth = Screen.width * PercentScreenWidth / 100;
            _gameFieldHeight = Screen.height * PercentScreenHeight / 100;
            

            float gameFieldWidth = _gameFieldWidth - DistanceBetweenColumnsX * amountColumn;
            //float gameFieldHeight = _gameFieldHeight - DistanceBetweenColumnsY * amountColumn;

            float sizeDominoWidth = gameFieldWidth / amountColumn;
            //float sizeDominoHeight = gameFieldHeight / amountColumn;

            CalculatePositions(sizeDominoWidth, amountColumn);
        }

        private void CalculatePositions(float sizeDominoWidth, int amountColumn)
        {
            int firstHalfAmountColumn = amountColumn / 2;
            int twoHalfAmountColumn = amountColumn - firstHalfAmountColumn;
            
            float pointX = 0;
            float pointY = 0; 

            pointY += Screen.height - _gameFieldHeight / 2 + IndentDown; 
            
            for (int i = 0; i < firstHalfAmountColumn; i++)
            {
                Vector2 point = new Vector2();
                
                if (amountColumn % 2 == 0 && i == 0)
                {
                    point = new Vector2(pointX + DistanceBetweenColumnsX / 2, pointY); 
                }
                else
                {
                    point = new Vector2(pointX += sizeDominoWidth + DistanceBetweenColumnsX, pointY); 
                }
                
                _positionsColumns.Add(point);
            }
            
            pointX = _positionsColumns[0].x;

            for (int i = 0; i < twoHalfAmountColumn; i++)
            {
                Vector2 point = new Vector2();

                point = new Vector2(pointX += sizeDominoWidth, pointY); 
                
                _positionsColumns.Add(point);
            }
        }
    }
}
