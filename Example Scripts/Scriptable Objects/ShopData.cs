using BugiGames.Tools;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using BugiGames.ExtensionMethod;
using System.Collections.ObjectModel;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace BugiGames.ScriptableObject
{
    public class ShopData : SOLoader<ShopData>
    {
        [FolderPath(AbsolutePath = false)]
        [SerializeField] private string shopItemsFolderPath;

        [SerializeField] public List<ShopItemForm> shopItemForms;
        public ReadOnlyCollection<ShopItemForm> ShopItemForms => shopItemForms.AsReadOnly();

        [field: SerializeReference] public ShopItemForm CurrentShopItemForm { get; set; }

        [Button]
        private void SaveFromShopItemForms()
        {
#if UNITY_EDITOR
            ShopDataTemp.ShopItemFormsTemp = shopItemForms;
            DataSaver.SaveAsJSON<List<ShopItemForm>>(ShopDataTemp.ShopItemFormsTemp);
#endif
        }

        private void Save()
        {
            DataSaver.SaveAsJSON<List<ShopItemForm>>(ShopDataTemp.ShopItemFormsTemp);
        }

        public void ChangeAndSave(ShopItemForm shopItemForm)
        {
#if UNITY_EDITOR
            for (int i = 0; i < ShopItemForms.Count; i++)
            {
                if (ShopItemForms[i].path == shopItemForm.path)
                {
                    ShopItemForms[i].isSelected = shopItemForm.isSelected;
                    ShopItemForms[i].isBought = shopItemForm.isBought;

                    CurrentShopItemForm = ShopItemForms[i];
                }
                else
                {
                    ShopItemForms[i].isSelected = false;
                }
            }
#endif

            for (int i = 0; i < ShopDataTemp.ShopItemFormsTemp.Count; i++)
            {
                if (ShopDataTemp.ShopItemFormsTemp[i].path == shopItemForm.path)
                {
                    ShopDataTemp.ShopItemFormsTemp[i].isSelected = shopItemForm.isSelected;
                    ShopDataTemp.ShopItemFormsTemp[i].isBought = shopItemForm.isBought;

                    ShopDataTemp.CurrentShopItemFormTemp = ShopDataTemp.ShopItemFormsTemp[i];
                }
                else
                {
                    ShopDataTemp.ShopItemFormsTemp[i].isSelected = false;
                }
            }

            Save();
        }

        [Button]
        public void Load()
        {
            var loadedData = DataSaver.TryLoadAsJSON<List<ShopItemForm>>(out bool isAnyData);

            if (isAnyData == false)
            {
                loadedData = shopItemForms;
                ShopDataTemp.ShopItemFormsTemp = shopItemForms;
                DataSaver.SaveAsJSON<List<ShopItemForm>>(ShopDataTemp.ShopItemFormsTemp);
                DefaultShopItems();
            }
            else
            {
                ShopDataTemp.ShopItemFormsTemp = loadedData;
                ShopDataTemp.CurrentShopItemFormTemp = ShopDataTemp.ShopItemFormsTemp.Find(itemForm => itemForm.isSelected == true
                                                                                                    && itemForm.isBought == true);
                if (ShopItemForms.Count > ShopDataTemp.ShopItemFormsTemp.Count)
                {
                    foreach (ShopItemForm itemForm in ShopItemForms)
                    {
                        if (ShopDataTemp.ShopItemFormsTemp.Any(itemFormTemp => itemFormTemp.path == itemForm.path) == false)
                        {
                            ShopDataTemp.ShopItemFormsTemp.Add(itemForm);
                        }
                    }
                }
                else if (ShopItemForms.Count < ShopDataTemp.ShopItemFormsTemp.Count)
                {
                    ShopDataTemp.ShopItemFormsTemp = ShopDataTemp.ShopItemFormsTemp.Intersect(ShopItemForms, new ShopItemEqualityComparer()).ToList();
                    ShopDataTemp.ShopItemFormsTemp.LogContents($"New recreated list of {ShopDataTemp.ShopItemFormsTemp}");
                }
            }

#if UNITY_EDITOR
            for (int i = 0; i < ShopDataTemp.ShopItemFormsTemp.Count; i++)
            {
                if (ShopItemForms[i].path == ShopDataTemp.ShopItemFormsTemp[i].path)
                {
                    ShopItemForms[i].isBought = ShopDataTemp.ShopItemFormsTemp[i].isBought;
                    ShopItemForms[i].isSelected = ShopDataTemp.ShopItemFormsTemp[i].isSelected;
                }
            }

            CurrentShopItemForm = ShopDataTemp.CurrentShopItemFormTemp;
#endif

            CheckIfAnyItemIsSelectedOrSelectDefault();
        }

        public void CheckIfAnyItemIsSelectedOrSelectDefault()
        {
            if (ShopDataTemp.ShopItemFormsTemp.Any(form => form.isSelected == true) == false)
            {
                ChoseFirstItem();
                Save();
            }
        }

        public void BuyAndSave(ShopItemForm shopItemFormToBuy)
        {
#if UNITY_EDITOR
            shopItemForms.Find(itemForm => itemForm.path == shopItemFormToBuy.path).isBought = true;
#endif

            ShopDataTemp.ShopItemFormsTemp.Find(itemForm => itemForm.path == shopItemFormToBuy.path).isBought = true;
            shopItemFormToBuy.isBought = true;

            Save();
        }

        [Button]
        public void ResetShopData()
        {
            DefaultShopItems();
            DataSaver.DeleteAsJSON<List<ShopItemForm>>();
            DebugColor.LogViolet($"Reset data: {this.name}");
        }

        private void DefaultShopItems()
        {
            for (int i = 0; i < ShopItemForms.Count; i++)
            {
                ShopItemForms[i].isBought = false;
                ShopItemForms[i].isSelected = false;
            }

            ShopDataTemp.ShopItemFormsTemp = shopItemForms;

            ChoseFirstItem();
        }

        private void ChoseFirstItem()
        {
#if UNITY_EDITOR
            var firstItem = ShopItemForms.First();
            firstItem.isBought = true;
            firstItem.isSelected = true;

            CurrentShopItemForm = firstItem;
#endif

            var firstItemTemp = ShopDataTemp.ShopItemFormsTemp.First();
            firstItemTemp.isBought = true;
            firstItemTemp.isSelected = true;

            ShopDataTemp.CurrentShopItemFormTemp = firstItemTemp;
        }

        public void CheckIfShopItemsIsRight()
        {
#if UNITY_EDITOR
            string[] guids = AssetDatabase.FindAssets("t:Prefab", new[] { shopItemsFolderPath });

            if (guids.Length != ShopItemForms.Count)
            {
                Debug.LogError($"ShopItemsCount: {ShopItemForms.Count}," +
                               $" must be same with prefab shop items count: {guids.Length} in folder");
            }
#endif
        }
    }

    [Serializable]
    public class ShopItemForm
    {
        public string path;
        public int price;
        public bool isBought;
        public bool isSelected;

        [JsonIgnore] public Sprite sprite;
    }

    internal static class ShopDataTemp
    {
        internal static List<ShopItemForm> ShopItemFormsTemp { get; set; }

        internal static ShopItemForm CurrentShopItemFormTemp { get; set; }
    }

    public class ShopItemEqualityComparer : IEqualityComparer<ShopItemForm>
    {
        public bool Equals(ShopItemForm x, ShopItemForm y)
        {
            return x.path == y.path;
        }

        public int GetHashCode(ShopItemForm obj)
        {
            return obj.path.GetHashCode();
        }
    }
}
