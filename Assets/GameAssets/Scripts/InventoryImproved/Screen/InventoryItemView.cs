using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemView : MonoBehaviour, IInventoryItemView
{
    [SerializeField] private Button _selectButton;
    [SerializeField] private TMP_Text _titleText;
    [SerializeField] private GameObject _selectedMarker;

    private readonly CompositeDisposable _disposables = new();

    public void Bind(IInventoryItemViewModel inventoryItemViewModel, IInventoryScreenViewModel inventoryScreenViewModel)
    {
        inventoryItemViewModel.Title
            .Subscribe(value => _titleText.text = value)
            .AddTo(_disposables);

        inventoryItemViewModel.IsSelected
            .Subscribe(value => _selectedMarker.SetActive(value))
            .AddTo(_disposables);

        _selectButton.onClick.AddListener(() => inventoryScreenViewModel.SelectItem(inventoryItemViewModel));
    }

    private void OnDestroy()
    {
        _disposables.Dispose();
    }
}
