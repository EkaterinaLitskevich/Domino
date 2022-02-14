using System;

namespace DragAndDrop
{
    public interface IClickHandler
    {
        IObservable<CallBackDrag> Trigger { get; }
    }
}