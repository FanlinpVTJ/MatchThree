namespace MatchThree.Presentation.Interfaces
{
    public interface IView<TViewModel>
    {
        void Bind(TViewModel viewModel);
    }
}
