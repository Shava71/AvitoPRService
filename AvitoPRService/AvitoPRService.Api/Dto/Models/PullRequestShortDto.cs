#nullable disable
namespace AvitoPRService.Api;

public partial class PullRequestShortDto
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
    public PullRequestShortStatus Status { get; set; }


}