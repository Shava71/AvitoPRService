#nullable disable
namespace AvitoPRService.Api;

public partial class TeamMemberDto
{
    [Newtonsoft.Json.JsonProperty("user_id", Required = Newtonsoft.Json.Required.Always)]
    [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
    public string User_id { get; set; }

    [Newtonsoft.Json.JsonProperty("username", Required = Newtonsoft.Json.Required.Always)]
    [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
    public string Username { get; set; }

    [Newtonsoft.Json.JsonProperty("is_active", Required = Newtonsoft.Json.Required.Always)]
    public bool Is_active { get; set; }
    
}