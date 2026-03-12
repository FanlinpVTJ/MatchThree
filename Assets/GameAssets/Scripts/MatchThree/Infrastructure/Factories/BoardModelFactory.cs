using System.Collections.Generic;
using MatchThree.Domain.Interfaces;
using MatchThree.Domain.Models;
using Zenject;

namespace MatchThree.Infrastructure.Factories
{
    public class BoardModelFactory : IBoardModelFactory
    {
        private readonly IInstantiator _instantiator;

        public BoardModelFactory(IInstantiator instantiator)
        {
            _instantiator = instantiator;
        }

        public BoardModel CreateBoardModel(int widthValue, int heightValue, List<CellModel> cellModels)
        {
            List<object> argumentValues = new List<object>
            {
                widthValue,
                heightValue,
                cellModels
            };

            BoardModel boardModel = _instantiator.Instantiate<BoardModel>(argumentValues);

            return boardModel;
        }
    }
}
