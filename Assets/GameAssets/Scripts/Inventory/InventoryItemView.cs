using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemView : MonoBehaviour
{
    [SerializeField]
    private Button _button;

    [SerializeField]
    private TMP_Text _titleText;

    [SerializeField]
    private GameObject _selectedMarker;

    private readonly CompositeDisposable _disposables = new();

    private InventoryViewModel _rootViewModel;
    private InventoryItemViewModel _itemViewModel;

    private void OnDestroy()
    {
        _disposables.Dispose();
    }

    public void Bind(InventoryItemViewModel itemViewModel, InventoryViewModel rootViewModel)
    {
        _rootViewModel = rootViewModel;
        _itemViewModel = itemViewModel;

        itemViewModel.Title
            .Subscribe(value => _titleText.text = value)
            .AddTo(_disposables);

        itemViewModel.IsSelected
          .Subscribe(value => _selectedMarker.SetActive(value))
          .AddTo(_disposables);

        _button.onClick.AddListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        _rootViewModel.SelectItem(_itemViewModel);
    }
}
