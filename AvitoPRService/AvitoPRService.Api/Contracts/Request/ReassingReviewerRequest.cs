#nullable disable
namespace AvitoPRService.Api;

public partial class ReassingReviewerRequest
{

    [Newtonsoft.Json.JsonProperty("pull_request_id", Required = Newtonsoft.Json.Required.Always)]
    [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
    public string Pull_request_id { get; set; }

    [Newtonsoft.Json.JsonProperty("old_user_id", Required = Newtonsoft.Json.Required.Always)]
    [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
    public string Old_user_id { get; set; }
    
}