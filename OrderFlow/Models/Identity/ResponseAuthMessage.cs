namespace OrderFlow.Models.Identity;

public class ResponseAuthMessage
{
    public string AccessToken { get; set; }
    public int AccessTokenExpiresIn { get; set; }
    public string RefreshToken { get; set; }
    public int RefreshTokenExpiresIn { get; set; }
    public string Message { get; set; } = string.Empty;
}