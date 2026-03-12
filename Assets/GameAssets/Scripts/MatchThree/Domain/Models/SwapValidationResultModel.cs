namespace MatchThree.Domain.Models
{
    public class SwapValidationResultModel
    {
        public SwapValidationResultModel(bool isValid, string validationMessage)
        {
            IsValid = isValid;
            ValidationMessage = validationMessage;
        }

        public bool IsValid { get; }

        public string ValidationMessage { get; }
    }
}
