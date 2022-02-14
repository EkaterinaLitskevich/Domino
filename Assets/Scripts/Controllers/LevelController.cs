using System;
using Domino;
using UnityEngine;
using Update;
using Zenject;

namespace Controllers
{
    public class LevelController : MonoBehaviour, IUpdate
    {
        [Inject] private DominoSpawner _dominoSpawner;
        [Inject] private Updater _updater;

        private void Start()
        {
            _updater.AddToList(this);
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
