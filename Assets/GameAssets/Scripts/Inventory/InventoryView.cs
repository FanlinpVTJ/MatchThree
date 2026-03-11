using UnityEngine;
using UnityEngine.UI;
using TMPro;
using R3;
using System.Collections.Generic;
using Zenject;
using Cysharp.Threading.Tasks;
using System.Threading.Tasks;
using System;
using DG.Tweening;

public class InventoryView : MonoBehaviour
{
    [Header("Root")]
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private RectTransform _panel;

    [Header("Controls")]
    [SerializeField] private Button _openButton;
    [SerializeField] private Button _closeButton;
    [SerializeField] private Button _useButton;
    [SerializeField] private Button _dropButton;
    [SerializeField] private TMP_Text _selectedTitleText;
    [SerializeField] private TMP_Text _loadingText;

    [Header("List")]
    [SerializeField] private Transform _contentRoot;
    [SerializeField] private InventoryItemView _itemPrefab;


    private InventoryViewModel _viewModel;
    private readonly CompositeDisposable _disposables = new();
    private readonly List<InventoryItemView> _spawnedViews = new();

    [Inject]
    public void Construct(InventoryViewModel viewModel)
    {
        _viewModel = viewModel;
        InitializeAsync().Forget();
    }

    private async UniTask InitializeAsync()
    {
        PrepareClosedState();

        _openButton.onClick.AddListener(() => _viewModel.Open());
        _closeButton.onClick.AddListener(() => _viewModel.Close());

        _useButton.onClick.AddListener(OnUseClicked);
        _dropButton.onClick.AddListener(OnDropClicked);

        _viewModel.SelectedTitle
            .Subscribe(title => _selectedTitleText.text = title)
            .AddTo(_disposables);

        _viewModel.CanUse
            .Subscribe(value => _useButton.interactable = value)
            .AddTo(_disposables);

        _viewModel.CanDrop
            .Subscribe(value => _dropButton.interactable = value)
            .AddTo(_disposables);

        _viewModel.IsBusy
            .Subscribe(value => _loadingText.gameObject.SetActive(value));

        _viewModel.IsOpened
            .Skip(1)
            .SubscribeAwait(async (isOpened, ct) =>
            {
                if (isOpened)
                {
                    await PlayOpenAsync();
                }
                else
                {
                    await PlayCloseAsync();
                }
            })
            .AddTo(_disposables);

        _viewModel.Items
            .Subscribe(items => RebuildList(items))
            .AddTo(_disposables);

        await _viewModel.InitializeAsync();
    }

    private void OnUseClicked()
    {
        OnUseClickedAsync().Forget();
    }

    private async UniTask OnUseClickedAsync()
    {
        await _viewModel.UseSelectedAsync();
    }

    private void OnDropClicked()
    {
        OnDropClickedAsync().Forget();
    }

    private async UniTask OnDropClickedAsync()
    {
        await _viewModel.DropSelectedAsync();
    }

    private void RebuildList(IReadOnlyList<InventoryItemViewModel> items)
    {
        ClearSpawned();
        for (int i = 0; i < items.Count; i++)
        {
            var instance = Instantiate(_itemPrefab, _contentRoot);
            instance.Bind(items[i], _viewModel);
            _spawnedViews.Add(instance);
        }
    }

    private void ClearSpawned()
    {
        for (int i = 0; i < _spawnedViews.Count; i++)
        {
            if (_spawnedViews[i] != null)
                Destroy(_spawnedViews[i].gameObject);
        }

        _spawnedViews.Clear();
    }

    private async UniTask PlayOpenAsync()
    {
        _canvasGroup.blocksRaycasts = true;
        _canvasGroup.interactable = true;

        var seq = DOTween.Sequence();
        seq.Append(_canvasGroup.DOFade(1f, 0.2f));
        seq.Join(_panel.DOAnchorPosY(0f, 0.25f).SetEase(Ease.OutCubic));

        await seq.AsyncWaitForCompletion().AsUniTask();
    }

    private async UniTask PlayCloseAsync()
    {
        _canvasGroup.interactable = false;

        var seq = DOTween.Sequence();
        seq.Append(_canvasGroup.DOFade(0f, 0.15f));
        seq.Join(_panel.DOAnchorPosY(-80f, 0.2f).SetEase(Ease.InCubic));

        await seq.AsyncWaitForCompletion().AsUniTask();

        _canvasGroup.blocksRaycasts = false;
    }

    private void PrepareClosedState()
    {
        _canvasGroup.alpha = 0f;
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
        _panel.anchoredPosition = new Vector2(0f, -80f);
    }

}
