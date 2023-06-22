using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Authentication.API.Exceptions.BadRequest;
using Authentication.API.Model;
using Authentication.API.Services;
using Authentication.API.Services.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Moq;

namespace Authentication.UnitTests.Services;

public class TokenServiceTests
{
    private readonly ITokenService _service;
    private readonly Mock<ApplicationDatabaseContext> _context;
    private readonly Mock<IConfiguration> _configuration;

    public TokenServiceTests()
    {
        _context = new Mock<ApplicationDatabaseContext>();
        _configuration = new Mock<IConfiguration>();
        var logger = new Logger<TokenService>(new LoggerFactory());
        _service = new TokenService(_configuration.Object, logger);
        
    }

    [Fact]
    public void CreateToken_ValidCredentials_ReturnsValidJWTSecurityToken()
    {
        // Arrange
        var jwtSecret = "some jwt secret key";
        var tokenValidityInMinutes = 7;
        var username = "someusername";
        _configuration.Setup<string>(x => x["JWT_SECRET"])
            .Returns(jwtSecret);
        _configuration.Setup<string>(x => x["JWT:TokenValidityInMinutes"])
            .Returns(tokenValidityInMinutes.ToString());
        var authClaims = new List<Claim>()
        {
            new Claim(ClaimTypes.Name, username),
        };

        var expectedToken = CreateJwtToken(jwtSecret, tokenValidityInMinutes, authClaims);
        var tokenHandler = new JwtSecurityTokenHandler();
        // Act
        var securityToken = _service.CreateToken(authClaims);
        var token = tokenHandler.WriteToken(securityToken);
        
        // Assert
        Assert.NotNull(token);
        Assert.NotEmpty(token);
        Assert.Equal(expectedToken, token);
    }

    [Fact]
    public void CreateToken_NotExistingTokenValidityEnvironmentVariable_ThrowsInvalidOperationExceptions()
    {
        // Arrange
        var jwtSecret = "some jwt secret key";
        var tokenValidityInMinutes = "invalid number";
        var username = "someusername";
        _configuration.Setup<string>(x => x["JWT_SECRET"])
            .Returns(jwtSecret);
        _configuration.Setup<string>(conf => conf["JWT:TokenValidityInMinutes"])
            .Returns(tokenValidityInMinutes);
        var authClaims = new List<Claim>()
        {
            new Claim(ClaimTypes.Name, username),
        };
        
        // Act 
        var func = () => _service.CreateToken(authClaims);
        
        // Assert
        Assert.Throws<InvalidOperationException>(func);
    }
    [Fact]
    public void GetPrincipalFromExpiredToken_NonExistingJwtSecretEnvironmentVariable_ThrowsInvalidOperationException()
    {
        // Arrange
        var jwtSecret = "some jwt secret key";
        var tokenValidityInMinutes = 7;
        var username = "someusername";
        _configuration.Setup(conf => conf["JWT_SECRET"])
            .Returns("");
        var authClaims = new List<Claim>()
        {
            new Claim(ClaimTypes.Name, username),
        };
        var token = CreateJwtToken(jwtSecret, tokenValidityInMinutes, authClaims);
        
        // Act
        var func = () => { _service.GetPrincipalFromAccessToken(token); };

        // Assert
        Assert.Throws<InvalidOperationException>(func);
    }
    [Fact]
    public void GetPrincipalFromExpiredToken_AccessTokenIsInvalid_ThrowsInvalidAccessTokenBadRequestException()
    {
        // Arrange
        var jwtSecret = "some jwt secret key";
        var tokenValidityInMinutes = 7;
        var username = "someusername";
        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.Name, username)
        };
        _configuration.Setup(conf => conf["JWT_SECRET"])
            .Returns(jwtSecret);
        var createdToken = CreateJwtToken(jwtSecret, tokenValidityInMinutes, claims);
        var token = createdToken.Reverse().ToString();
        
        // Act
        var func = () => { _service.GetPrincipalFromAccessToken(token); };
        
        // Assert
        Assert.Throws<InvalidAccessTokenBadRequestException>(func);
    }

    [Fact]
    public void GetPrincipalFromExpiredToken_ValidCredentialsAndSecret_ReturnsValidPrincipal()
    {
        // Arrange
        var jwtSecret = "some jwt secret key";
        var tokenValidityInMinutes = 7;
        var username = "someusername";
        
        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.Name, username)
        };
        _configuration.Setup(conf => conf["JWT_SECRET"])
            .Returns(jwtSecret);
        var token = CreateJwtToken(jwtSecret, tokenValidityInMinutes, claims);
        
        // Act
        var (principal, _) = _service.GetPrincipalFromAccessToken(token);
        
        // Assert
        var nameFromPrincipal = principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
        Assert.NotNull(nameFromPrincipal);
        Assert.NotEmpty(nameFromPrincipal);
        Assert.Equal(username, nameFromPrincipal);
    }
    private string CreateJwtToken(string jwtSecret, int tokenValidityInMinutes, List<Claim> authClaims)
    {
        var expiredDateTime = DateTime.Now.AddMinutes(tokenValidityInMinutes);
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret));
        var signingCredentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256);
        var expectedSecurityToken = new JwtSecurityToken(expires: expiredDateTime, claims: authClaims,
            signingCredentials: signingCredentials);

        var tokenHandler = new JwtSecurityTokenHandler();
        
        return tokenHandler.WriteToken(expectedSecurityToken);
    }
}