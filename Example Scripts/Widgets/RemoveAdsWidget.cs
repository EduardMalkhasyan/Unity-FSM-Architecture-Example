using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BugiGames.UI.Widget
{
    public class RemoveAdsWidget : MonoBehaviour
    {
        [SerializeField] private Button backButton;
        [SerializeField] private Button removeAdsPurchaseButton;
        [SerializeField] private Button removeAdsPurchaseRestoreButton;

        [SerializeField] private TextMeshProUGUI purchaseAdsText;

        public event Action OnGoBack;
        public event Action OnRemoveAdsPurchase;
        public event Action OnRemoveAdsPurchaseRestore;

        private void OnEnable()
        {
            backButton.onClick.AddListener(() => OnGoBack.Invoke());
            removeAdsPurchaseButton.onClick.AddListener(() => OnRemoveAdsPurchase.Invoke());
            removeAdsPurchaseRestoreButton.onClick.AddListener(() => OnRemoveAdsPurchaseRestore.Invoke());
        }

        private void OnDisable()
        {
            backButton.onClick.RemoveAllListeners();
            removeAdsPurchaseButton.onClick.RemoveAllListeners();
            removeAdsPurchaseRestoreButton.onClick.RemoveAllListeners();
        }

        public void PurchasedAdsStatus()
        {
            removeAdsPurchaseButton.interactable = false;
            purchaseAdsText.text = "Purchased";
        }

        public void SetPurchaseAdsText(string text)
        {
            purchaseAdsText.text = text;
        }
    }
}
