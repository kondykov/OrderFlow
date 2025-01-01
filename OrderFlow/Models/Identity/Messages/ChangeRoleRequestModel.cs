namespace OrderFlow.Models.Identity.Messages;

public class ChangeRoleRequestModel
{
    public required string UserEmail { get; set; }
    public required string RoleName { get; set; }
}