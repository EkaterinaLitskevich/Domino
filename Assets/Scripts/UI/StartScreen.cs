using System;
using System.Collections;
using System.Collections.Generic;
using Controllers;
using UI;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class StartScreen : MonoBehaviour
{
    [SerializeField] private Button _startButton;
    
    private CompositeDisposable _disposable = new CompositeDisposable();
    
    [Inject] private LevelController _levelController;
    [Inject] private GameScreen _gameScreen;

    private void Start()
    {
        _startButton.onClick.AsObservable().Subscribe(_ =>
        {
            _gameScreen.gameObject.SetActive(true);
            _levelController.Initialize();
            transform.gameObject.SetActive(false);
        }).AddTo(_disposable);
    }
}
