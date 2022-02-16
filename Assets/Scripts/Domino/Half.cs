using UnityEngine;
using Zenject;
using Image = UnityEngine.UI.Image;

public class Half : MonoBehaviour
{
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private Image _image;
    
    [Inject] private DominoHalfSource _dominoHalfSource;
    
    private int _value;
    public int Value => _value;

    public Vector2 SizeDelta
    {
        get => _rectTransform.sizeDelta;
    }

    public void SetValue(int value)
    {
        Debug.Log(_dominoHalfSource);
        
        _value = value;
        if (_dominoHalfSource.GetImage(value) != null)
        {
            _image = _dominoHalfSource.GetImage(value);
        }
    }
}
