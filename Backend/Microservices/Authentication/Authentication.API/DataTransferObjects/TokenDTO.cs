namespace Authentication.API.DataTransferObjects;

public class TokenDTO
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}