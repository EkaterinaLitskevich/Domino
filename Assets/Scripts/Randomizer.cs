using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class Randomizer : MonoBehaviour
{
    public int GetRandomValue(int minValue, int maxValue)
    {
        Random random = new Random();
        int value = random.Next(minValue, maxValue + 1);
        return value;
    }
}
