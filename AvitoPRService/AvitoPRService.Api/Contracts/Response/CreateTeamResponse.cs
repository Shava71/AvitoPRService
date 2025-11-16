#nullable disable
using AvitoPRService.Application;

namespace AvitoPRService.Api;

public partial class CreateTeamResponse
{

    [Newtonsoft.Json.JsonProperty("team", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
    public TeamDto Team { get; set; }
    
}