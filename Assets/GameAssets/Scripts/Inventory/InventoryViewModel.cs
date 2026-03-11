using Cysharp.Threading.Tasks;
using R3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class InventoryViewModel : IDisposable
{
    #region Private fields
    private readonly IInventoryService _inventoryService;
    private readonly CompositeDisposable _disposables = new();

    private readonly List<InventoryItemViewModel> _items = new();
    private readonly ReactiveProperty<IReadOnlyList<InventoryItemViewModel>> _itemsView;
    private readonly ReactiveProperty<InventoryItemViewModel> _selectedItem;
    private readonly ReactiveProperty<bool> _isBusy = new(false);
    private readonly ReactiveProperty<bool> _isOpened = new(false);
    #endregion

    #region Public fields
    public ReadOnlyReactiveProperty<IReadOnlyList<InventoryItemViewModel>> Items { get; }
    public ReadOnlyReactiveProperty<InventoryItemViewModel> Selecteditem { get; }
    public ReadOnlyReactiveProperty<bool> IsBusy { get; }
    public ReadOnlyReactiveProperty<bool> IsOpened { get; }
    public ReadOnlyReactiveProperty<string> SelectedTitle { get; }
    public ReadOnlyReactiveProperty<bool> CanUse { get; }
    public ReadOnlyReactiveProperty<bool> CanDrop { get; }
    #endregion

    public InventoryViewModel(IInventoryService inventoryService)
    {
        _inventoryService = inventoryService;

        _itemsView = new ReactiveProperty<IReadOnlyList<InventoryItemViewModel>>(Array.Empty<InventoryItemViewModel>());
        _selectedItem = new ReactiveProperty<InventoryItemViewModel>(null);
        _isBusy = new ReactiveProperty<bool>(false);
        _isOpened = new ReactiveProperty<bool>(false);

        Items = _itemsView
            .ToReadOnlyReactiveProperty()
            .AddTo(_disposables);

        Selecteditem = _selectedItem
            .ToReadOnlyReactiveProperty()
            .AddTo(_disposables);

        IsBusy = _isBusy
            .ToReadOnlyReactiveProperty()
            .AddTo(_disposables);

        IsOpened = _isOpened
            .ToReadOnlyReactiveProperty()
            .AddTo(_disposables);

        SelectedTitle = _selectedItem
            .Select(item => item == null ? "nothing to select" : item.Title.Value)
            .ToReadOnlyReactiveProperty()
            .AddTo(_disposables);

        CanUse = Observable
            .CombineLatest(_selectedItem, _isBusy, (item, busy) => item != null && !busy)
            .ToReadOnlyReactiveProperty()
            .AddTo(_disposables);

        CanDrop = Observable
            .CombineLatest(_selectedItem, _isBusy, (item, busy) => item != null && !busy)
            .ToReadOnlyReactiveProperty()
            .AddTo(_disposables);
    }

    public async UniTask InitializeAsync()
    {
        _isBusy.Value = true;

        try
        {
            var loadeditems = await _inventoryService.LoadAsync();
            _items.Clear();

            foreach (var item in loadeditems)
            {
                _items.Add(new InventoryItemViewModel(item));
            }

            PublishItems();
            SelectItem(_items.Count > 0 ? _items[0] : null);


        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError("InventoryViewModel " + e.Message);
        }
        finally
        {
            _isBusy.Value = false;
        }
    }

    public void Open()
    {
        _isOpened.Value = true;
    }

    public void Close()
    {
        _isOpened.Value = false;
    }

    public void SelectItem(InventoryItemViewModel item)
    {
        foreach (var inventoryItem in _items)
        {
            inventoryItem.IsSelected.Value = false;
        }

        _selectedItem.Value = item;

        if (item != null)
        {
            item.IsSelected.Value = true;
        }
    }

    public async UniTask UseSelectedAsync()
    {
        await ExecuteActionForSelectedItem(_inventoryService.UseAsync, false, "UseSelectedAsync");
    }

    public async UniTask DropSelectedAsync()
    {
        await ExecuteActionForSelectedItem(_inventoryService.DropAsync, true, "DropSelectedAsync");
    }

    private async UniTask ExecuteActionForSelectedItem(Func<string, UniTask> action, bool isDrop, string operationName)
    {
        var selected = _selectedItem.CurrentValue;

        if (selected == null || _isBusy.CurrentValue)
        {
            UnityEngine.Debug.Log($"InventoryViewModel: selected {(selected == null).ToString()}, isBusy {_isBusy.CurrentValue.ToString()}");
            return;
        }

        _isBusy.Value = true;

        try
        {
            await action(selected.Id);
            await ReloadPreserveSelectionAsync(isDrop ? null : selected.Id);
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError($"InventoryViewModel: {operationName} {e.Message}");
        }
        finally
        {
            _isBusy.Value = false;
        }
    }

    private async UniTask ReloadPreserveSelectionAsync(string preferredId)
    {
        var loadedItems = await _inventoryService.LoadAsync();
        _items.Clear();

        foreach (var item in loadedItems)
        {
            _items.Add(new InventoryItemViewModel(item));
        }
        
        PublishItems();
        
        var targetItem = _items.FirstOrDefault(x => x.Id == preferredId) ?? _items.FirstOrDefault();

        SelectItem(targetItem);
    }

    private void PublishItems()
    {
        _itemsView.Value = _items.ToArray();
    }

    public void Dispose()
    {
        _disposables.Dispose();

        foreach (var item in _items)
        {
            item.Title.Dispose();
            item.IsSelected.Dispose();
        }

        _itemsView.Dispose();
        _selectedItem.Dispose();
        _isBusy.Dispose();
        _isOpened.Dispose();
    }
}
