#nullable disable
namespace AvitoPRService.Api;


public partial class ErrorDto
{

    [Newtonsoft.Json.JsonProperty("code", Required = Newtonsoft.Json.Required.Always)]
    [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
    [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public ErrorCode Code { get; set; }

    [Newtonsoft.Json.JsonProperty("message", Required = Newtonsoft.Json.Required.Always)]
    [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
    public string Message { get; set; }

}