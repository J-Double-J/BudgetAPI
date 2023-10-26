using Domain.JsonSerializer;
using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;

namespace Domain.Abstract
{
    /// <summary>
    /// Struct that represent Error Codes.
    /// </summary>
    /// <remarks>
    /// Error codes are constructed of three levels that all must be provided:
    /// <list type="bullet">
    /// Error Type: High level representation of what kind of error occurred. Ex. Validation, Access, Http
    /// </list>
    /// <list type="bullet">
    /// Error Domain: This represents WHERE the error occurred. Ex. Entity, Database, IO, Request
    /// </list>
    /// <list type="bullet">
    /// Error Specification: This will give a more specific detail on what exactly went wrong at that level.
    /// </list>
    /// <example>Validation.Entity.InvalidTitle : This is a validation error that occurred in an entity where the title was invalid.</example>
    /// </remarks>
    [JsonConverter(typeof(ToStringJsonConverter))]
    public readonly struct ErrorCode
    {
        public static readonly ErrorCode None = new("", "", "");

        public ErrorCode(string errorType, string errorDomain, string errorSpecification)
        {
            ErrorType = errorType;
            ErrorDomain = errorDomain;
            ErrorSpecification = errorSpecification;
        }

        /// <summary>
        /// Gets or sets the Error Type
        /// </summary>
        public string ErrorType { get; init; }

        /// <summary>
        /// Gets or sets the Error Domain
        /// </summary>
        public string ErrorDomain { get; init; }

        /// <summary>
        /// Gets or sets the Error Specification
        /// </summary>
        public string ErrorSpecification { get; init; }

        public static ErrorCode ConstructFromCombinedTypeDomain(string errorTypeAndDomain, string errorSpecification)
        {
            string[] splitString = errorTypeAndDomain.Split('.');

            if(splitString.Length != 2 )
            {
                throw new InvalidOperationException("This method can only be used if a combination of the error type and domain have already been combined.");
            }

            foreach (string str in splitString)
            {
                if (string.IsNullOrEmpty(str))
                {
                    throw new InvalidOperationException("This method can only be used if the error code is represented as an error code: ErrorType.ErrorDomain.ErrorSpecification");
                }
            }

            return new ErrorCode(splitString[0], splitString[1], errorSpecification);
        }

        public static ErrorCode ConstructFromStringRepresentation(string errorCode)
        {
            string[] splitString = errorCode.Split(".");

            if(splitString.Length != 3 )
            {
                throw new InvalidOperationException("This method can only be used if the error code is represented as an error code: ErrorType.ErrorDomain.ErrorSpecification");
            }

            foreach(string str in splitString)
            {
                if (string.IsNullOrEmpty(str))
                {
                    throw new InvalidOperationException("This method can only be used if the error code is represented as an error code: ErrorType.ErrorDomain.ErrorSpecification");
                }
            }

            return new ErrorCode(splitString[0], splitString[1], splitString[2]);
        }

        public override string ToString()
        {
            return $"{ErrorType}.{ErrorDomain}.{ErrorSpecification}";
        }

        public static bool operator ==(ErrorCode left, ErrorCode right)
        {
            return left.ToString() == right.ToString();
        }

        public static bool operator != (ErrorCode left, ErrorCode right)
        {
            return left.ToString() != right.ToString();
        }

        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            if (obj is null)
            {
                return false;
            }

            if(obj is not ErrorCode)
            {
                return false;
            }

            return this == (ErrorCode)obj;
        }

        public override int GetHashCode()
        {
            // Combine the hash codes of the ErrorType, ErrorDomain, and ErrorSpecification properties.
            int hash = 17; // A prime number to start with.
            hash = hash * 23 + (ErrorType?.GetHashCode() ?? 0);
            hash = hash * 23 + (ErrorDomain?.GetHashCode() ?? 0);
            hash = hash * 23 + (ErrorSpecification?.GetHashCode() ?? 0);
            return hash;
        }
    }
}