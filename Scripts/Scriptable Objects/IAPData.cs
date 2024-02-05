using BugiGames.Tools;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BugiGames.ScriptableObject
{
    public enum IAPTypes
    {
        RemoveAds,
        BuyCoins100
    }

    public class IAPData : SOLoader<IAPData>
    {
        [SerializeField] private SerializableDictionary<IAPTypes, string> iAPValues;

        private const string isAdsPurchasedKey = "isAdsPurchasedKey";
        [ShowInInspector, ReadOnly] private bool IsAdsPurchasedInspector => IsAdsPurchased();

        public string GetIAPValue(IAPTypes iAPTypes)
        {
            return iAPValues.dictionary[iAPTypes];
        }

        [Button]
        public void StoreAdsPurchaseStatus()
        {
            bool isAdsPurchasedValue = true;
            PlayerPrefs.SetInt(isAdsPurchasedKey, isAdsPurchasedValue ? 1 : 0);

            Debug.Log($"Ads Purchase Stored! isAdsPurchasedValue: {isAdsPurchasedValue}," +
                      $" {isAdsPurchasedKey} of key value is {PlayerPrefs.GetInt(isAdsPurchasedKey)}");
        }

        public bool IsAdsPurchased()
        {
            return PlayerPrefs.GetInt(isAdsPurchasedKey, 0) == 1;
        }

        [Button]
        public void ResetIAPData()
        {
            bool isAdsPurchasedValue = false;
            PlayerPrefs.SetInt(isAdsPurchasedKey, isAdsPurchasedValue ? 1 : 0);
            Debug.Log($"Ads Purchase Reset! isAdsPurchasedValue: {isAdsPurchasedValue}," +
                      $" {isAdsPurchasedKey} of key value is {PlayerPrefs.GetInt(isAdsPurchasedKey)}");

            DebugColor.LogViolet($"Reset data: {this.name}");
        }
    }
}
