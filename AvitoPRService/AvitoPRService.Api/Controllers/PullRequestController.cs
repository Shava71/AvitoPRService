using AvitoPRService.Application.Services.Interfaces;
using AvitoPRService.Mapper;
using Microsoft.AspNetCore.Mvc;

namespace AvitoPRService.Api.Controllers;

[ApiController]
[Route("pullRequest")]
public class PullRequestController : ControllerBase
{
    private readonly IPullRequestService _pullRequestService;


    public PullRequestController(IPullRequestService pullRequestService)
    {
        _pullRequestService = pullRequestService;
    }
    
    /// <summary>
    /// Создать PR и автоматически назначить до 2 ревьюверов из команды автора
    /// </summary>
    /// <returns>PR создан</returns>
    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] CreatePullRequestRequest request)
    {
        var pr = await _pullRequestService.CreateAsync(request.Pull_request_id, request.Pull_request_name, request.Author_id);
        return Created(string.Empty, new CreatePullRequestResponse { Pr = DtoMapper.ToPullRequestDto(pr) });
    }

    /// <summary>
    /// Пометить PR как MERGED (идемпотентная операция)
    /// </summary>
    /// <returns>PR в состоянии MERGED</returns>
    [HttpPost("merge")]
    public async Task<IActionResult> Merge([FromBody] MergePullRequestRequest request)
    {
        var pr = await _pullRequestService.MergeAsync(request.Pull_request_id);
        return Ok(new MergePullRequestResponse { Pr = DtoMapper.ToPullRequestDto(pr) });
    }

    /// <summary>
    /// Переназначить конкретного ревьювера на другого из его команды
    /// </summary>
    /// <returns>Переназначение выполнено</returns>
    [HttpPost("reassign")]
    public async Task<IActionResult> Reassign([FromBody] ReassingReviewerRequest request)
    {
        var pr = await _pullRequestService.ReassignReviewerAsync(request.Pull_request_id, request.Old_user_id);
        
        var replacedBy = pr.Reviewers.Select(r => r.UserId).FirstOrDefault(u => u != request.Old_user_id);

        return Ok(new ReassignReviewerResponse { Pr = DtoMapper.ToPullRequestDto(pr), Replaced_by = replacedBy ?? string.Empty });
    }
    
}