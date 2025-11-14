#nullable disable
namespace AvitoPRService.Api;

public partial class GetReviewResponse
{

    [Newtonsoft.Json.JsonProperty("user_id", Required = Newtonsoft.Json.Required.Always)]
    [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
    public string User_id { get; set; }

    [Newtonsoft.Json.JsonProperty("pull_requests", Required = Newtonsoft.Json.Required.Always)]
    [System.ComponentModel.DataAnnotations.Required]
    public System.Collections.Generic.List<PullRequestShortDto> Pull_requests { get; set; } = new System.Collections.Generic.List<PullRequestShortDto>();
    
}