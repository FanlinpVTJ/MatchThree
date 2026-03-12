using Cysharp.Threading.Tasks;
using System.Collections.Generic;

//Data source
public interface IInventoryRepository
{
    UniTask<IReadOnlyList<IInventoryItemModel>> LoadItemsAsync();
    UniTask RemoveItemAsync(string itemId);
}
