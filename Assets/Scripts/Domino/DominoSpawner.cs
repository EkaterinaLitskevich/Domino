using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Domino
{
    public class DominoSpawner : MonoBehaviour
    {
        private const int MaxValue = 6;
        private const int MinValue = 0;
        
        [SerializeField] private DominoController _dominoPrefab;
        [SerializeField] private int _amountDominoStand;
        [SerializeField] private int _amountDominoGame;

        private List<DominoController> _dominoControllersStand = new List<DominoController>();
        private List<DominoController> _dominoControllersGame = new List<DominoController>();

        [Inject] private DominoPlacement _dominoPlacement;
        //[Inject] private Randomizer _randomizer;
        
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

                //int randomValue = _randomizer.GetRandomValue(MinValue, MaxValue);
                dominoController.Initial(0);
            }
        }
        
        private DominoController CreateDomino(bool isStand)
        {
            DominoController dominoController =
                Instantiate(_dominoPrefab, transform);
            dominoController.IsStand = isStand;

            if (isStand)
            {
                dominoController.IsLast = true;
            }

            return dominoController;
        }
    }
}

