using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Domino
{
    public class DominoSpawner : MonoBehaviour
    {
        [SerializeField] private DominoController _dominoPrefab;
        [SerializeField] private int _amountDominoStand;
        [SerializeField] private int _amountDominoGame;

        private List<DominoController> _dominoControllersStand = new List<DominoController>();
        private List<DominoController> _dominoControllersGame = new List<DominoController>();

        [Inject] private DominoPlacement _dominoPlacement;

        public void CreateStartDomino()
        {
            InitialDomino(_dominoControllersStand, _amountDominoStand, true);
            InitialDomino(_dominoControllersGame, _amountDominoGame, false);
        }

        private void InitialDomino(List<DominoController> dominoControllers, int amountDomino, bool isStand)
        {
            for (int i = 0; i < amountDomino; i++)
            {
                DominoController dominoController = CreateDomino(isStand);

                _dominoPlacement.PlaceDomino(dominoController, isStand);

                dominoControllers.Add(dominoController);
                
                dominoController.Initial();
            }
        }
        
        private DominoController CreateDomino(bool isStand)
        {
            DominoController dominoController =
                Instantiate(_dominoPrefab, transform);
            dominoController.IsStand = isStand;

            return dominoController;
        }
    }
}

