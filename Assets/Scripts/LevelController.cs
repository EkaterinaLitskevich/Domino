using System;
using System.Collections;
using System.Collections.Generic;
using Domino;
using UnityEngine;
using Zenject;

public class LevelController : MonoBehaviour
{
    [Inject] private DominoSpawner _dominoSpawner;

    private void Initialize()
    {
        _dominoSpawner.CreateStartDomino();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Initialize();
        }
    }
}
