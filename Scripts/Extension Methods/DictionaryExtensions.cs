using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BugiGames.ExtensionMethod
{
    public static class DictionaryExtensions
    {
        public static bool IsEmptyOrNull<TKey, TValue>(this Dictionary<TKey, TValue> dictionary)
        {
            return dictionary == null || dictionary.Count == 0;
        }

        private static void PrintDictionary<TKey, TValue>(this Dictionary<TKey, TValue> dictionary)
        {
            foreach (var pair in dictionary)
            {
                Debug.Log($"Key: {pair.Key}, Value: {pair.Value}");
            }
        }
    }
}
