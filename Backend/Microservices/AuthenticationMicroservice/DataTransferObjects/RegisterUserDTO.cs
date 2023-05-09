using System.ComponentModel.DataAnnotations;

namespace AuthenticationMicroservice.DataTransferObjects;

public class RegisterUserDTO
{
    public string Username { get; set; }
    public string EmailAddress { get; set; }
    public string Password { get; set; }
}