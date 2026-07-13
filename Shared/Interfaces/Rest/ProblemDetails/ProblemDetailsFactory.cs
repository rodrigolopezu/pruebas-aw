using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using eb17953u202421866.Shared.Resources;
using eb17953u202421866.Shared.Resources.Errors;

namespace eb17953u202421866.Shared.Interfaces.Rest.ProblemDetails;

/// <summary>
///     Builds a localized ProblemDetails response for controller actions, given an HTTP status code,
///     an optional business-error enum (used as the localization key for the Title) and a detail message.
/// </summary>
/// <remarks>
///     Use this from your ActionResultAssembler (per bounded context) instead of calling
///     `controller.Problem(...)` directly, so every error response in the API is localized consistently.
///     Author: __YOUR_NAME__
/// </remarks>
public class ProblemDetailsFactory(
    IStringLocalizer<ErrorMessages> errorLocalizer,
    IStringLocalizer<CommonMessages> commonLocalizer)
{
    public IActionResult CreateProblemDetails(
        ControllerBase controller,
        int statusCode,
        Enum? errorEnum,
        string detailMessage)
    {
        var title = errorEnum != null ? errorLocalizer[$"{errorEnum}"].Value : commonLocalizer["GenericError"].Value;

        var problemDetails = new Microsoft.AspNetCore.Mvc.ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = detailMessage,
            Instance = controller.HttpContext.Request.Path
        };

        return controller.StatusCode(statusCode, problemDetails);
    }
}
