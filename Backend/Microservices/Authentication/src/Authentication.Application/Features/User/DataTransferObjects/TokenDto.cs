namespace Authentication.Application.Features.User.DataTransferObjects;

public class TokenDto
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}