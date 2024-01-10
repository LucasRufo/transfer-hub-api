using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json.Serialization;

namespace Money.API.Endpoints.Shared;

public class CustomProblemDetails : ProblemDetails
{
    [JsonPropertyName("errors")]
    public IList<CustomProblemDetailsError> Errors { get; set; } = new List<CustomProblemDetailsError>();

    public CustomProblemDetails()
    {
    }

    public static CustomProblemDetails CreateValidationProblemDetails(string path, IList<CustomProblemDetailsError> errors)
    {
        return new CustomProblemDetails()
        {
            Title = "One or more validations errors occurred.",
            Status = (int)HttpStatusCode.BadRequest,
            Detail = "An error occurred. Check 'errors' for details.",
            Errors = errors,
            Instance = path
        };
    }

    public static CustomProblemDetails CreateDomainProblemDetails(HttpStatusCode statusCode, string path, Error error)
    {
        return new CustomProblemDetails()
        {
            Title = error.Code,
            Status = (int)statusCode,
            Detail = error.Description,
            Errors = [],
            Instance = path
        };
    }
}

public class CustomProblemDetailsError
{
    public string Property { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;

    public CustomProblemDetailsError()
    {
    }

    public CustomProblemDetailsError(string propertyName, string errorMessage)
    {
        Property = propertyName;
        Message = errorMessage;
    }
}
