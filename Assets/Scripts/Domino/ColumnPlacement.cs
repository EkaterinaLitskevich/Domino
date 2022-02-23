using System;
using System.Collections.Generic;
using UnityEngine;

namespace Domino
{
    public class ColumnPlacement : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private float _percentScreenWidth = 80;
        [SerializeField] private float _percentScreenHeight = 70;
        [SerializeField] private float _distanceBetweenColumnsX = 1;
        //[SerializeField] private float _distanceBetweenColumnsY = 0.3f;
        [SerializeField] private float _indentDown = 1;

        private List<Vector2> _positionsColumns = new List<Vector2>();
        private Vector2 _fullScreen;
        private float _gameFieldWidth;
        private float _gameFieldHeight;

        public Camera Camera => _camera;
        public float IndentDown => _indentDown;

        public Vector2 GetPositionColumn()
        {
            Vector2 point = _positionsColumns[0];
            _positionsColumns.RemoveAt(0);
            return point;
        }
        
        public float CalculateGameField(int amountColumn)
        {
            _fullScreen = _camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
            _fullScreen *= 2;

            _gameFieldWidth = _fullScreen.x * _percentScreenWidth / 100;
            _gameFieldHeight = _fullScreen.y * _percentScreenHeight / 100;
            
            float sumSizeDominoX = _gameFieldWidth - _distanceBetweenColumnsX * (amountColumn - 1);

            float  sideSizeDomino = sumSizeDominoX / amountColumn / 2;

            CalculatePositions(sideSizeDomino, amountColumn);

            return sideSizeDomino;
        }

        private void CalculatePositions(float sizeDominoWidth, int amountColumn)
        {
            int firstHalfAmountColumn = amountColumn / 2;

            if (amountColumn % 2 != 0)
            {
                firstHalfAmountColumn += 1;
            }
            
            int twoHalfAmountColumn = amountColumn - firstHalfAmountColumn;

            float pointX = 0;
            float pointY = 0; 

            pointY += _fullScreen.y / 2 - _gameFieldHeight / 2 + _indentDown; 
            
            for (int i = 0; i < firstHalfAmountColumn; i++)
            {
                Vector2 point = new Vector2();
                
                if (amountColumn % 2 != 0 && i == 0)
                {
                    point = new Vector2(pointX, pointY); 
                }
                else
                {
                    if (i == 0)
                    {
                        point = new Vector2(pointX += sizeDominoWidth / 2 + _distanceBetweenColumnsX / 2, pointY); 
                    }
                    else
                    {
                        point = new Vector2(pointX += sizeDominoWidth + _distanceBetweenColumnsX, pointY); 
                    }
                }
                
                _positionsColumns.Add(point);
            }
            
            pointX = _positionsColumns[0].x;

            for (int i = 0; i < twoHalfAmountColumn; i++)
            {
                Vector2 point = new Vector2();

                point = new Vector2(pointX -= sizeDominoWidth + _distanceBetweenColumnsX, pointY);

                _positionsColumns.Add(point);
            }
        }
    }
}
