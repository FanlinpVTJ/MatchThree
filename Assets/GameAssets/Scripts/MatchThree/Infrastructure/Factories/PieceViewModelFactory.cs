using System.Collections.Generic;
using MatchThree.Domain.Models;
using MatchThree.Presentation.ViewModels;
using Zenject;

namespace MatchThree.Infrastructure.Factories
{
    public class PieceViewModelFactory
    {
        private readonly IInstantiator _instantiator;

        public PieceViewModelFactory(IInstantiator instantiator)
        {
            _instantiator = instantiator;
        }

        public PieceViewModel CreatePieceViewModel(PieceModel pieceModel)
        {
            List<object> argumentValues = new List<object>
            {
                pieceModel
            };

            PieceViewModel pieceViewModel = _instantiator.Instantiate<PieceViewModel>(argumentValues);

            return pieceViewModel;
        }
    }
}
