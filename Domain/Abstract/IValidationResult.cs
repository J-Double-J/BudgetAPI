namespace Domain.Abstract
{
    public interface IValidationResult
    {
        public static Error ValidationError(string domain, string errorSpecification)
        {
            return new Error(new("ValidationError", domain, errorSpecification), "A validation error occurred.");
        } 

        ValidationError[] ValidationErrors { get; }
    }
}
