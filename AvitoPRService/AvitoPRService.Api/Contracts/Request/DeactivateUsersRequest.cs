namespace AvitoPRService.Api;

public class DeactivateUsersRequest
{
    public List<string> UserIds { get; set; } = new List<string>();
    public bool ReassignOpenPRs { get; set; } = true;
}