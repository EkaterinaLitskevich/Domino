using System.Threading.Tasks;
using Controllers;
using Cysharp.Threading.Tasks;
using Domino;
using Random;
using ScriptableObjects;
using UI;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class CoreSceneInstallers : MonoInstaller
    {
        public static Context Context { get; private set; }

        [SerializeField] private Context _context;
        [SerializeField] private string _nameDominoSpawnerPath;
        [SerializeField] private string _nameLevelControllerPath;
        [SerializeField] private string _nameGameScreenPath;
        [SerializeField] private string _nameStartScreenPath;
        [SerializeField] private string _nameColumnSpawnerPath;
        [SerializeField] private DominoPlacement _dominoPlacement;
        [SerializeField] private DominoHalfSource _dominoHalfSource;
        [SerializeField] private ColumnPlacement _сolumnPlacement;
        [SerializeField] private Canvas _canvas;
        
        public override async void InstallBindings()
        {
            BindColumnPlacement();
            BindDominoSource();
            BindRandomizer();
            BindDominoPlacement();
            BindColumnSpawner();
            await BindDominoSpawner();
            BindGameScreen();
            await BindLevelController();
            BindStartScreen();

            Context = _context;
        }

        private void BindColumnPlacement()
        {
            BindObjectAsSingle(_сolumnPlacement);
        }
        
        private void BindDominoSource()
        {
            BindObjectAsSingle(_dominoHalfSource);
        }
        
        private async void BindColumnSpawner()
        {
            var loadRequest = Resources.LoadAsync<ColumnSpawner>(_nameColumnSpawnerPath);

            await UniTask.WaitUntil(() => loadRequest.isDone);
            
            ColumnSpawner columnSpawner = Container
                .InstantiatePrefabForComponent<ColumnSpawner>(loadRequest.asset);
            
            BindObjectAsSingle(columnSpawner);
        }
        
        private async void BindStartScreen()
        {
            var loadRequest = Resources.LoadAsync<StartScreen>(_nameStartScreenPath);

            await UniTask.WaitUntil(() => loadRequest.isDone);
            
            StartScreen startScreen = Container
                .InstantiatePrefabForComponent<StartScreen>(loadRequest.asset, _canvas.transform);
            
            BindObjectAsSingle(startScreen);
        }

        private async void BindGameScreen()
        {
            var loadRequest = Resources.LoadAsync<GameScreen>(_nameGameScreenPath);

            await UniTask.WaitUntil(() => loadRequest.isDone);
            
            GameScreen gameScreen = Container
                .InstantiatePrefabForComponent<GameScreen>(loadRequest.asset, _canvas.transform);
            
            BindObjectAsSingle(gameScreen);
        }
        
        private void BindRandomizer()
        {
            Container
                .BindInterfacesTo<Randomizer>().AsSingle();
        }

        private void BindDominoPlacement()
        {
            BindObjectAsSingle(_dominoPlacement);
        }
        
        private async Task BindDominoSpawner()
        {
            var loadRequest = Resources.LoadAsync<DominoSpawner>(_nameDominoSpawnerPath);

            await UniTask.WaitUntil(() => loadRequest.isDone);
            
            DominoSpawner dominoSpawner = Container
                .InstantiatePrefabForComponent<DominoSpawner>(loadRequest.asset);

            BindObjectAsSingle(dominoSpawner);
        }
        
        private async Task BindLevelController()
        {
            var loadRequest = Resources.LoadAsync<LevelController>(_nameLevelControllerPath);

            await UniTask.WaitUntil(() => loadRequest.isDone);
            
            LevelController levelController = Container
                .InstantiatePrefabForComponent<LevelController>(loadRequest.asset);
            
            BindObjectAsSingle(levelController);
        }

        private void BindObjectAsSingle<T>(T obj)
        {
            Container
                .Bind<T>()
                .FromInstance(obj)
                .AsSingle();
        }
    }
}
