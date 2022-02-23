using ScriptableObjects;
using UnityEngine;
using Zenject;

public class Half : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    //[SerializeField] private TextMeshProUGUI _text;
    
    [Inject] private DominoHalfSource _dominoHalfSource;
    
    private int _value;
    
    public int Value => _value;
    public Sprite Sprite => _spriteRenderer.sprite;

    public void SetValue(int value)
    {
        //_text.text = string.Format("{0}", value);
        
        _value = value;
        if (_dominoHalfSource.GetSprite(value) != null)
        {
            //_image = _dominoHalfSource.GetImage(value);
        }
    }
}
