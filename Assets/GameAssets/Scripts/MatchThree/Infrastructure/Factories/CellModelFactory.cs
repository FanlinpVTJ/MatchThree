using System.Collections.Generic;
using MatchThree.Domain.Interfaces;
using MatchThree.Domain.Models;
using MatchThree.Domain.ValueObjects;
using Zenject;

namespace MatchThree.Infrastructure.Factories
{
    public class CellModelFactory : ICellModelFactory
    {
        private readonly IInstantiator _instantiator;

        public CellModelFactory(IInstantiator instantiator)
        {
            _instantiator = instantiator;
        }

        public CellModel CreateCellModel(GridPositionModel positionModel, PieceModel pieceModel)
        {
            List<object> argumentValues = new List<object>
            {
                positionModel,
                pieceModel
            };

            CellModel cellModel = _instantiator.Instantiate<CellModel>(argumentValues);

            return cellModel;
        }
    }
}
