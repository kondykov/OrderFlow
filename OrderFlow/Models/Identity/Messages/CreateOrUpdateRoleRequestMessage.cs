namespace OrderFlow.Models.Identity.Messages;

public class CreateOrUpdateRoleRequestMessage
{
    public string RoleName { get; set; }
    public string NormalizedRoleName { get; set; }
}