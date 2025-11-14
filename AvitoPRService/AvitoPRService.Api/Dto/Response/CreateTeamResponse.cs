#nullable disable
namespace AvitoPRService.Api;

public partial class CreateTeamResponse
{

    [Newtonsoft.Json.JsonProperty("team", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
    public TeamDto TeamDto { get; set; }
    
}