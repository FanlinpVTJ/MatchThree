namespace MatchThree.Domain
{
    public class TileModel
    {
        public TileColorType ColorType { get; }

        public TileModel(TileColorType colorType)
        {
            ColorType = colorType;
        }
    }
}
