using Newtonsoft.Json;
using System.Text;

namespace Domain.Abstract
{
    public class DomainValidationResult : Result, IValidationResult
    {
        private ErrorCode? _highLevelErrorCode;

        private DomainValidationResult(ValidationError[] errors)
            : base(false, IValidationResult.ValidationError(errors[0].Code.ErrorDomain, errors[0].Code.ErrorSpecification))
        {
            ValidationErrors = errors;
        }

        /// <summary>
        /// Successful Validation Para
        /// </summary>
        /// <param name="success"></param>
        /// <param name="errorType"></param>
        private DomainValidationResult()
            : base(true, Error.None)
        {
            ValidationErrors = Array.Empty<ValidationError>(); 
        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ErrorCode? HighLevelErrorCode
        {
            get
            {
                if (_highLevelErrorCode != null && _highLevelErrorCode != ErrorCode.None)
                {
                    return _highLevelErrorCode;
                }

                if (ValidationErrors.Length > 0)
                {
                    return ValidationErrors[0].Code;
                }

                return null;
            }

            set
            {
                if (value is null)
                {
                    throw new InvalidOperationException("Cannot set HighLevelErrorCode to null");
                }

                _highLevelErrorCode = value;
            }
        }

        public ValidationError[] ValidationErrors { get; }

        public static new DomainValidationResult Success() { return SuccessfulValidation(); }

        public static DomainValidationResult SuccessfulValidation() { return new DomainValidationResult(); }

        public static DomainValidationResult WithErrors(ValidationError[] errors) { return new DomainValidationResult(errors); }
    }
}
