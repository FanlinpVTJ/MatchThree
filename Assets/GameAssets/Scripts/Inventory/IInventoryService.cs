using Cysharp.Threading.Tasks;
using System.Collections.Generic;

//интерфейс сервиса инвенторя
public interface IInventoryService
{
    UniTask<IReadOnlyList<InventoryItemData>> LoadAsync();
    UniTask UseAsync(string itemId);
    UniTask DropAsync(string itemId);
}
