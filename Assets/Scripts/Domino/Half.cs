using TMPro;
using UnityEngine;
using Zenject;
using Image = UnityEngine.UI.Image;

public class Half : MonoBehaviour
{
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _text;
    
    [Inject] private DominoHalfSource _dominoHalfSource;
    
    private int _value;
    
    public int Value => _value;
    public Image Image => _image;
    public Vector2 SizeDelta
    {
        get => _rectTransform.sizeDelta;
    }

    public void SetValue(int value)
    {
        _text.text = string.Format("{0}", value);
        
        _value = value;
        if (_dominoHalfSource.GetImage(value) != null)
        {
            //_image = _dominoHalfSource.GetImage(value);
        }
    }
}
