using Cysharp.Threading.Tasks;
using UnityEngine;

public interface IInventoryScreenAnimator
{
    UniTask PlayShowAnimationAsync(CanvasGroup canvasGroup, RectTransform panelTransform);
    UniTask PlayHideAnimationAsync(CanvasGroup canvasGroup, RectTransform panelTransform);
}