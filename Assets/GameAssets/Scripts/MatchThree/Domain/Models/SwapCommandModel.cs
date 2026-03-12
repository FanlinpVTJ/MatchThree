using MatchThree.Domain.ValueObjects;

namespace MatchThree.Domain.Models
{
    public class SwapCommandModel
    {
        public SwapCommandModel(GridPositionModel sourcePositionModel, GridPositionModel targetPositionModel)
        {
            SourcePositionModel = sourcePositionModel;
            TargetPositionModel = targetPositionModel;
        }

        public GridPositionModel SourcePositionModel { get; }

        public GridPositionModel TargetPositionModel { get; }
    }
}
