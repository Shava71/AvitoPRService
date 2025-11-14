#nullable disable
namespace AvitoPRService.Api;

public partial class TeamDto
{

    [Newtonsoft.Json.JsonProperty("team_name", Required = Newtonsoft.Json.Required.Always)]
    [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
    public string Team_name { get; set; }

    [Newtonsoft.Json.JsonProperty("members", Required = Newtonsoft.Json.Required.Always)]
    [System.ComponentModel.DataAnnotations.Required]
    public System.Collections.Generic.List<TeamMemberDto> Members { get; set; } = new System.Collections.Generic.List<TeamMemberDto>();
    
}