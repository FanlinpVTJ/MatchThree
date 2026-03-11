using R3;

//модель содержит инфу о текущем выбранном предмете
public class InventoryItemViewModel
{
    public string Id { get; }
    public ReactiveProperty<string> Title;
    public ReactiveProperty<bool> IsSelected;

    public InventoryItemViewModel(InventoryItemData data)
    {
        Id = data.Id;
        Title = new ReactiveProperty<string>(data.Title);
        IsSelected = new ReactiveProperty<bool>(false);
    }
}
