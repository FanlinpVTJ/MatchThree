using System.Collections.Generic;
using MatchThree.Domain.Interfaces;
using MatchThree.Domain.Models;
using MatchThree.Domain.ValueObjects;
using Zenject;

namespace MatchThree.Infrastructure.Factories
{
    public class PieceModelFactory : IPieceModelFactory
    {
        private readonly IInstantiator _instantiator;

        public PieceModelFactory(IInstantiator instantiator)
        {
            _instantiator = instantiator;
        }

        public PieceModel CreatePieceModel(int pieceIdentifier, int pieceTypeIdentifier, GridPositionModel positionModel)
        {
            List<object> argumentValues = new List<object>
            {
                pieceIdentifier,
                pieceTypeIdentifier,
                positionModel
            };

            PieceModel pieceModel = _instantiator.Instantiate<PieceModel>(argumentValues);

            return pieceModel;
        }
    }
}
