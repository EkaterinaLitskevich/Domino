using UnityEngine.EventSystems;

namespace DragAndDrop
{
    public class CallBackDrag
    {
        public string Key;
        public PointerEventData PointerData;

        public CallBackDrag(string key, PointerEventData data)
        {
            Key = key;
            PointerData = data;
        }
    }
}