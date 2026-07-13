using eb17953u202421866.Shared.Infrastructure.Pipeline.Middleware.Components;

namespace eb17953u202421866.Shared.Infrastructure.Pipeline.Middleware.Extensions;

/// <summary>
///     Registers custom middleware components in the pipeline.
/// </summary>
/// <remarks>
///     Author: __YOUR_NAME__
/// </remarks>
public static class MiddlewareExtensions
{
    /// <summary>Registers <see cref="GlobalExceptionHandlerMiddleware" /> in the pipeline.</summary>
    public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<GlobalExceptionHandlerMiddleware>();
    }
}
