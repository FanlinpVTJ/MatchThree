using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public sealed class InventoryScreenAnimator : IInventoryScreenAnimator
{
    public async UniTask PlayShowAnimationAsync(CanvasGroup canvasGroup, RectTransform panelTransform)
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;

        var sequence = DOTween.Sequence();
        sequence.Append(canvasGroup.DOFade(1f, 0.2f));
        sequence.Join(panelTransform.DOAnchorPosY(0f, 0.25f).SetEase(Ease.OutCubic));

        await sequence.AsyncWaitForCompletion().AsUniTask();
    }

    public async UniTask PlayHideAnimationAsync(CanvasGroup canvasGroup, RectTransform panelTransform)
    {
        canvasGroup.interactable = false;

        var sequence = DOTween.Sequence();
        sequence.Append(canvasGroup.DOFade(0f, 0.15f));
        sequence.Join(panelTransform.DOAnchorPosY(-80f, 0.2f).SetEase(Ease.InCubic));

        await sequence.AsyncWaitForCompletion().AsUniTask();

        canvasGroup.blocksRaycasts = false;
    }
}