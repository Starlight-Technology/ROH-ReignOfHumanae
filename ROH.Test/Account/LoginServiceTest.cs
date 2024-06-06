using Moq;

using ROH.Domain.Accounts;
using ROH.Interfaces.Authentication;
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
        LoginModelValidator validator = new();

        Mock<IExceptionHandler> mockExceptionHandler = new();
        Mock<IUserService> mockUserService = new();
        Mock<IAuthService> mockAuthService = new(); 
        mockAuthService.Setup(x => x.GenerateJwtToken(It.IsAny<UserModel>())).Returns("");

        LoginService service = new(mockExceptionHandler.Object, validator, mockUserService.Object, mockAuthService.Object);

        // Act
        DefaultResponse result = await service.Login(new LoginModel());

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, result.HttpStatus);
        Assert.False(string.IsNullOrWhiteSpace(result.Message));
    }

    [Fact]
    public async Task Login_WithNotFoundCredentials_ShouldReturnNotFound()
    {
        // Arrange
        LoginModelValidator validator = new();

        Mock<IExceptionHandler> mockExceptionHandler = new();

        Mock<IUserService> mockUserService = new();
        _ = mockUserService.Setup(x => x.FindUserByEmail(It.IsAny<string>())).ReturnsAsync(() => null);
        _ = mockUserService.Setup(x => x.FindUserByUserName(It.IsAny<string>())).ReturnsAsync(() => null);

        Mock<IAuthService> mockAuthService = new(); 
        mockAuthService.Setup(x => x.GenerateJwtToken(It.IsAny<UserModel>())).Returns("");

        LoginService service = new(mockExceptionHandler.Object, validator, mockUserService.Object, mockAuthService.Object);

        LoginModel loginModel = new()
        {
            Login = "test",
            Password = "test"
        };

        // Act
        DefaultResponse result = await service.Login(loginModel);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, result.HttpStatus);
        Assert.False(string.IsNullOrWhiteSpace(result.Message));
    }

    [Fact]
    public async Task Login_WithInvalidPassword_ShouldReturnError()
    {
        // Arrange
        Guid guidTest = Guid.NewGuid();
        User user = new(1, 1, guidTest, "test", "test");
        user.SetPassword("test");
        UserModel userModelTest = new() { Email = "test@test.com", UserName = "User Name Test", Guid = guidTest };

        LoginModel loginModel = new()
        {
            Login = "test",
            Password = "testwrong"
        };

        LoginModelValidator validator = new();

        Mock<IExceptionHandler> mockExceptionHandler = new();

        Mock<IUserService> mockUserService = new();
        _ = mockUserService.Setup(x => x.FindUserByEmail(It.IsAny<string>())).ReturnsAsync(userModelTest);
        _ = mockUserService.Setup(x => x.FindUserByUserName(It.IsAny<string>())).ReturnsAsync(userModelTest);

        Mock<IAuthService> mockAuthService = new(); 
        mockAuthService.Setup(x => x.GenerateJwtToken(It.IsAny<UserModel>())).Returns("");

        LoginService service = new(mockExceptionHandler.Object, validator, mockUserService.Object, mockAuthService.Object);

        DefaultResponse expected = new(httpStatus: HttpStatusCode.Unauthorized, message: "Invalid password!");

        // Act
        DefaultResponse result = await service.Login(loginModel);

        // Assert
        Assert.Equivalent(expected, result);
    }

    [Fact]
    public async Task Login_WithValidCredentials_ShouldReturnUserModel()
    {
        // Arrange
        string pass = "test";
        Guid guidTest = Guid.NewGuid();
        User user = new(1, 1, guidTest, "test@test.com", "User Name Test");
        user.SetPassword(pass);
        UserModel userModelTest = new() { Email = "test@test.com", UserName = "User Name Test", Guid = guidTest };

        LoginModel loginModel = new()
        {
            Login = "test",
            Password = pass
        };

        LoginModelValidator validator = new();

        Mock<IExceptionHandler> mockExceptionHandler = new();

        Mock<IUserService> mockUserService = new();
        _ = mockUserService.Setup(x => x.FindUserByEmail(It.IsAny<string>())).ReturnsAsync(userModelTest);
        _ = mockUserService.Setup(x => x.FindUserByUserName(It.IsAny<string>())).ReturnsAsync(userModelTest);
        _ = mockUserService.Setup(x => x.ValidatePassword(It.IsAny<string>(), It.IsAny<Guid>())).ReturnsAsync(true);

        Mock<IAuthService> mockAuthService = new(); 
        mockAuthService.Setup(x => x.GenerateJwtToken(It.IsAny<UserModel>())).Returns("");

        LoginService service = new(mockExceptionHandler.Object, validator, mockUserService.Object, mockAuthService.Object);

        DefaultResponse expected = new(objectResponse: new UserModel() { Email = user.Email, UserName = user.UserName, Guid = user.Guid , Token = ""});

        // Act
        DefaultResponse result = await service.Login(loginModel);

        // Assert
        Assert.Equivalent(expected, result);
    }
}
