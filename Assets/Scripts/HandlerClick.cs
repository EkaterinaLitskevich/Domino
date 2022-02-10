using System;
using System.Collections;
using System.Collections.Generic;
using Domino;
using UnityEngine;
using UnityEngine.EventSystems;

public class HandlerClick : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public event Action<PointerEventData> OnDragObj;
    public event Action<PointerEventData> OnBeginDragObj;
    public event Action<PointerEventData> OnEndDragObj;

    public void OnDrag(PointerEventData eventData)
    {
        OnDragObj?.Invoke(eventData);
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        OnBeginDragObj?.Invoke(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        OnEndDragObj?.Invoke(eventData);
    }
}
