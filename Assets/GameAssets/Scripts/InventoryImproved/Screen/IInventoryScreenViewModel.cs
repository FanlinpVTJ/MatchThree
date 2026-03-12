using Cysharp.Threading.Tasks;
using R3;
using System.Collections.Generic;
using UnityEngine;

public interface IInventoryScreenViewModel
{
    ReadOnlyReactiveProperty<IReadOnlyList<IInventoryItemViewModel>> Items { get; }
    ReadOnlyReactiveProperty<IInventoryItemViewModel> SelectedItem { get; }
    ReadOnlyReactiveProperty<string> SelectedItemTitle { get; }
    ReadOnlyReactiveProperty<bool> IsLoading { get; }
    ReadOnlyReactiveProperty<bool> IsOpened { get; }
    ReadOnlyReactiveProperty<bool> CanUseSelectedItem { get; }
    ReadOnlyReactiveProperty<bool> CanDropSelectedItem { get; }

    IAsyncCommand UseSelectedItemCommand { get; }
    IAsyncCommand DropSelectedItemCommand { get; }

    UniTask InitializeAsync();
    void Open();
    void Close();

    void SelectItem(IInventoryItemViewModel inventoryItemViewModel);
}
