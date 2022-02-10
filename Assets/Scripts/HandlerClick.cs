using System;
using System.Collections;
using System.Collections.Generic;
using Domino;
using UnityEngine;
using UnityEngine.EventSystems;

public class HandlerClick : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Vector2.Lerp(transform.position, eventData.position, 1f);
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("OnBeginDrag");
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEndDrag");
    }
}
