using Cysharp.Threading.Tasks;
using Domino;
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
        public override void InstallBindings()
        {
            BindDominoPlacement();
            BindDominoSpawner();
            BindLevelController();
        }

        private void BindDominoPlacement()
        {
            BindObject(_dominoPlacement);
        }
        
        private async void BindDominoSpawner()
        {
            //LoadFromResources<LevelController>(_nameDominoSpawnerPath, _canvas.transform);
            
            var loadRequest = Resources.LoadAsync<DominoSpawner>(_nameDominoSpawnerPath);

            await UniTask.WaitUntil(() => loadRequest.isDone);
            
            DominoSpawner dominoSpawner = Container
                .InstantiatePrefabForComponent<DominoSpawner>(loadRequest.asset, _canvas.transform);

            BindObject(dominoSpawner);
        }
        
        private async void BindLevelController()
        {
            //LoadFromResources<LevelController>(_nameLevelControllerPath, null);
            
            var loadRequest = Resources.LoadAsync<LevelController>(_nameLevelControllerPath);

            await UniTask.WaitUntil(() => loadRequest.isDone);
            
            LevelController levelController = Container
                .InstantiatePrefabForComponent<LevelController>(loadRequest.asset);
            
            BindObject(levelController);
        }

        /*private async void LoadFromResources<T>(string objectPathName, Transform parent)
        {
            var loadRequest = Resources.LoadAsync<LevelController>(objectPathName);

            await UniTask.WaitUntil(() => loadRequest.isDone);
            
            T obj = Container
                .InstantiatePrefabForComponent<T>(loadRequest.asset, parent);
        }*/

        private void BindObject<T>(T obj)
        {
            Container
                .Bind<T>()
                .FromInstance(obj)
                .AsSingle();
        }
    }
}
