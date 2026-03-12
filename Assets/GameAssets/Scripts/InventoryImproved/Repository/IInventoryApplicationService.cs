using Cysharp.Threading.Tasks;
using System.Collections.Generic;

public interface IInventoryApplicationService
{
    UniTask<IReadOnlyList<IInventoryItemModel>> LoadInventoryAsync();
    UniTask UseItemAsync(IInventoryItemModel itemModel);
    UniTask DropItemAsync(IInventoryItemModel itemModel);
}
