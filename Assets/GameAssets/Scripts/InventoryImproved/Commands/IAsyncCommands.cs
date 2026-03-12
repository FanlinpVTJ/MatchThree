using Cysharp.Threading.Tasks;

public interface IAsyncCommand
{
    bool CanExecute { get; }
    UniTask ExecuteAsync();
}
