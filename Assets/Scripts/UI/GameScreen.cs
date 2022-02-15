using System;
using Domino;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GameScreen : MonoBehaviour
{
    [SerializeField] private Button _rotateButton;

    private CompositeDisposable _disposable = new CompositeDisposable();
    
    [Inject] private DominoSpawner _dominoSpawner;

    private void Start()
    {
        Debug.Log("2");
        _rotateButton.onClick.AsObservable().Subscribe(_ =>
        {
            _dominoSpawner.RotateDominoGame();
        }).AddTo(_disposable);
    }

    private void OnDisable()
    {
        _disposable.Dispose();
    }
}
