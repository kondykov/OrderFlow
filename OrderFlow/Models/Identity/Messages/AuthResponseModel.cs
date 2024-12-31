namespace OrderFlow.Models.Identity.Messages;

public class AuthResponseModel
{
    public string AccessToken { get; set; }
    public string? AccessTokenExpiresIn { get; set; }
    public string RefreshToken { get; set; }
    public string RefreshTokenExpiresIn { get; set; }
}