#nullable disable
namespace AvitoPRService.Api;

public partial class PullRequestDto
{

    [Newtonsoft.Json.JsonProperty("pull_request_id", Required = Newtonsoft.Json.Required.Always)]
    [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
    public string Pull_request_id { get; set; }

    [Newtonsoft.Json.JsonProperty("pull_request_name", Required = Newtonsoft.Json.Required.Always)]
    [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
    public string Pull_request_name { get; set; }

    [Newtonsoft.Json.JsonProperty("author_id", Required = Newtonsoft.Json.Required.Always)]
    [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
    public string Author_id { get; set; }

    [Newtonsoft.Json.JsonProperty("status", Required = Newtonsoft.Json.Required.Always)]
    [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
    [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public PullRequestStatusDto StatusDto { get; set; }

    /// <summary>
    /// user_id назначенных ревьюверов (0..2)
    /// </summary>
    [Newtonsoft.Json.JsonProperty("assigned_reviewers", Required = Newtonsoft.Json.Required.Always)]
    [System.ComponentModel.DataAnnotations.Required]
    public System.Collections.Generic.List<string> Assigned_reviewers { get; set; } = new System.Collections.Generic.List<string>();

    [Newtonsoft.Json.JsonProperty("createdAt", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
    public System.DateTimeOffset? CreatedAt { get; set; }

    [Newtonsoft.Json.JsonProperty("mergedAt", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
    public System.DateTimeOffset? MergedAt { get; set; }
    
}