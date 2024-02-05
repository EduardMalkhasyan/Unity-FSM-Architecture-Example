using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;

namespace BugiGames.Tools
{
    [Serializable]
    public class SerializableDictionary<TKey, TValue> : ISerializationCallbackReceiver
    {
        private bool hasDuplicates;
        private string errorMessage;

        [InfoBox("$errorMessage", InfoMessageType.Error, VisibleIf = nameof(hasDuplicates))]
        [SerializeField] private List<KeyValueEntry> entries;

        private List<TKey> keys = new List<TKey>();

        public Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>();

        [Serializable]
        class KeyValueEntry
        {
            public TKey key;
            public TValue value;
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            dictionary.Clear();

            for (int i = 0; i < entries.Count; i++)
            {
                dictionary.Add(entries[i].key, entries[i].value);
            }
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            if (entries == null)
            {
                return;
            }

            keys.Clear();

            for (int i = 0; i < entries.Count; i++)
            {
                keys.Add(entries[i].key);
            }

            var result = keys.GroupBy(x => x)
                             .Where(g => g.Count() > 1)
                             .Select(x => new { Element = x.Key, Count = x.Count() })
                             .ToList();

            hasDuplicates = result.Count > 0;

            if (hasDuplicates)
            {
                var duplicates = string.Join(", ", result);
                errorMessage = $"Warning: {GetType().Name} keys have duplicates {duplicates}";
            }
        }
    }
}
