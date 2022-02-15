using System.Threading.Tasks;
using Controllers;
using Cysharp.Threading.Tasks;
using Domino;
using Random;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class CoreSceneInstallers : MonoInstaller
    {
        [SerializeField] private string _nameDominoSpawnerPath;
        [SerializeField] private string _nameLevelControllerPath;
        [SerializeField] private DominoPlacement _dominoPlacement;
        [SerializeField] private Canvas _canvas;
        
        public override async void InstallBindings()
        {
            BindRandomizer();
            BindDominoPlacement();
            await BindDominoSpawner();

            BindLevelController();
        }
        
        private void BindRandomizer()
        {
            Container
                .BindInterfacesTo<Randomizer>().AsSingle();
        }

        private void BindDominoPlacement()
        {
            BindObject(_dominoPlacement);
        }
        
        private async Task BindDominoSpawner()
        {
            var loadRequest = Resources.LoadAsync<DominoSpawner>(_nameDominoSpawnerPath);

            await UniTask.WaitUntil(() => loadRequest.isDone);
            
            DominoSpawner dominoSpawner = Container
                .InstantiatePrefabForComponent<DominoSpawner>(loadRequest.asset, _canvas.transform);

            BindObject(dominoSpawner);
        }
        
        private async void BindLevelController()
        {
            var loadRequest = Resources.LoadAsync<LevelController>(_nameLevelControllerPath);

            await UniTask.WaitUntil(() => loadRequest.isDone);
            
            LevelController levelController = Container
                .InstantiatePrefabForComponent<LevelController>(loadRequest.asset);
            
            BindObject(levelController);
        }

        private void BindObject<T>(T obj)
        {
            Container
                .Bind<T>()
                .FromInstance(obj)
                .AsSingle();
        }
    }
}
