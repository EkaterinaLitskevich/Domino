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

    
    [SerializeField] private Image _image;
    
    private int _value;

    public void Initial()
    {   
        SetValue();
        SetImage();
    }

    private void SetValue()
    {
        
    }

    private void SetImage()
    {
    }
}
