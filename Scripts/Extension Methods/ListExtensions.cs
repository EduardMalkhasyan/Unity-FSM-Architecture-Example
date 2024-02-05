using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

namespace BugiGames.ExtensionMethod
{
    public static class ListExtensions
    {
        public static void LogContents<T>(this List<T> list, string text = null)
        {
            string json = JsonConvert.SerializeObject(list, Formatting.Indented);
            Debug.Log($"{text}. List contents: {json}");
        }

        public static void LogWarningContents<T>(this List<T> list, string text = null)
        {
            string json = JsonConvert.SerializeObject(list, Formatting.Indented);
            Debug.LogWarning($"{text}. List contents: {json}");
        }

        public static void LogErrorContents<T>(this List<T> list, string text = null)
        {
            string json = JsonConvert.SerializeObject(list, Formatting.Indented);
            Debug.LogError($"{text}. List contents: {json}");
        }

        public static List<Transform> GetAllChildrenExcludingParent(this Transform parent)
        {
            List<Transform> children = new List<Transform>();

            for (int i = 0; i < parent.childCount; i++)
            {
                children.Add(parent.GetChild(i));
            }

            return children;
        }

        /// <summary>
        /// Swaps the elements at the specified indices in the list.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="list">The list in which to perform the swap.</param>
        /// <param name="indexA">The index of the first element to swap.</param>
        /// <param name="indexB">The index of the second element to swap.</param>
        /// <returns>The modified list after swapping the elements at the specified indices.</returns>
        public static IList<T> Swap<T>(this IList<T> list, int indexA, int indexB)
        {
            T temp = list[indexA];
            list[indexA] = list[indexB];
            list[indexB] = temp;
            return list;
        }

        /// <summary>
        /// Adds an element to the list only if it is not already present.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="list">The list to which the element is to be added.</param>
        /// <param name="item">The element to add to the list.</param>
        /// <returns>True if the element was added, false if it already exists in the list.</returns>
        public static bool TryAdd<T>(this List<T> list, T item)
        {
            if (list.Contains(item) == false)
            {
                list.Add(item);
                return true;
            }
            return false;
        }
    }
}
