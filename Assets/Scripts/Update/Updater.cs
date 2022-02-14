using System.Collections.Generic;
using UnityEngine;

namespace Update
{
    public class Updater : MonoBehaviour
    {
        private List<IUpdate> _updates = new List<IUpdate>();

        public void AddToList(IUpdate update)
        {
            _updates.Add(update);
        }
    
        private void Update()
        {
            for (int i = 0; i < _updates.Count; i++)
            {
                if (_updates[i] != null)
                {
                    _updates[i].UpdateManual();
                }
            }
        }
    }
}
