namespace OrderFlow.Models.Identity.Messages.Response;

public class ResponseAuthMessage : ResponseMessage
{
    public string AccessToken { get; set; }
    public string? AccessTokenExpiresIn { get; set; }
    public string RefreshToken { get; set; }
    public string RefreshTokenExpiresIn { get; set; }
}