using Microsoft.AspNetCore.Diagnostics;

namespace AvitoPRService.Extensions.MiddlewareExtension;

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseExceptionHandler(this IApplicationBuilder app)
    {
        return app.UseMiddleware<ExceptionHandlerMiddleware>();
    }
}