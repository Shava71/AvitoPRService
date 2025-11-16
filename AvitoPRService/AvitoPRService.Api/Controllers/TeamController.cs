using AvitoPRService.Application;
using AvitoPRService.Application.Services.Interfaces;
using AvitoPRService.Domain.Entities;
using AvitoPRService.Mapper;
using Microsoft.AspNetCore.Mvc;

namespace AvitoPRService.Api.Controllers;

[ApiController]
[Route("/team")]
public class TeamController : ControllerBase
{
    private readonly ITeamService _teamService;

    public TeamController(ITeamService teamService)
    {
        _teamService = teamService;
    }
    
    /// <summary>
    /// Создать команду с участниками (создаёт/обновляет пользователей)
    /// </summary>
    /// <returns>Команда создана</returns>
    [HttpPost("add")]
    public async Task<IActionResult> Add([FromBody] TeamDto body)
    {
        // map request DTO -> simple tuple list for service
        var members = body.Members
            .Select(m => (m.User_id, m.Username, m.Is_active))
            .ToList();

        Team team = await _teamService.CreateTeamAsync(body.Team_name, members);

        var resp = new CreateTeamResponse { Team = DtoMapper.ToTeamDto(team) };
        return Created(string.Empty, resp);
    }

    /// <summary>
    /// Получить команду с участниками
    /// </summary>
    /// <param name="team_name">Уникальное имя команды</param>
    /// <returns>Объект команды</returns>
    [HttpGet("get")]
    public async Task<IActionResult> Get([FromQuery] string team_name)
    {
        Team team = await _teamService.GetTeamAsync(team_name);
        
        return Ok(DtoMapper.ToTeamDto(team));
    }
}