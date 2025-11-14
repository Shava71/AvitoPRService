#nullable disable
namespace AvitoPRService.Api;

public partial class MergePullRequestRequest
{

    [Newtonsoft.Json.JsonProperty("pull_request_id", Required = Newtonsoft.Json.Required.Always)]
    [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
    public string Pull_request_id { get; set; }
    
}