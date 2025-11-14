using AvitoPRService.Domain.Repositories.Interfaces;
using AvitoPRService.Infrastructure.Repositories.Implementations;

namespace AvitoPRService.DependencyInjection.ServiceExtentions;

public static class AddRepositoryExtension
{
    public static IServiceCollection AddRepository(this IServiceCollection services)
    {
        services.AddScoped<IPullRequestRepository, PullRequestRepository>();
        services.AddScoped<IReviewerRepository, ReviewerRepository>();
        services.AddScoped<ITeamRepository, TeamRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        
        return services;
    }
}