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
        private Subject<CallBack> listeners = new Subject<CallBack>();

        public IObservable<CallBack> Trigger => listeners;
    
        public void OnDrag(PointerEventData eventData) => listeners.OnNext(new CallBack(KeysStorage.Drag, eventData));
    
        public void OnBeginDrag(PointerEventData eventData) => listeners.OnNext(new CallBack(KeysStorage.BeginDrag, eventData));

        public void OnEndDrag(PointerEventData eventData) => listeners.OnNext(new CallBack(KeysStorage.EndDrag, eventData));
    }
}