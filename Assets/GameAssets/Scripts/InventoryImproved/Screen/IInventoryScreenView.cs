using Cysharp.Threading.Tasks;

public interface IInventoryScreenView
{
    UniTask ShowAsync();
    UniTask HideAsync();
    void Bind(IInventoryScreenViewModel inventoryScreenViewModel);
}