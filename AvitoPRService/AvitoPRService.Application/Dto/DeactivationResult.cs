namespace AvitoPRService.Application.Dto;

public class DeactivationResult
{
    public List<string> DeactivatedUsers { get; set; } = new();
    public List<PRReassignment> ReassignedPRs { get; set; } = new();
}