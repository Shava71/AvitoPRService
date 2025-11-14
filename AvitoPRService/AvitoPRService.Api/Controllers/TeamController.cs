using Microsoft.AspNetCore.Mvc;

namespace AvitoPRService.Api.Controllers;

[ApiController]
[Route("/team")]
public class TeamController : ControllerBase
{

    public TeamController()
    {
        
    }
    
    /// <summary>
    /// Создать команду с участниками (создаёт/обновляет пользователей)
    /// </summary>
    /// <returns>Команда создана</returns>
    [HttpPost("add")]
    public async Task<IActionResult<CreateTeamResponse>> Add([FromBody] TeamDto body)
    {

    }

    /// <summary>
    /// Получить команду с участниками
    /// </summary>
    /// <param name="team_name">Уникальное имя команды</param>
    /// <returns>Объект команды</returns>
    [HttpGet("get")]
    public async Task<IActionResult<TeamDto>> Get([FromQuery] string team_name)
    {

    }
}