using Cysharp.Threading.Tasks;
using System;

public class AsyncCommand : IAsyncCommand
{
    private readonly Func<bool> _canExecute;
    private readonly Func<UniTask> _executeAsync;

    public AsyncCommand(Func<UniTask> executeAsync, Func<bool> canExecute)
    {
        _canExecute = canExecute;
        _executeAsync = executeAsync;
    }

    public bool CanExecute => _canExecute?.Invoke() ?? true;

    public async UniTask ExecuteAsync()
    {
        if (!CanExecute) return;
        await _executeAsync.Invoke();
    }
}
