#nullable disable
using AvitoPRService.Application;

namespace AvitoPRService.Api;

public partial class SetUserIsActiveResponse
{

    [Newtonsoft.Json.JsonProperty("user", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
    public UserDto UserDto { get; set; }

}