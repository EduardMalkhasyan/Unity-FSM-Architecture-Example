using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace BugiGames.ExtensionMethod
{
    public static class ForceRebuildLayoutEx
    {
        public static void RebuildLayout(this RectTransform rect, int repreatCount = 0, float interval = 0.1f)
        {
            RebuildAsync(rect, repreatCount, interval);
        }

        public static void RebuildLayoutAllChilds(this RectTransform rect, int repreatCount = 0, float interval = 0.1f)
        {
            var childRects = rect.gameObject.GetComponentsInChildren<RectTransform>();

            for (int i = 0; i < childRects.Length; i++)
            {
                RebuildAsync(childRects[i], repreatCount, interval);
            }
        }

        public static async void RebuildAsync(RectTransform rect, int repreatCount = 0, float interval = 0.1f)
        {
            int limit = 0;
            var secInterval = (int)(interval * 1000);

            while (limit <= repreatCount)
            {
                limit++;
                await UniTask.Delay(secInterval);

                if (rect != null)
                {
                    LayoutRebuilder.ForceRebuildLayoutImmediate(rect);
                }
                else
                {
                    Debug.LogWarning($"Rect transform is null or disabled");
                }
            }
        }

        public static async void RefreshLayoutGroupsImmediateAndRecursive(this LayoutGroup root, int repreatCount = 1, float interval = 0.1f)
        {
            var secInterval = (int)(interval * 1000);

            for (int i = 0; i < repreatCount; i++)
            {
                await UniTask.Delay(secInterval);

                LayoutRebuilder.ForceRebuildLayoutImmediate(root.GetComponent<RectTransform>());
                foreach (var layoutGroup in root.GetComponentsInChildren<LayoutGroup>())
                {
                    LayoutRebuilder.ForceRebuildLayoutImmediate(layoutGroup.GetComponent<RectTransform>());
                }
            }
        }
    }
}
