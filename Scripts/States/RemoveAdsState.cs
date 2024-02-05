using BugiGames.Ads;
using BugiGames.IAP;
using BugiGames.ScriptableObject;
using BugiGames.StateMachine;
using BugiGames.UI;
using BugiGames.UI.Widget;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace BugiGames.GameState
{
    public class RemoveAdsState : MainAbstractGameState
    {
        [Inject] private UIScreensController screensController;
        [Inject] private MainGameStates mainGameStates;
        [Inject] private RemoveAdsWidget removeAdsWidget;
        [Inject] private AdsController adsController;
        [Inject] private PurchaseManager purchaseManager;
        [Inject] private InitializeGamingServices initializeGamingServices;
        [Inject] private MainMenuState mainMenuState;
        [Inject] private WaitLoadingSpinnerDots waitLoadingSpinnerDots;

        public override void Enter()
        {
            screensController.ShowUIScreen(UIScreenEnum.RemoveAds);
            removeAdsWidget.OnGoBack += EnterMainMenu;

            if (IAPData.Value.IsAdsPurchased() == false)
            {

                if (initializeGamingServices.IsSuccessInitedGamingServices == false)
                {
                    initializeGamingServices.Initialize();
                }

                if (purchaseManager.IsSuccessInitedPurchases == false)
                {
                    purchaseManager.InitializePurchasing();
                }

                removeAdsWidget.OnRemoveAdsPurchase += RemoveAdsPurchase;
                removeAdsWidget.OnRemoveAdsPurchaseRestore += RemoveAdsPurchaseRestore;

                purchaseManager.OnSuccessPurchasedAds += SuccessPurchaseAds;
                purchaseManager.OnFailPurchasedAds += FailPurchaseAds;

                purchaseManager.OnSuccessRestorePurchasedAds += SuccessPurchaseAds;
                purchaseManager.OnFailRestorePurchasedAds += FailPurchaseAds;

                TryGetProductPrice();
            }
            else
            {
                removeAdsWidget.PurchasedAdsStatus();
            }
        }

        public override void Exit()
        {
            removeAdsWidget.OnGoBack -= EnterMainMenu;

            if (IAPData.Value.IsAdsPurchased() == false)
            {
                removeAdsWidget.OnRemoveAdsPurchase -= RemoveAdsPurchase;
                removeAdsWidget.OnRemoveAdsPurchaseRestore -= RemoveAdsPurchaseRestore;

                purchaseManager.OnSuccessPurchasedAds -= SuccessPurchaseAds;
                purchaseManager.OnFailPurchasedAds -= FailPurchaseAds;

                purchaseManager.OnSuccessRestorePurchasedAds -= SuccessPurchaseAds;
                purchaseManager.OnFailRestorePurchasedAds -= FailPurchaseAds;
            }
        }

        private async void TryGetProductPrice()
        {
            await UniTask.WaitUntil(() => purchaseManager.IsStoreControllerInited());
            removeAdsWidget.SetPurchaseAdsText(purchaseManager.GetProductPriceInDollarsForRemoveAdsPurchase());
        }

        private void EnterMainMenu()
        {
            mainGameStates.EnterState<MainMenuState>();
        }

        private void RemoveAdsPurchase()
        {
            if (purchaseManager.IsStoreControllerInited() == false)
            {
                Debug.LogWarning("Cannot purchase. Store controller is not initialized yet.");
                return;
            }
            else
            {
                waitLoadingSpinnerDots.EnableLoadingScreen();
                purchaseManager.TryBuyRemoveAds();
            }
        }

        private void RemoveAdsPurchaseRestore()
        {
            waitLoadingSpinnerDots.EnableLoadingScreen();
            purchaseManager.RestorePurchases();
        }

        private void SuccessPurchaseAds()
        {
            waitLoadingSpinnerDots.DisableLoadingScreen();
            adsController.HideBannerBottom();
            IAPData.Value.StoreAdsPurchaseStatus();
            removeAdsWidget.PurchasedAdsStatus();
        }

        private void FailPurchaseAds()
        {
            waitLoadingSpinnerDots.DisableLoadingScreen();
            Debug.LogError("Fail purchase ads");
        }
    }
}
