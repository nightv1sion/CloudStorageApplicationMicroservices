using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
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
    private readonly string _jwtSecret;
    private readonly int _tokenValidityInMinutes;
    private readonly string _username;

    public TokenServiceTests()
    {
        _context = new Mock<ApplicationDatabaseContext>();
        _configuration = new Mock<IConfiguration>();
        var logger = new Logger<TokenService>(new LoggerFactory());
        _service = new TokenService(_configuration.Object, logger);
        _jwtSecret = "some jwt secret key";
        _tokenValidityInMinutes = 7;
        _username = "someusername";
    }

    [Fact]
    public void TokenService_CreateToken_ReturnsValidJWTSecurityToken()
    {
        // Arrange
        _configuration.Setup<string>(x => x["JWT_SECRET"])
            .Returns(_jwtSecret);
        _configuration.Setup<string>(x => x["JWT:TokenValidityInMinutes"])
            .Returns(_tokenValidityInMinutes.ToString());
        var authClaims = new List<Claim>()
        {
            new Claim(ClaimTypes.Name, _username),
        };

        var expectedToken = CreateJwtToken(_jwtSecret, _tokenValidityInMinutes, authClaims);
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
    public void TokenService_CreateToken_ThrowsInvalidOperationExceptions_WhenTokenValidityInMinutesIsNotPresent()
    {
        // Arrange
        _configuration.Setup<string>(x => x["JWT_SECRET"])
            .Returns(_jwtSecret);
        _configuration.Setup(conf => conf["JWT:TokenValidityInMinutes"])
            .Returns(_tokenValidityInMinutes.ToString());
        var authClaims = new List<Claim>()
        {
            new Claim(ClaimTypes.Name, _username),
        };
        
        // Act 
        var func = () => _service.CreateToken(authClaims);
        
        // Assert
        Assert.Throws<InvalidOperationException>(func);
    }
    [Fact]
    public void TokenService_GetPrincipalFromExpiredToken_ThrowsException_WhenJwtSecretIsNotPresent()
    {
        // Arrange
        _configuration.Setup(conf => conf["JWT_SECRET"])
            .Returns("");
        var authClaims = new List<Claim>()
        {
            new Claim(ClaimTypes.Name, _username),
        };
        var token = CreateJwtToken(_jwtSecret, _tokenValidityInMinutes, authClaims);
        
        // Act
        var func = () => _service.GetPrincipaFromExpiredToken(token);

        // Assert
        Assert.Throws<InvalidOperationException>(func);
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