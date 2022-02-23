using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "DominoHalf", menuName = "Source/DominoHalf")]
    public class DominoHalfSource : SerializedScriptableObject
    {
        public Dictionary<int, Sprite> _halfs = new Dictionary<int, Sprite>();

        public Sprite GetSprite(int value)
        {
            if (_halfs[value] != null)
            {
                return _halfs[value];
            }

            return null;
        }
    }
}
