
namespace Domino
{
    public class CallBackDominoPlacement 
    {
        public string Key;
        public DominoController DominoControllerGame;
        public DominoController DominoControllerStand;
        
        public CallBackDominoPlacement(string key, DominoController dominoControllerGame, DominoController dominoControllerStand)
        {
            Key = key;
            DominoControllerGame = dominoControllerGame;
            DominoControllerStand = dominoControllerStand;
        }
    }
}
