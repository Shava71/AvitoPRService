using AvitoPRService.Application.Dto;

namespace AvitoPRService.Application.Services.Interfaces;

public interface IStatsService
{
    Task<StatsDto> GetStatsAsync(CancellationToken cancellationToken = default);
}