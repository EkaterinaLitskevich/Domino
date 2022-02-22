using System.Collections.Generic;
using DragAndDrop;
using Random;
using UniRx;
using UnityEngine;
using Zenject;

namespace Domino
{
    public class DominoSpawner : MonoBehaviour
    {
        private const int MaxValue = 6;
        private const int MinValue = 0;
        
        [SerializeField] private DominoController _dominoPrefab;
        [SerializeField] private int _amountColumnStand;
        [SerializeField] private int _amountDominoGame;

        private List<DominoController> _dominoControllersStand = new List<DominoController>();
        private List<DominoController> _dominoControllersGame = new List<DominoController>();

        private CompositeDisposable _disposable = new CompositeDisposable();

        [Inject] private DominoPlacement _dominoPlacement;
        [Inject] private ColumnSpawner _columnSpawner;
        [Inject] private IRandom _randomizer;

        private void OnEnable()
        {
            _dominoPlacement.Trigger.Where(result => result.Key.Equals(KeysStorage.DominoPlacement)).Subscribe(AddToListDominoStand).AddTo(_disposable);
            _dominoPlacement.Trigger.Where(result => result.Key.Equals(KeysStorage.DominoDestroy)).Subscribe(RemoveFromArray).AddTo(_disposable);
            _dominoPlacement.Trigger.Where(result => result.Key.Equals(KeysStorage.EmptyRowUp)).Subscribe(CreateDominoStand).AddTo(_disposable);
        }

        private void OnDisable()
        {
            _disposable.Dispose();
        }

        public void CreateStartDomino()
        {
            _columnSpawner.InitialColumn(_amountColumnStand);
            CreateDominoStand(null);
            CreateDominoGame();
        }

        public void RotateDominoGame()
        {
            for (int i = 0; i < _dominoControllersGame.Count; i++)
            {
                _dominoControllersGame[i].Rotate();
            }
        }

        public void ShuffleDomino()
        {
            for (int i = 0; i < _dominoControllersGame.Count; i++)
            {
                List<int> randomValues = CreateRandomValuesArray(_dominoControllersGame[i].HalfsCount);
                _dominoControllersGame[i].SetValueHalfs(randomValues);
            }
        }

        private void RemoveFromArray(CallBackDominoPlacement callBackDominoPlacement)
        {
            SearchDominoInArray(_dominoControllersStand, callBackDominoPlacement.DominoControllerStand);
        }

        private void SearchDominoInArray(List<DominoController> dominoControllers, DominoController dominoControllerStand)
        {
            for (int i = 0; i < dominoControllers.Count; i++)
            {
                if (dominoControllers[i] == dominoControllerStand)
                {
                    dominoControllers.RemoveAt(i);
                    
                    return;
                }
            }
        }

        private void CreateDominoStand(CallBackDominoPlacement callBackDominoPlacement)
        {
            _columnSpawner.ClearColumns();
            InitialDomino(_dominoControllersStand, _amountColumnStand, true, true, true);
        }
        
        private void CreateDominoGame()
        {
            InitialDomino(_dominoControllersGame, _amountDominoGame, false, false, false);
        }

        private void AddToListDominoStand(CallBackDominoPlacement callBackDominoPlacement)
        {
            _dominoControllersStand.Add(callBackDominoPlacement.DominoControllerGame);
            _dominoControllersGame.Remove(callBackDominoPlacement.DominoControllerGame);

            callBackDominoPlacement.DominoControllerGame.SetHalfForCompare();

            CheckArrayDominoGame();
        }
        
        private void CheckArrayDominoGame()
        {
            if (_dominoControllersGame.Count <= 0)
            {
                CreateDominoGame();
            }
        }

        private void InitialDomino(List<DominoController> dominoControllers, int amountColumn, bool isStand, bool isFirst, bool isLast)
        {
            for (int i = 0; i < amountColumn; i++)
            {
                DominoController dominoController = CreateDomino(isStand, isFirst, isLast);
                
                List<int> randomValues = CreateRandomValuesArray(dominoController.HalfsCount);
                dominoController.Initial(randomValues);
                
                _dominoPlacement.PlaceDomino(dominoController, isStand, _columnSpawner.ColumnsDomino[i]);

                dominoControllers.Add(dominoController);
            }
        }
        
        private List<int> CreateRandomValuesArray(int amountHalfs)
        { 
            List<int> halfs = new List<int>();

            for (int i = 0; i < amountHalfs; i++)
            {
                int randomValue = _randomizer.GetRandomValue(MinValue, MaxValue);
                halfs.Add(randomValue);
            }
            
            return halfs;
        }
        
        private DominoController CreateDomino(bool isStand, bool isFirst, bool isLast)
        {
            DominoController dominoController = Instantiate(_dominoPrefab, transform);
            
            dominoController.IsStand = isStand;

            return dominoController;
        }
    }
}

