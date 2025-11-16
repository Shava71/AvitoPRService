using AvitoPRService.Application.Services.Interfaces;
using AvitoPRService.Domain.Entities;
using AvitoPRService.Domain.Exception;
using AvitoPRService.Domain.Repositories.Interfaces;

namespace AvitoPRService.Application.Services.Implementations;

public class TeamService : ITeamService
{
    private readonly ITeamRepository _teamRepo;
    private readonly IUserRepository _userRepo;
    private readonly IUnitOfWork _unitOfWork;

    public TeamService(ITeamRepository teamRepo, IUserRepository userRepo, IUnitOfWork unitOfWork)
    {
        _teamRepo = teamRepo;
        _userRepo = userRepo;
        _unitOfWork = unitOfWork;
    }
    public async Task<Team> CreateTeamAsync(string teamName, List<(string userId, string username, bool isActive)> members, CancellationToken cancellationToken = default)
    {
        Team? team = await _teamRepo.GetByNameAsync(teamName, cancellationToken);

        if (team != null)
        {
            throw new TeamExistsException(); // такая команда уже существует
        }
        else
        {
            team = new Team(teamName);
            await _teamRepo.AddAsync(team, cancellationToken);
        }
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        foreach ((string userId, string username, bool isActive) member in members)
        {
            User? user = await _userRepo.GetByIdAsync(member.userId, cancellationToken);

            if (user == null)
            {
                user = new User(member.userId, member.username, member.isActive, team);
                await _userRepo.AddAsync(user, cancellationToken);
            }
            else
            {
                user.Username = member.username;
                user.SetActive(member.isActive);
                user.TeamName = teamName;
                user.Team = team;

                await _userRepo.UpdateAsync(user, cancellationToken);
            }

            if (!team.Members.Contains(user))
                team.Members.Add(user);
        }

        await _teamRepo.UpdateAsync(team, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return team;
    }

    public async Task<Team?> GetTeamAsync(string teamName, CancellationToken cancellationToken = default)
    {
        Team? team = await _teamRepo.GetByNameAsync(teamName, cancellationToken);

        if (team == null)
        {
            throw new NotFoundException();
        }
        
        return team;
    }
}