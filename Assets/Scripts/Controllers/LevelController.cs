using System;
using Domino;
using UniRx;
using UnityEngine;
using Zenject;

namespace Controllers
{
    public class LevelController : MonoBehaviour
    {
        private CompositeDisposable _disposable = new CompositeDisposable();
        
        [Inject] private DominoSpawner _dominoSpawner;

        private void OnEnable()
        {
            Observable.EveryUpdate().Subscribe(_ =>
            {
                UpdateManual();
            }).AddTo(_disposable);
        }
        
        private void OnDisable()
        {
            _disposable.Dispose();
        }

        private void Initialize()
        {
            _dominoSpawner.CreateStartDomino();
        }

        public void UpdateManual()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                Initialize();
            }
        }
    }
}
