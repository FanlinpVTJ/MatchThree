using Cysharp.Threading.Tasks;
using R3;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class InventoryScreenView : MonoBehaviour, IInventoryScreenView
{
    [Header("Root")]
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private RectTransform _panelTransform;

    [Header("Controls")]
    [SerializeField] private Button _openButton;
    [SerializeField] private Button _closeButton;
    [SerializeField] private Button _useButton;
    [SerializeField] private Button _dropButton;
    [SerializeField] private TMP_Text _selectedItemTitleText;
    [SerializeField] private GameObject _loadingIndicator;

    [Header("List")]
    [SerializeField] private Transform _itemContainer;
    [SerializeField] private InventoryItemView _inventoryItemViewPrefab;

    private CompositeDisposable _disposables = new();
    private readonly List<InventoryItemView> _spawnedItemViews = new();

    private IInventoryScreenViewModel _inventoryScreenViewModel;
    private IInventoryScreenAnimator _inventoryScreenAnimator;
    private IInstantiator _instantiator;

    [Inject]
    public void Construct(IInventoryScreenViewModel inventoryScreenViewModel, IInstantiator instantiator, IInventoryScreenAnimator inventoryScreenAnimator)
    {
        _inventoryScreenViewModel = inventoryScreenViewModel;
        _instantiator = instantiator;
        _inventoryScreenAnimator = inventoryScreenAnimator;

    }

    private void Start()
    {
        PrepareInitialState();
        Bind(_inventoryScreenViewModel);
        _inventoryScreenViewModel.InitializeAsync().Forget();
    }

    private void OnDestroy()
    {
        _openButton.onClick?.RemoveAllListeners();
        _closeButton.onClick?.RemoveAllListeners();
        _useButton.onClick?.RemoveAllListeners();
        _dropButton.onClick?.RemoveAllListeners();
        _disposables.Dispose();
        ClearItemViews();
    }

    public void Bind(IInventoryScreenViewModel inventoryScreenViewModel)
    {
        _inventoryScreenViewModel = inventoryScreenViewModel;

        _openButton.onClick.AddListener(_inventoryScreenViewModel.Open);
        _closeButton.onClick.AddListener(_inventoryScreenViewModel.Close);
        _useButton.onClick.AddListener(OnUseButtonClicked);
        _dropButton.onClick.AddListener(OnDropButtonClicked);

        _inventoryScreenViewModel.SelectedItemTitle
            .Subscribe(title => _selectedItemTitleText.text = title)
            .AddTo(_disposables);

        _inventoryScreenViewModel.IsLoading
            .Subscribe(value => _loadingIndicator.SetActive(value))
            .AddTo(_disposables);

        _inventoryScreenViewModel.CanDropSelectedItem
            .SubscribeToInteractable(_dropButton)
            .AddTo(_disposables);

        _inventoryScreenViewModel.CanUseSelectedItem
            .SubscribeToInteractable(_useButton)
            .AddTo(_disposables);

        _inventoryScreenViewModel.Items
            .Subscribe(RebuildItemViews)
            .AddTo(_disposables);

        _inventoryScreenViewModel.IsOpened
            .Skip(1)
            .SubscribeAwait(async (isOnened, ct) =>
            {
                if (isOnened)
                {
                    await ShowAsync();
                }
                else
                {
                    await HideAsync();
                }
            })
            .AddTo(_disposables);
    }


    public async UniTask ShowAsync()
    {
        await _inventoryScreenAnimator.PlayShowAnimationAsync(_canvasGroup, _panelTransform);
    }

    public async UniTask HideAsync()
    {
        await _inventoryScreenAnimator.PlayHideAnimationAsync(_canvasGroup, _panelTransform);
    }

    private void OnUseButtonClicked()
    {
        _inventoryScreenViewModel.UseSelectedItemCommand.ExecuteAsync();
    }

    private void OnDropButtonClicked()
    {
        _inventoryScreenViewModel.DropSelectedItemCommand.ExecuteAsync();
    }

    private void RebuildItemViews(IReadOnlyList<IInventoryItemViewModel> itemViewModels)
    {
        ClearItemViews();

        foreach (var itemViewModel in itemViewModels)
        {
            var itemView = _instantiator.InstantiatePrefabForComponent<InventoryItemView>(_inventoryItemViewPrefab, _itemContainer);
            itemView.Bind(itemViewModel, _inventoryScreenViewModel);
            _spawnedItemViews.Add(itemView);
        }
    }

    private void PrepareInitialState()
    {
        _canvasGroup.alpha = 0f;
        _canvasGroup.blocksRaycasts = false;
        _canvasGroup.interactable = false;
        _panelTransform.anchoredPosition = new Vector2(0f, -80f);
    }

    private void ClearItemViews()
    {
        foreach (var itemView in _spawnedItemViews)
        {
            if (itemView != null)
                Destroy(itemView.gameObject);
        }

        _spawnedItemViews.Clear();
    }
}
