using Microsoft.AspNetCore.Mvc;

namespace AvitoPRService.Api.Controllers;

[ApiController]
[Route("users")]
public class UserController : ControllerBase
{
    
    public UserController()
    {
        
    }
    
    /// <summary>
    /// Установить флаг активности пользователя
    /// </summary>
    /// <returns>Обновлённый пользователь</returns>
    [HttpPost("setIsActive")]
    public async Task<Response2> SetIsActive([FromBody] Body body)
    {
        
    }
    
    /// <summary>
    /// Получить PR'ы, где пользователь назначен ревьювером
    /// </summary>
    /// <param name="user_id">Идентификатор пользователя</param>
    /// <returns>Список PR'ов пользователя</returns>
    [HttpGet("getReview")]
    public async Task<Response6> GetReview([FromQuery] string user_id)
    {

    }
    
}