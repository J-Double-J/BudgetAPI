using Domain.CustomFluentValidation;
using Newtonsoft.Json;

namespace Domain.Abstract
{
    /// <summary>
    /// Class for Validation Errors.
    /// </summary>
    public class ValidationError : Error
    {

        /// <summary>
        /// Validation Error for constructor.
        /// </summary>
        /// <param name="messageHeader">String to display in message before error. Note that a new line is inserted at the end.</param>
        /// <param name="errorMessage">Error message for validation.</param>
        public ValidationError(ErrorCode errorCode, string errorMessage)
            : base(errorCode, errorMessage)
        {
            DetermineIfValidationLevelIsInternal(errorCode);
        }

        public ValidationError(ErrorCode errorCode, string errorMessage, bool isInternal)
            : base(errorCode, errorMessage, isInternal)
        {
        }

        [JsonIgnore]
        public BudgetValidatorType ValidationErrorLevel { get; init; }

        /// <summary>
        /// Determines if the error code type is an internal error. This can be used to mask errors when returned in the API.
        /// </summary>
        /// <param name="validationLevel">What level the error occurred at.</param>
        /// <exception cref="InvalidOperationException">Throws if a <see cref="BudgetValidatorType.Mixed"/> is passed in. Errors
        /// must </exception>
        private void DetermineIfValidationLevelIsInternal(ErrorCode errorCode)
        {
            BudgetValidatorType validatorLevel = ValidationErrorCodeFactory.ValidationLevelFromCode(errorCode);

            // Likely subject to change at some point as errors become more specific. These enums might be extracted to classes to have behavior.
            switch (validatorLevel)
            {
                case BudgetValidatorType.None:        IsInternalError = false; break;
                case BudgetValidatorType.Entity:      IsInternalError = false; break;
                case BudgetValidatorType.Database:    IsInternalError = true; break;
                case BudgetValidatorType.ExternalCommunication:   IsInternalError = false; break;
                case BudgetValidatorType.IO:                      IsInternalError = true; break;
                case BudgetValidatorType.Configuration:           IsInternalError = true; break;
                case BudgetValidatorType.Mixed:       throw new InvalidOperationException("Validation Errors must not be mixed and must be assigned.");
                case BudgetValidatorType.Unknown:     IsInternalError = true; break; // We assume we did not account for an internal problem, or code was improperly formmatted.
            }
        }
    }
}
