using AvitoPRService.Application.Dto;
using AvitoPRService.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AvitoPRService.Api.Controllers;

[ApiController]
[Route("stats")]
public class StatsController : ControllerBase
{
    private readonly IStatsService _statsService;

    public StatsController(IStatsService statsService)
    {
        _statsService = statsService;
    }

    /// <summary>
    /// Получить статистику по PR и назначениям
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetStats()
    {
        StatsDto stats = await _statsService.GetStatsAsync();
        return Ok(stats);
    }
}