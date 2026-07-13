using System.Net.Mime;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using eb17953u202421866.Shared.Resources;
using eb17953u202421866.Shared.Resources.Errors;

namespace eb17953u202421866.Shared.Infrastructure.Pipeline.Middleware.Components;

/// <summary>
///     Catches any unhandled exception that reaches the pipeline and returns a localized
///     ProblemDetails response instead of letting ASP.NET Core's default (unlocalized) handler answer.
/// </summary>
/// <remarks>
///     IMPORTANT: registering this class alone does nothing. You MUST call
///     `app.UseGlobalExceptionHandler();` in Program.cs, as EARLY as possible in the pipeline
///     (right after `var app = builder.Build();`, before UseRequestLocalization/UseSwagger/MapControllers).
///     Forgetting this line means the middleware exists but never runs — a real bug found in a
///     previous submission where exception handling silently fell back to ASP.NET Core defaults.
///     Author: __YOUR_NAME__
/// </remarks>
public class GlobalExceptionHandlerMiddleware(
    RequestDelegate next,
    ILogger<GlobalExceptionHandlerMiddleware> logger,
    IStringLocalizer<ErrorMessages> errorLocalizer,
    IStringLocalizer<CommonMessages> commonLocalizer)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (OperationCanceledException ex)
        {
            logger.LogWarning(ex, "Request was cancelled: {Message}", ex.Message);
            await WriteProblemAsync(context, 499, commonLocalizer["OperationCancelled"], errorLocalizer["OperationCancelled"]); // 499: non-standard "client closed request", no official StatusCodes constant
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unhandled exception occurred: {Message}", ex.Message);
            await WriteProblemAsync(context, StatusCodes.Status500InternalServerError, commonLocalizer["InternalServerError"], errorLocalizer["GenericError"]);
        }
    }

    private static async Task WriteProblemAsync(HttpContext context, int statusCode, string title, string detail)
    {
        context.Response.ContentType = MediaTypeNames.Application.Json;
        context.Response.StatusCode = statusCode;

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = detail,
            Instance = context.Request.Path
        };

        var jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        await context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails, jsonOptions));
    }
}
