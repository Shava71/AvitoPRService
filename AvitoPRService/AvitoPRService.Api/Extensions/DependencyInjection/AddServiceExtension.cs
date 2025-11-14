using AvitoPRService.Application.Services.Implementations;
using AvitoPRService.Application.Services.Interfaces;

namespace AvitoPRService.DependencyInjection.ServiceExtentions;

public static class AddServiceExtension
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IPullRequestService, PullRequestService>();
        services.AddScoped<ITeamService, TeamService>();
        
        return services;
    }
}