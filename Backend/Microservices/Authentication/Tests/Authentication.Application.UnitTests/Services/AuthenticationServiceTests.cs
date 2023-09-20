using System.Security.Claims;
using Authentication.Application.Common.Exceptions.BadRequest;
using Authentication.Application.Common.Exceptions.Unauthorized;
using Authentication.Application.Extensions.Services;
using Authentication.Application.Extensions.Services.Contracts;
using Authentication.Application.Features.User.DataTransferObjects;
using Authentication.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Authentication.Application.UnitTests.Services;

public class AuthenticationServiceTests
{
    private readonly IAuthenticationService _service;
    private readonly Mock<UserManager<User>> _userManager;
    private readonly Mock<ITokenService> _tokenService;
    private readonly Mock<IConfiguration> _configuration;

    public AuthenticationServiceTests()
    {
        _userManager = new Mock<UserManager<User>>(
            Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);
        _configuration = new Mock<IConfiguration>();
        
        var tokenServiceLogger = new Logger<TokenService>(new LoggerFactory());
        var authenticationServiceLogger = new Logger<AuthenticationService>(new LoggerFactory());

        _tokenService = new Mock<ITokenService>();
        _service = new AuthenticationService(
            _userManager.Object,
            _tokenService.Object,
            _configuration.Object,
            authenticationServiceLogger);
    }

    [Fact]
    public async Task RegisterUser_CreateUserWithValidCredentials_ReturnsSucceededIdentityResult()
    {
        // Arrange
        var dto = new RegisterUserDto()
        {
            Username = It.IsAny<string>(),
            Password = It.IsAny<string>(),
            EmailAddress = It.IsAny<string>(),
        };
        _userManager.Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
            .Returns(Task.FromResult(IdentityResult.Success));

        // Act
        var result = await _service.RegisterUserAsync(dto);
        
        // Assert
        Assert.True(result.Succeeded);
    }
    
    [Fact]
    public async Task RegisterUser_CreateUserWithInvalidCredentials_ReturnsFailedIdentityResult()
    {
        // Arrange
        var dto = new RegisterUserDto()
        {
            Username = It.IsAny<string>(),
            Password = It.IsAny<string>(),
            EmailAddress = It.IsAny<string>(),
        };
        _userManager.Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Failed());

        // Act
        var result = await _service.RegisterUserAsync(dto);
        
