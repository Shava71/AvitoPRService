using AvitoPRService.Application;
using AvitoPRService.Application.Mapper;
using AvitoPRService.Domain.Exception;

namespace AvitoPRService.Extensions.Middleware;

public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlerMiddleware> _logger;

    public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (DomainException ex)
        {
            _logger.LogError(ex, "Domain error: "+ ex.Message);

            ErrorResponseDto error = ErrorMapper.ToResponse(ex);
            context.Response.StatusCode = GetStatusCode(error.ErrorDto.CodeDto);
            context.Response.ContentType = "application/json";
            
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(error);
            
            await context.Response.WriteAsync(json);
        }
    }

    private int GetStatusCode(ErrorCodeDto code) => code switch
    {
        ErrorCodeDto.TEAM_EXISTS => StatusCodes.Status400BadRequest,
        ErrorCodeDto.PR_EXISTS => StatusCodes.Status409Conflict,
        ErrorCodeDto.PR_MERGED => StatusCodes.Status409Conflict,
        ErrorCodeDto.NOT_ASSIGNED => StatusCodes.Status409Conflict,
        ErrorCodeDto.NO_CANDIDATE => StatusCodes.Status409Conflict,
        ErrorCodeDto.NOT_FOUND => StatusCodes.Status404NotFound,

        _ => StatusCodes.Status400BadRequest
    };
}