#nullable disable
namespace AvitoPRService.Api;

public partial class CreatePullRequestResponse
{

    [Newtonsoft.Json.JsonProperty("pr", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
    public PullRequestDto Pr { get; set; }

}