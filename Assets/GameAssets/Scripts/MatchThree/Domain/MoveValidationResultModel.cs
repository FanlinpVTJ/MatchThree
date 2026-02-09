namespace MatchThree.Domain
{
    public readonly struct MoveValidationResultModel
    {
        public bool IsValid { get; }

        public string ErrorMessage { get; }

        public MoveValidationResultModel(bool isValid, string errorMessage)
        {
            IsValid = isValid;
            ErrorMessage = errorMessage;
        }

        public static MoveValidationResultModel Valid()
        {
            MoveValidationResultModel result = new MoveValidationResultModel(true, string.Empty);

            return result;
        }

        public static MoveValidationResultModel Invalid(string errorMessage)
        {
            MoveValidationResultModel result = new MoveValidationResultModel(false, errorMessage);

            return result;
        }
    }
}
