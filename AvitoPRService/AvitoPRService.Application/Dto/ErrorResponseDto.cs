#nullable disable
namespace AvitoPRService.Api;

public partial class ErrorResponseDto
{

    [Newtonsoft.Json.JsonProperty("error", Required = Newtonsoft.Json.Required.Always)]
    [System.ComponentModel.DataAnnotations.Required]
    public ErrorDto ErrorDto { get; set; } = new ErrorDto();
    
}