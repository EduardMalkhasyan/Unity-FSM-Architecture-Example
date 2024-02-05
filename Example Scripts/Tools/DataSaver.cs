using Newtonsoft.Json;
using UnityEngine;

namespace BugiGames.Tools
{
    public static class DataSaver
    {
        public static void SaveAsJSON<TValue>(TValue value)
        {
            var dataKey = $"{typeof(TValue)}";

            string json = JsonConvert.SerializeObject(value, Formatting.Indented);
            PlayerPrefs.SetString($"{dataKey}", json);
            Debug.Log($"Saved from key: {dataKey}, data: {json}");
        }

        public static TValue LoadAsJSON<TValue>()
        {
            string data = null;

            var dataKey = $"{typeof(TValue)}";

            if (PlayerPrefs.HasKey(dataKey))
            {
                data = PlayerPrefs.GetString(dataKey);
                Debug.Log($"Loaded from key: {dataKey}, data: {data}");
            }
            else
            {
                Debug.LogWarning($"Data of key: {dataKey} dosent exist");
            }

            return JsonConvert.DeserializeObject<TValue>(data);
        }

        public static TValue TryLoadAsJSON<TValue>(out bool isAnyData)
        {
            string data = null;
            var dataKey = $"{typeof(TValue)}";

            if (PlayerPrefs.HasKey($"{dataKey}"))
            {
                data = PlayerPrefs.GetString($"{dataKey}");
                Debug.Log($"Loaded from key: {dataKey}, data: {data}");
                isAnyData = true;
                return JsonConvert.DeserializeObject<TValue>(data);
            }
            else
            {
                Debug.LogWarning($"Data of key: {dataKey} dosent exist");
                isAnyData = false;
                return default;
            }
        }

        public static void DeleteAsJSON<TValue>()
        {
            var key = $"{typeof(TValue)}";

            if (PlayerPrefs.HasKey(key))
            {
                PlayerPrefs.DeleteKey(key);
                Debug.Log($"Deleted data of key: {key}");
            }
            else
            {
                Debug.LogWarning($"Data of key: {key} dosent exist");
            }
        }

        public static void DeleteAll()
        {
            PlayerPrefs.DeleteAll();
            Debug.Log($"All data deleted");
        }
    }
}
