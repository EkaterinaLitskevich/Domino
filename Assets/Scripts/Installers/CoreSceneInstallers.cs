using System.Threading.Tasks;
using Controllers;
using Cysharp.Threading.Tasks;
using DefaultNamespace;
using Domino;
using UnityEngine;
using Update;
using Zenject;

namespace Installers
{
    public class CoreSceneInstallers : MonoInstaller
    {
        [SerializeField] private string _nameDominoSpawnerPath;
        [SerializeField] private string _nameLevelControllerPath;
        [SerializeField] private string _nameUpdaterPath;
        [SerializeField] private DominoPlacement _dominoPlacement;
        [SerializeField] private Canvas _canvas;
        public override async void InstallBindings()
        {
            BindRandomizer();
            BindDominoPlacement();
            await BindUpdater();
            await BindDominoSpawner();

            BindLevelController();
        }

        private async Task BindUpdater()
        {
            var loadRequest = Resources.LoadAsync<Updater>(_nameUpdaterPath);

            await UniTask.WaitUntil(() => loadRequest.isDone);
            
            Updater updater = Container
                .InstantiatePrefabForComponent<Updater>(loadRequest.asset);

            BindObject(updater);
        }
        
        private void BindRandomizer()
        {
            Container
                .BindInterfacesAndSelfTo<Randomizer>().AsSingle();
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
