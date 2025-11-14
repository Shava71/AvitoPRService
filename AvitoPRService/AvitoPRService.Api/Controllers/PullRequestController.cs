using Microsoft.AspNetCore.Mvc;

namespace AvitoPRService.Api.Controllers;

[ApiController]
[Route("pullRequest")]
public class PullRequestController : ControllerBase
{

    public PullRequestController()
    {
        
    }
    
    /// <summary>
    /// Создать PR и автоматически назначить до 2 ревьюверов из команды автора
    /// </summary>
    /// <returns>PR создан</returns>
    [HttpPost("create")]
    public async Task<CreatePullRequestResponse> Create([FromBody] CreatePullRequestRequest request)
    {

    }

    /// <summary>
    /// Пометить PR как MERGED (идемпотентная операция)
    /// </summary>
    /// <returns>PR в состоянии MERGED</returns>
    [HttpPost("merge")]
    public async Task<MergePullRequestResponse> Merge([FromBody] MergePullRequestRequest request)
    {

    }

    /// <summary>
    /// Переназначить конкретного ревьювера на другого из его команды
    /// </summary>
    /// <returns>Переназначение выполнено</returns>
    [HttpPost("reassign")]
    public async Task<ReassignReviewerResponse> Reassign([FromBody] ReassingReviewerRequest request)
    {

    }
    
}2