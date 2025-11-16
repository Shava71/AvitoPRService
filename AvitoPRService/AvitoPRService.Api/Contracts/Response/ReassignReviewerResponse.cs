#nullable disable
using AvitoPRService.Application;

namespace AvitoPRService.Api;

public partial class ReassignReviewerResponse
{

    [Newtonsoft.Json.JsonProperty("pr", Required = Newtonsoft.Json.Required.Always)]
    [System.ComponentModel.DataAnnotations.Required]
    public PullRequestDto Pr { get; set; } = new PullRequestDto();

    /// <summary>
    /// user_id нового ревьювера
    /// </summary>
    [Newtonsoft.Json.JsonProperty("replaced_by", Required = Newtonsoft.Json.Required.Always)]
    [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
    public string Replaced_by { get; set; }
    

}