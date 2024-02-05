using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

namespace BugiGames.ExtensionMethod
{
    public static class NumberExtensions
    {
        public static int NonRepeatingRandom(this System.Random random, int minValue, int maxValue, ref int lastRandomValue)
        {
            int randomValue;
            do
            {
                randomValue = random.Next(minValue, maxValue + 1);
            } while (randomValue == lastRandomValue);

            lastRandomValue = randomValue;
            return randomValue;
        }

        public static int ToIntAndMultiplyBy1000(this float value)
        {
            return (int)(value * 1000);
        }
    }
}
