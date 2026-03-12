using System.Collections.Generic;
using MatchThree.Domain.Models;
using MatchThree.Presentation.ViewModels;
using Zenject;

namespace MatchThree.Infrastructure.Factories
{
    public class CellViewModelFactory
    {
        private readonly IInstantiator _instantiator;

        public CellViewModelFactory(IInstantiator instantiator)
        {
            _instantiator = instantiator;
        }

        public CellViewModel CreateCellViewModel(CellModel cellModel)
        {
            List<object> argumentValues = new List<object>
            {
                cellModel
            };

            CellViewModel cellViewModel = _instantiator.Instantiate<CellViewModel>(argumentValues);

            return cellViewModel;
        }
    }
}
