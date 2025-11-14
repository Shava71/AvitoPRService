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
    public async Task<Response3> Create([FromBody] Body2 body)
    {

    }

    /// <summary>
    /// Пометить PR как MERGED (идемпотентная операция)
    /// </summary>
    /// <returns>PR в состоянии MERGED</returns>
    [HttpPost("pullRequest/merge")]
    public System.Threading.Tasks.Task<Response4> Merge([Microsoft.AspNetCore.Mvc.FromBody] Body3 body)
    {

    }

    /// <summary>
    /// Переназначить конкретного ревьювера на другого из его команды
    /// </summary>
    /// <returns>Переназначение выполнено</returns>
    [HttpPost("pullRequest/reassign")]
    public async Task<Response5> Reassign([FromBody] Body4 body)
    {

    }
    
}