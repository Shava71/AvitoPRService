#nullable disable
using AvitoPRService.Application;

namespace AvitoPRService.Api;

public partial class MergePullRequestResponse
{

    [Newtonsoft.Json.JsonProperty("pr", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
    public PullRequestDto Pr { get; set; }

    
}