using System;
using Domino;
using UniRx;
using UnityEngine;
using Zenject;

namespace Controllers
{
    public class LevelController : MonoBehaviour
    {
        [Inject] private DominoSpawner _dominoSpawner;

        private void Start()
        {
            Observable.EveryUpdate().Subscribe(_ =>
            {
                UpdateManual(); ;
            });
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
