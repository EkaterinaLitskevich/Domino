using System;
using System.Collections;
using System.Collections.Generic;
using Domino;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DragAndDrop
{
    public class HandlerClick : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IClickHandler
    {
        private Subject<CallBackDrag> listeners = new Subject<CallBackDrag>();

        public IObservable<CallBackDrag> Trigger => listeners;
    
        public void OnDrag(PointerEventData eventData) => listeners.OnNext(new CallBackDrag(KeysStorage.Drag, eventData));
    
        public void OnBeginDrag(PointerEventData eventData) => listeners.OnNext(new CallBackDrag(KeysStorage.BeginDrag, eventData));

        public void OnEndDrag(PointerEventData eventData) => listeners.OnNext(new CallBackDrag(KeysStorage.EndDrag, eventData));
    }
}