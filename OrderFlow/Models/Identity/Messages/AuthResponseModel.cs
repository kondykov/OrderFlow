namespace OrderFlow.Models.Identity.Messages;

public class AuthResponseModel
{
    public string AccessToken { get; set; }
    public DateTime AccessTokenExpiresIn { get; set; }
    public string RefreshToken { get; set; }
    public DateTime RefreshTokenExpiresIn { get; set; }
}