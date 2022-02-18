using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "DominoHalf", menuName = "Source/DominoHalf")]
public class DominoHalfSource : SerializedScriptableObject
{
    public Dictionary<int, Image> _halfs = new Dictionary<int, Image>();

    public Image GetImage(int value)
    {
        if (_halfs[value] != null)
        {
            return _halfs[value];
        }

        return null;
    }
}