        // Assert
        Assert.False(result.Succeeded);
    }

    [Fact]
    public async Task LoginUser_LoginWithNotExistingUsername_ThrowsInvalidUserNameUnauthorized()
    {
        // Arrange
        var dto = new LoginUserDto()
        {
            Username = It.IsAny<string>(),
            Password = It.IsAny<string>(),
        };
        _userManager.Setup(x => x.FindByNameAsync(It.IsAny<string>()))
            .ReturnsAsync((User)null);

        // Act
        var func = async () => await _service.LoginUserAsync(dto);
        
        // Assert
        await Assert.ThrowsAsync<InvalidUserNameUnauthorized>(func);
    }

    [Fact]
    public async Task
        LoginUser_LoginWithInvalidPassword_ThrowsInvalidUserPasswordUnauthorized()
    {
        // Arrange
        var dto = new LoginUserDto()
        {
            Username = It.IsAny<string>(),
            Password = It.IsAny<string>(),
        };

        _userManager.Setup(x => x.FindByNameAsync(It.IsAny<string>()))
            .ReturnsAsync(new User());
        _userManager.Setup(x => x.CheckPasswordAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(false);
        
        // Act
        var func = async () => await _service.LoginUserAsync(dto);
        
        // Assert
        await Assert.ThrowsAsync<InvalidUserPasswordUnauthorized>(func);
    }
    [Fact]
    public async Task
        LoginUser_InvalidRefreshTokenValidityInDaysEnvironmentVariable_ThrowsInvalidOperationException()
    {
        // Arrange
        var dto = new LoginUserDto()
        {
            Username = It.IsAny<string>(),
            Password = It.IsAny<string>(),
        };
        var refreshTokenValidity = "invalid refresh token validity";
        
        _userManager.Setup(x => x.FindByNameAsync(It.IsAny<string>()))
            .ReturnsAsync(new User(){UserName = "username"});
        _userManager.Setup(x => x.CheckPasswordAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(true);
        _configuration.Setup(x => x["JWT:RefreshTokenValidityInDays"])
            .Returns(refreshTokenValidity);
        // Act
        var func = async () => await _service.LoginUserAsync(dto);
        
        // Assert
        await Assert.ThrowsAsync<InvalidOperationException>(func);
    }
    [Fact]
    public async Task
        LoginUser_NotPresentTokenValidityInMinutesEnvironmentVariable_ThrowsInvalidOperationException()
    {
        // Arrange
        var dto = new LoginUserDto()
        {
            Username = It.IsAny<string>(),
            Password = It.IsAny<string>(),
        };
        
        _userManager.Setup(x => x.FindByNameAsync(It.IsAny<string>()))
            .ReturnsAsync(new User(){UserName = "username"});
        _userManager.Setup(x => x.CheckPasswordAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(true);
        _configuration.Setup(x => x["JWT:RefreshTokenValidityInDays"])
            .Returns(string.Empty);
        // Act
        var func = async () => await _service.LoginUserAsync(dto);
        
        // Assert
        await Assert.ThrowsAsync<InvalidOperationException>(func);
    }
    
    [Fact]
    public async Task
        GetRefreshToken_InvalidAccessToken_ThrowsInvalidAccessTokenBadRequestException()
    {
        // Arrange
        var dto = new TokenDto()
        {
            AccessToken = It.IsAny<string>(),
            RefreshToken = It.IsAny<string>(),
        };
        var claimsPrincipal = new ClaimsPrincipal();
        _tokenService.Setup(x => x.GetPrincipalFromAccessToken(It.IsAny<string>()))
            .Returns((claimsPrincipal, new DateTime()));
        _userManager.Setup(x => x.FindByNameAsync(It.IsAny<string>()))
            .ReturnsAsync((User) null);
        
        // Act
        var func = async () => await _service.GetRefreshTokenAsync(dto);
        
        // Assert
        await Assert.ThrowsAsync<InvalidAccessTokenBadRequestException>(func);
    }
    [Fact]
    public async Task
        GetRefreshToken_InvalidRefreshToken_ThrowsInvalidRefreshTokenBadRequestException()
    {
        // Arrange
        var refreshToken = "refreshToken";
        var dto = new TokenDto()
        {
            AccessToken = It.IsAny<string>(),
            RefreshToken = refreshToken,
        };
        var claimsPrincipal = new ClaimsPrincipal();
        _tokenService.Setup(x => x.GetPrincipalFromAccessToken(It.IsAny<string>()))
            .Returns((claimsPrincipal, new DateTime()));
        _userManager.Setup(x => x.FindByNameAsync(It.IsAny<string>()))
            .ReturnsAsync(new User()
            {
                RefreshToken = $"invalid {refreshToken}"
            });
        
        // Act
        var func = async () => await _service.GetRefreshTokenAsync(dto);
        
        // Assert
        await Assert.ThrowsAsync<InvalidRefreshTokenBadRequestException>(func);
    }
    [Fact]
    public async Task
        GetRefreshToken_ExpiredRefreshToken_ThrowsRefreshTokenIsExpiredBadRequest()
    {
        // Arrange
        var refreshToken = "refreshToken";
        var invalidRefreshTokenExpiryDate = DateTime.Now.AddDays(-1);
        var dto = new TokenDto()
        {
            AccessToken = It.IsAny<string>(),
            RefreshToken = refreshToken,
        };
        var claimsPrincipal = new ClaimsPrincipal();
        _tokenService.Setup(x => x.GetPrincipalFromAccessToken(It.IsAny<string>()))
            .Returns((claimsPrincipal, new DateTime()));
        _userManager.Setup(x => x.FindByNameAsync(It.IsAny<string>()))
            .ReturnsAsync(new User()
            {
                RefreshToken = refreshToken,
                RefreshTokenExpiryTime = invalidRefreshTokenExpiryDate,
            });
        
        // Act
        var func = async () => await _service.GetRefreshTokenAsync(dto);
        
        // Assert
        await Assert.ThrowsAsync<RefreshTokenIsExpiredBadRequest>(func);
    }
}