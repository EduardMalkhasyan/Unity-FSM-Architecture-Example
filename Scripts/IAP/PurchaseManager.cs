using BugiGames.ScriptableObject;
using BugiGames.Tools;
using System;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;

namespace BugiGames.IAP
{
    public class PurchaseManager : IDetailedStoreListener
    {
        private IStoreController storeController;

        private IGooglePlayStoreExtensions googlePlayStoreExtensions;
        private IAppleExtensions appleExtensions;

        public event Action OnSuccessPurchasedAds;
        public event Action OnFailPurchasedAds;

        public event Action OnSuccessRestorePurchasedAds;
        public event Action OnFailRestorePurchasedAds;

        public bool IsSuccessInitedPurchases { get; private set; }

        public void InitializePurchasing()
        {
            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

            if (Application.platform == RuntimePlatform.Android)
            {
                IDs androidIDs = new IDs
                {
                   {IAPData.Value.GetIAPValue(IAPTypes.RemoveAds), GooglePlay.Name },
                };

                builder.AddProduct(IAPData.Value.GetIAPValue(IAPTypes.RemoveAds), ProductType.NonConsumable, androidIDs);
            }
            else if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                IDs iosIDs = new IDs
                {
                   { IAPData.Value.GetIAPValue(IAPTypes.RemoveAds), AppleAppStore.Name },
                };

                builder.AddProduct(IAPData.Value.GetIAPValue(IAPTypes.RemoveAds), ProductType.NonConsumable, iosIDs);
            }
            else
            {
#if UNITY_EDITOR
                IDs androidIDs = new IDs
                {
                   {IAPData.Value.GetIAPValue(IAPTypes.RemoveAds), GooglePlay.Name },
                };

                builder.AddProduct(IAPData.Value.GetIAPValue(IAPTypes.RemoveAds), ProductType.NonConsumable, androidIDs);
#endif
            }

            UnityPurchasing.Initialize(this, builder);
        }

        public void TryBuyRemoveAds()
        {
            if (IsStoreControllerInited() == false)
            {
                Debug.LogWarning("Cannot purchase. Store controller is not initialized yet.");
                return;
            }

            if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
            {
                storeController.InitiatePurchase(IAPData.Value.GetIAPValue(IAPTypes.RemoveAds));
            }
            else
            {
#if UNITY_EDITOR
                storeController.InitiatePurchase(IAPData.Value.GetIAPValue(IAPTypes.RemoveAds));
#endif
            }
        }

        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            IsSuccessInitedPurchases = true;
            DebugColor.LogGreen("In-App Purchasing successfully initialized");
            storeController = controller;
            googlePlayStoreExtensions = extensions.GetExtension<IGooglePlayStoreExtensions>();
            appleExtensions = extensions.GetExtension<IAppleExtensions>();
        }

        public void OnInitializeFailed(InitializationFailureReason error)
        {
            IsSuccessInitedPurchases = false;
            OnInitializeFailed(error, null);
        }

        public void OnInitializeFailed(InitializationFailureReason error, string message)
        {
            var errorMessage = $"Purchasing failed to initialize. Reason: {error}.";

            if (message != null)
            {
                errorMessage += $" More details: {message}";
            }

            Debug.LogError(errorMessage);
        }

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
        {
            var product = args.purchasedProduct;

            if (product.definition.id == IAPData.Value.GetIAPValue(IAPTypes.RemoveAds))
            {
                OnSuccessPurchasedAds.Invoke();
            }

            DebugColor.LogGreen($"Purchase Complete - Product: {product.definition.id}");

            return PurchaseProcessingResult.Complete;
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            Debug.LogError($"Purchase failed - Product: '{product.definition.id}', PurchaseFailureReason: {failureReason}");
            OnFailPurchasedAds.Invoke();
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
        {
            Debug.LogError($"Purchase failed - Product: '{product.definition.id}'," +
                $" Purchase failure reason: {failureDescription.reason}," +
                $" Purchase failure details: {failureDescription.message}");

            OnFailPurchasedAds.Invoke();
        }

        public void RestorePurchases()
        {
            if (storeController == null)
            {
                Debug.LogWarning("Cannot restore purchases. Store controller is not initialized yet.");
                OnFailRestorePurchasedAds.Invoke();
                return;
            }

            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                appleExtensions.RestoreTransactions(OnRestore);
            }
            else if (Application.platform == RuntimePlatform.Android)
            {
                googlePlayStoreExtensions.RestoreTransactions(OnRestore);
            }
            else
            {
                Debug.LogWarning("Restore purchases is not supported on this platform.");
                OnFailRestorePurchasedAds.Invoke();
            }
        }

        private void OnRestore(bool success, string error)
        {
            var restoreMessage = "";
            if (success)
            {
                if (IsRemoveAdsPurchased())
                {
                    OnSuccessRestorePurchasedAds.Invoke();
                }
                else
                {
                    OnFailRestorePurchasedAds.Invoke();
                }

                restoreMessage = "Restore Successful";
            }
            else
            {
                OnFailRestorePurchasedAds.Invoke();
                restoreMessage = $"Restore Failed with error: {error}";
            }

            Debug.Log(restoreMessage);
        }

        public bool IsRemoveAdsPurchased()
        {
            if (IsStoreControllerInited() == false)
            {
                Debug.Log("Cannot check purchases. Store controller is not initialized yet.");
                return false;
            }

            Product product = null;

            if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
            {
                product = storeController.products.WithID(IAPData.Value.GetIAPValue(IAPTypes.RemoveAds));
            }
            else
            {
#if UNITY_EDITOR
                product = storeController.products.WithID(IAPData.Value.GetIAPValue(IAPTypes.RemoveAds));
#endif
            }

            if (product != null && product.hasReceipt)
            {
                DebugColor.LogGreen("Remove Ads product has been restored!");
                return true;
            }
            else
            {
                Debug.LogWarning("Remove Ads product has not been restored.");
            }

            return false;
        }

        public string GetProductPriceInDollarsForRemoveAdsPurchase()
        {
            string productId = null;

            if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
            {
                productId = IAPData.Value.GetIAPValue(IAPTypes.RemoveAds);
            }
            else
            {
#if UNITY_EDITOR
                return "Buy";
#endif
            }

            Product product = storeController.products.WithID(productId);

            if (product != null)
            {
                DebugColor.LogGreen($"Product found: {product.metadata.localizedTitle}" +
                                    $" - Price: {product.metadata.localizedPriceString}");

                return product.metadata.localizedPriceString;
            }
            else
            {
                Debug.LogWarning("Product not found.");
                return "Buy";
            }
        }

        public bool IsStoreControllerInited()
        {
            return storeController != null;
        }
    }
}
