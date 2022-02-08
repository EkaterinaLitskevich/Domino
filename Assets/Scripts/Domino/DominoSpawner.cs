using System;
using System.Collections.Generic;
using UnityEngine;

namespace Domino
{
    public class DominoSpawner : MonoBehaviour
    {
        [SerializeField] private DominoController _dominoPrefab;
        [SerializeField] private Transform _pointPositionUp;
        [SerializeField] private Transform _pointPositionDown;
        [SerializeField] private int _amountDomino;

        private List<DominoController> _dominoControllersStand = new List<DominoController>();
        private List<DominoController> _dominoControllersGame = new List<DominoController>();

        private void Start()
        {
            CreateDomino(_dominoControllersStand, _pointPositionUp, true);
            CreateDomino(_dominoControllersGame, _pointPositionDown, false);
        }

        private void CreateDomino(List<DominoController> dominoControllers, Transform point, bool isStand)
        {
            for (int i = 0; i < _amountDomino; i++)
            {
                DominoController dominoController = Instantiate(_dominoPrefab, point.position, transform.rotation, transform);
                dominoController.IsStand = isStand;
                dominoControllers.Add(dominoController);
            }
        }
    
    }
}
