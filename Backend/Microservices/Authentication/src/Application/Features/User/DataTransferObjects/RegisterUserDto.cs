namespace Authentication.Application.Features.User.DataTransferObjects;

public class RegisterUserDto
{
    public string Username { get; set; }
    public string EmailAddress { get; set; }
    public string Password { get; set; }
}