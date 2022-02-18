namespace Random
{
    public interface IRandom
    {
        public int GetRandomValue(int minValue, int maxValue);
    }

    public class Randomizer : IRandom
    {
        private System.Random _random = new System.Random();
        public int GetRandomValue(int minValue, int maxValue)
        {
            int value = _random.Next(minValue, maxValue + 1);
            return value;
        }
    }
}