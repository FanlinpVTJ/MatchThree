using System;
using R3;

namespace MatchThree.Presentation.ViewModels
{
    public abstract class ViewModelBase : IDisposable
    {
        private DisposableBag _disposableBag;

        public void Dispose()
        {
            _disposableBag.Dispose();
        }

        protected void AddDisposable(IDisposable disposable)
        {
            _disposableBag.Add(disposable);
        }
    }
}
