using System.Collections.Generic;
using MatchThree.Domain.Models;

namespace MatchThree.Domain.Interfaces
{
    public interface IBoardModelFactory
    {
        BoardModel CreateBoardModel(int widthValue, int heightValue, List<CellModel> cellModels);
    }
}
