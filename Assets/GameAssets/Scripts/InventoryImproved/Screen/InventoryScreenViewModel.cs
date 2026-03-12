using Cysharp.Threading.Tasks;
using R3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class InventoryScreenViewModel : IInventoryScreenViewModel, IDisposable
{
    private readonly IInventoryApplicationService _inventoryApplicationService;
    private readonly IInventoryModel _inventoryModel;
    private CompositeDisposable _disposables = new();

    private readonly ReactiveProperty<IReadOnlyList<IInventoryItemViewModel>> _items;
    private readonly ReactiveProperty<IInventoryItemViewModel> _selectedItem;
    private readonly ReactiveProperty<bool> _isLoading;
    private readonly ReactiveProperty<bool> _isOpened;

    public ReadOnlyReactiveProperty<IReadOnlyList<IInventoryItemViewModel>> Items { get; }
    public ReadOnlyReactiveProperty<IInventoryItemViewModel> SelectedItem { get; }
    public ReadOnlyReactiveProperty<string> SelectedItemTitle { get; }
    public ReadOnlyReactiveProperty<bool> IsLoading { get; }
    public ReadOnlyReactiveProperty<bool> IsOpened { get; }
    public ReadOnlyReactiveProperty<bool> CanUseSelectedItem { get; }
    public ReadOnlyReactiveProperty<bool> CanDropSelectedItem { get; }

    public IAsyncCommand UseSelectedItemCommand { get; }
    public IAsyncCommand DropSelectedItemCommand { get; }

    public InventoryScreenViewModel(IInventoryApplicationService inventoryApplicationService, IInventoryModel inventoryModel)
    {
        _inventoryApplicationService = inventoryApplicationService;
        _inventoryModel = inventoryModel;

        _items = new ReactiveProperty<IReadOnlyList<IInventoryItemViewModel>>(Array.Empty<IInventoryItemViewModel>())
            .AddTo(_disposables);

        _selectedItem = new ReactiveProperty<IInventoryItemViewModel>(null)
            .AddTo(_disposables);

        _isLoading = new ReactiveProperty<bool>(false)

            .AddTo(_disposables);
        _isOpened = new ReactiveProperty<bool>(false)
            .AddTo(_disposables);

        Items = _items
            .ToReadOnlyReactiveProperty()
            .AddTo(_disposables);

        SelectedItem = _selectedItem
            .ToReadOnlyReactiveProperty()
            .AddTo(_disposables);

        SelectedItemTitle = _selectedItem
            .Select(item => item == null ? "Nothing selected" : item.Title.CurrentValue)
            .ToReadOnlyReactiveProperty()
            .AddTo(_disposables);

        IsLoading = _isLoading
            .ToReadOnlyReactiveProperty()
            .AddTo(_disposables);

        IsOpened = _isOpened
           .ToReadOnlyReactiveProperty()
           .AddTo(_disposables);

        CanUseSelectedItem = Observable
            .CombineLatest(_selectedItem, _isLoading, (item, isLoading) => item != null && !isLoading && item.GetModel().IsConsumable)
            .ToReadOnlyReactiveProperty()
            .AddTo(_disposables);

        CanDropSelectedItem = Observable
            .CombineLatest(_selectedItem, _isLoading, (item, isLoading) => item != null && !isLoading)
            .ToReadOnlyReactiveProperty()
            .AddTo(_disposables);

        UseSelectedItemCommand = new AsyncCommand(UseSelectedItemAsync, () => CanUseSelectedItem.CurrentValue);
        DropSelectedItemCommand = new AsyncCommand(DropSelectedItemAsync, () => CanDropSelectedItem.CurrentValue);
    }

    public async UniTask InitializeAsync()
    {
        _isLoading.Value = true;

        try
        {
            var itemModels = await _inventoryApplicationService.LoadInventoryAsync();
            _inventoryModel.SetItems(itemModels);

            var itemViewModels = _inventoryModel.Items
                .Select(itemModel => (IInventoryItemViewModel)new InventoryItemViewModel(itemModel))
                .ToArray();

            _items.Value = itemViewModels;

            if (_items.Value.Count > 0)
            {
                var item = _items.Value.FirstOrDefault();
                SelectItem(item);
            }
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError($"InventoryScreenViewModel InitializeAsync {e.Message}");
        }
        finally
        {
            _isLoading.Value = false;
        }
    }

    public void SelectItem(IInventoryItemViewModel inventoryItemViewModel)
    {
        foreach (var item in _items.Value)
        {
            item.Deselect();
        }

        _selectedItem.Value = inventoryItemViewModel;
        inventoryItemViewModel?.Select();
    }

    public void Open()
    {
        _isOpened.Value = true;
    }

    public void Close()
    {
        _isOpened.Value = false;
    }

    private async UniTask UseSelectedItemAsync()
    {
        var selectedItemViewModel = _selectedItem.Value;
        if (selectedItemViewModel == null)
            return;

        _isLoading.Value = true;

        try
        {
            await _inventoryApplicationService.UseItemAsync(selectedItemViewModel.GetModel());
            await ReloadAsync(selectedItemViewModel.Id);
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError($"InventoryScreenViewModel UseSelectedItemAsync {e.Message}");
        }
        finally
        {
            _isLoading.Value = false;
        }
    }

    private async UniTask DropSelectedItemAsync()
    {
        var selectedItemViewModel = _selectedItem.Value;
        if (selectedItemViewModel == null)
            return;

        _isLoading.Value = true;

        try
        {
            await _inventoryApplicationService.DropItemAsync(selectedItemViewModel.GetModel());
            await ReloadAsync(null);
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError($"InventoryScreenViewModel UseSelectedItemAsync {e.Message}");
        }
        finally
        {
            _isLoading.Value = false;
        }
    }

    private async UniTask ReloadAsync(string id)
    {
        var itemModels = await _inventoryApplicationService.LoadInventoryAsync();
        _inventoryModel.SetItems(itemModels);
        var itemViewModels = _inventoryModel.Items
                .Select(itemModel => (IInventoryItemViewModel)new InventoryItemViewModel(itemModel))
                .ToArray();

        _items.Value = itemViewModels;
        var selectedItem = _items.Value.FirstOrDefault(item => item.Id == id) ?? _items.Value.FirstOrDefault();
        SelectItem(selectedItem);
    }

    public void Dispose()
    {
        _disposables.Dispose();
        _items.Dispose();
        _selectedItem.Dispose();
        _isLoading.Dispose();
        _isOpened.Dispose();
    }
}
