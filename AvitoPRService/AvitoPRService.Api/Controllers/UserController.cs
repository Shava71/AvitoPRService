using AvitoPRService.Application.Services.Interfaces;
using AvitoPRService.Mapper;
using Microsoft.AspNetCore.Mvc;

namespace AvitoPRService.Api.Controllers;

[ApiController]
[Route("users")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    
    public UserController(IUserService userService)
    {
        _userService = userService;
    }
    
    /// <summary>
    /// Установить флаг активности пользователя
    /// </summary>
    /// <returns>Обновлённый пользователь</returns>
    [HttpPost("setIsActive")]
    public async Task<IActionResult> SetIsActive([FromBody] SetUserIsActiveRequest request)
    {
        var user = await _userService.SetUserActiveAsync(request.User_id, request.Is_active);
        return Ok(new SetUserIsActiveResponse { UserDto = DtoMapper.ToUserDto(user) });
    }
    
    /// <summary>
    /// Получить PR'ы, где пользователь назначен ревьювером
    /// </summary>
    /// <param name="user_id">Идентификатор пользователя</param>
    /// <returns>Список PR'ов пользователя</returns>
    [HttpGet("getReview")]
    public async Task<IActionResult> GetReview([FromQuery] string user_id)
    {
        var prs = await _userService.GetUserReviewsAsync(user_id);

        var resp = new GetReviewResponse
        {
            User_id = user_id,
            Pull_requests = prs.Select(DtoMapper.ToPullRequestShortDto).ToList()
        };

        return Ok(resp);
    }
    
}