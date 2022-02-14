using UnityEngine;

namespace Domino
{
    public class CallBackDominoPlacement 
    {
        public string Key;
        public DominoController DominoController;
        
        public CallBackDominoPlacement(string key, DominoController dominoController)
        {
            Key = key;
            DominoController = dominoController;
        }
    }
}
