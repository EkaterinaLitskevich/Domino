using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using Random = System.Random;

public class Randomizer : IRandom
{
    public int RandomValue(int minValue, int maxValue)
    {
        Random random = new Random();
        int value = random.Next(minValue, maxValue + 1);
        return value;
    }
}
