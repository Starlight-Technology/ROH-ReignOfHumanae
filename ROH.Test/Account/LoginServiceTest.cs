using Moq;

using ROH.Domain.Accounts;
using ROH.Interfaces.Services.Account;
using ROH.Interfaces.Services.ExceptionService;
using ROH.Services.Account;
using ROH.StandardModels.Account;
using ROH.StandardModels.Response;
using ROH.Validations.Account;

using System.Net;

namespace ROH.Test.Account;
public class LoginServiceTest
{
    [Fact]
    public async Task Login_WithEmptyCredentials_ShouldReturnError()
    {
        // Arrange
        var validator = new LoginModelValidator();

        Mock<IExceptionHandler> mockExceptionHandler = new();
        Mock<IUserService> mockUserService = new();

        var service = new LoginService(mockExceptionHandler.Object, validator, mockUserService.Object);

        // Act
        var result = await service.Login(new LoginModel());

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, result.HttpStatus);
        Assert.False(string.IsNullOrWhiteSpace(result.Message));
    }

    [Fact]
    public async Task Login_WithNotFoundCredentials_ShouldReturnNotFound()
    {
        // Arrange
        var validator = new LoginModelValidator();

        Mock<IExceptionHandler> mockExceptionHandler = new();

        Mock<IUserService> mockUserService = new();
        mockUserService.Setup(x => x.FindUserByEmail(It.IsAny<string>())).ReturnsAsync(() => null);
        mockUserService.Setup(x => x.FindUserByUserName(It.IsAny<string>())).ReturnsAsync(() => null);

        var service = new LoginService(mockExceptionHandler.Object, validator, mockUserService.Object);

        var loginModel = new LoginModel()
        {
            Login = "test",
            Password = "test"
        };

        // Act
        var result = await service.Login(loginModel);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, result.HttpStatus);
        Assert.False(string.IsNullOrWhiteSpace(result.Message));
    }

    [Fact]
    public async Task Login_WithValidCredentials_ShouldReturnUserModel()
    {
        // Arrange
        User user = new User(1,1, Guid.NewGuid(), "test", "test");
        UserModel userModel = new () 
        {
            Email = user.Email,
            Guid = user.Guid,
            UserName = user.UserName
        };

        var validator = new LoginModelValidator();

        Mock<IExceptionHandler> mockExceptionHandler = new();

        Mock<IUserService> mockUserService = new();
        mockUserService.Setup(x => x.FindUserByEmail(It.IsAny<string>())).ReturnsAsync(user);
        mockUserService.Setup(x => x.FindUserByUserName(It.IsAny<string>())).ReturnsAsync(user);

        var service = new LoginService(mockExceptionHandler.Object, validator, mockUserService.Object);

        var loginModel = new LoginModel()
        {
            Login = "test",
            Password = "test"
        };

        var expected = new DefaultResponse(objectResponse: new UserModel() { Email = user.Email, UserName = user.UserName, Guid = user.Guid });

        // Act
        var result = await service.Login(loginModel);

        // Assert
        Assert.Equivalent(expected, result);
    }
}
