using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DominoController : MonoBehaviour
{
    [SerializeField] private int _amountHalfs;
    [SerializeField] private Half _halfPrefab;
    [SerializeField] private BoxCollider2D _collider;
    
    private List<Half> _halfs = new List<Half>();
    private List<Half> _halfForCompare = new List<Half>();

    private bool _isStand;

    private void Start()
    {
        FillArray();
        FillGrid();
        SetCollider();
    }

    private void FillArray()
    {
        Half half = new Half();
        
        for (int i = 0; i < _amountHalfs; i++)
        {
            half = Instantiate(_halfPrefab, transform.position, transform.rotation, null);
            _halfs.Add(half);
        }
    }

    private void FillGrid()
    {
        int halfArray = _amountHalfs / 2;

        for (int i = 0; i < halfArray; i++)
        {
            _halfs[i].transform.SetParent(transform);
            SetHalfsForCompare(true, i);
        }

        for (int i = _halfs.Count - 1; i >= halfArray; i--)
        {
            _halfs[i].transform.SetParent(transform);
            SetHalfsForCompare(false, i);
        }
    }

    private void SetHalfsForCompare(bool isStand, int i)
    {
        if (_isStand == isStand)
        {
            _halfForCompare.Add(_halfs[i]);
        }
    }

    private void SetCollider()
    {
        Vector2 sizeCollider = _collider.size;
        _collider.size = new Vector2(sizeCollider.x, sizeCollider.y * _amountHalfs);
    }
}
