using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;
using Random = System.Random;

public class Half : MonoBehaviour
{
    private const int MaxValue = 6;
    private const int MinValue = 0;
    
    [SerializeField] private Image _image;
    
    private int _value;

    public void Initial()
    {   
        SetValue();
        SetImage();
    }

    private void SetValue()
    {
        Random random = new Random();

        _value = random.Next(MinValue, MaxValue + 1);
    }

    private void SetImage()
    {
    }
}
