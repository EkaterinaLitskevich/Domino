using System.Threading.Tasks;
using Controllers;
using Cysharp.Threading.Tasks;
using Domino;
using Random;
using UI;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class CoreSceneInstallers : MonoInstaller
    {
        [SerializeField] private string _nameDominoSpawnerPath;
        [SerializeField] private string _nameLevelControllerPath;
        [SerializeField] private string _nameGameScreenPath;
        [SerializeField] private DominoPlacement _dominoPlacement;
        [SerializeField] private Canvas _canvas;
        
        public override async void InstallBindings()
        {
            BindRandomizer();
            BindDominoPlacement();
            await BindDominoSpawner();
            BindGameScreen();
            BindLevelController();
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
                .InstantiatePrefabForComponent<DominoSpawner>(loadRequest.asset, _canvas.transform);

            BindObjectAsSingle(dominoSpawner);
        }
        
        private async void BindLevelController()
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
