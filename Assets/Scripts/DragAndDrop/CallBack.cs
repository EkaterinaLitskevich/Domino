using UnityEngine.EventSystems;

namespace DragAndDrop
{
    public class CallBack
    {
        public string Key;
        public PointerEventData PointerData;

        public CallBack(string key, PointerEventData data)
        {
            Key = key;
            PointerData = data;
        }
    }
}