using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Random
{
    public interface IRandom
    {
        public int GetRandomValue(int minValue, int maxValue);
    }

    public class Randomizer : IRandom
    {
        public int GetRandomValue(int minValue, int maxValue)
        {
            System.Random random = new System.Random();
            int value = random.Next(minValue, maxValue + 1);
            return value;
        }
    }
}