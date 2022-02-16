using Domino;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class GameScreen : MonoBehaviour
    {
        [SerializeField] private Button _rotateButton;
        [SerializeField] private Button _shuffleButton;

        private CompositeDisposable _disposable = new CompositeDisposable();
    
        [Inject] private DominoSpawner _dominoSpawner;

        private void Start()
        {
            _rotateButton.onClick.AsObservable().Subscribe(_ =>
            {
                _dominoSpawner.RotateDominoGame();
            }).AddTo(_disposable);
            
            _shuffleButton.onClick.AsObservable().Subscribe(_ =>
            {
                _dominoSpawner.ShuffleDomino();
            }).AddTo(_disposable);
        }

        private void OnDisable()
        {
            _disposable.Dispose();
        }
    }
}
