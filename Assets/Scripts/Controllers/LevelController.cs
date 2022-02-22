using Domino;
using UniRx;
using UnityEngine;
using Zenject;

namespace Controllers
{
    public class LevelController : MonoBehaviour
    {
        [Inject] private DominoSpawner _dominoSpawner;

        public void Initialize()
        {
            _dominoSpawner.CreateStartDomino();
        }
    }
}
