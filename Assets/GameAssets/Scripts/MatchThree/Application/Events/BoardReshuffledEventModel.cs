namespace MatchThree.Application.Events
{
    public readonly struct BoardReshuffledEventModel
    {
        public int ReshuffleAttemptCount { get; }

        public BoardReshuffledEventModel(int reshuffleAttemptCount)
        {
            ReshuffleAttemptCount = reshuffleAttemptCount;
        }
    }
}
