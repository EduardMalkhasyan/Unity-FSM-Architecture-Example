using UnityEngine;
using UnityEngine.Purchasing;

namespace BugiGames.IAP
{
    public class UserWarningGooglePlayStore
    {
        public void UpdateWarningText()
        {
            var currentAppStore = StandardPurchasingModule.Instance().appStore;

            var warningMessage = currentAppStore != AppStore.GooglePlay ?
                "This sample is meant to be tested using the Google Play Store.\n" +
                $"The currently selected store is: {currentAppStore}.\n"
                : "";

            Debug.LogWarning(warningMessage);
        }
    }
}
