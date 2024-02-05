using BugiGames.ScriptableObject;
using Sirenix.OdinInspector;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BugiGames.Window
{
    public class BuyShopItemWindow : MonoBehaviour, IUsable
    {
        [SerializeField] private Animator animator;

        [SerializeField] private Button noButton;
        [SerializeField] private Button yesButton;

        [SerializeField] private Image itemImage;
        [SerializeField] private TextMeshProUGUI itemPrice;

        private Vector3 originalPosition;

        private const string Open = "Open";
        private const string Close = "Close";

        public bool IsUsable { get; private set; }

        public event Action<ShopItemForm> OnBuyItem;

        [SerializeField] private ShopItemForm selectedShopItemForm;

        private void Awake()
        {
            originalPosition = transform.localPosition;

            if (IsUsable == false)
            {
                gameObject.SetActive(false);
            }
        }

        private void Update()
        {
            var clipInfo = animator.GetCurrentAnimatorClipInfo(0);
            var stateInfo = animator.GetCurrentAnimatorStateInfo(0);

            if (clipInfo.Length > 0)
            {
                var clip = clipInfo[0].clip;

                if (animator.IsInTransition(0) == false && clip.name == Close && stateInfo.normalizedTime >= 1f)
                {
                    gameObject.SetActive(false);
                    transform.localPosition = originalPosition;
                }
            }
        }

        private void OnEnable()
        {
            noButton.onClick.AddListener(CloseWindow);
            yesButton.onClick.AddListener(() => OnBuyItem.Invoke(selectedShopItemForm));
        }

        private void OnDisable()
        {
            noButton.onClick.RemoveAllListeners();
            yesButton.onClick.RemoveAllListeners();
        }

        [Button]
        public void CloseWindow()
        {
            animator.SetTrigger(Close);
        }

        [Button]
        public void OpenWindow()
        {
            transform.localPosition = Vector3.zero;

            IsUsable = true;
            gameObject.SetActive(true);
            animator.SetTrigger(Open);
        }

        public void Setup(ShopItemForm shopItemForm)
        {
            this.selectedShopItemForm = shopItemForm;

            itemImage.sprite = this.selectedShopItemForm.sprite;
            itemPrice.text = this.selectedShopItemForm.price.ToString();

            if (shopItemForm.price > GameCurrencyProps.Value.Coins)
            {
                yesButton.interactable = false;
            }
            else
            {
                yesButton.interactable = true;
            }
        }
    }
}

