using System;

namespace DragAndDrop
{
    public interface IClickHandler
    {
        IObservable<CallBack> Trigger { get; }
    }
}