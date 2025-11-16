using AvitoPRService.Extensions.Middleware;
using Microsoft.AspNetCore.Diagnostics;

namespace AvitoPRService.Extensions.MiddlewareExtension;

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseAdditionalExceptionHandler(this IApplicationBuilder app)
    {
        return app.UseMiddleware<ErrorHandlerMiddleware>();
    }
}