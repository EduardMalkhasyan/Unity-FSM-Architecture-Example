using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace BugiGames.ExtensionMethod
{
    public static class ScrollRectExtensions
    {
        public static void DoToTop(this ScrollRect scrollRect, float duration = 0)
        {
            RectTransform contentTransform = scrollRect.content;
            float currentPosition = contentTransform.anchoredPosition.y;
            float targetPosition = 0f;

            DOTween.To(() => currentPosition, x =>
            {
                currentPosition = x;
                contentTransform.anchoredPosition = new Vector2(contentTransform.anchoredPosition.x, currentPosition);
            }, targetPosition, duration)
            .SetEase(Ease.Linear);
        }
    }
}
