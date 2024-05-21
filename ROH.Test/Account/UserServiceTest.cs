using Moq;

using ROH.Domain.Accounts;
using ROH.Interfaces.Repository.Account;
using ROH.Interfaces.Services.ExceptionService;
using ROH.Services.Account;
using ROH.StandardModels.Account;
using ROH.StandardModels.Response;
using ROH.Validations.Account;

using System.Net;

namespace ROH.Test.Account;
public class UserServiceTest
{
    [Fact]
    public async Task NewUser_ShouldReturn_Error_WhenUserModelNotValid()
    {
        // Arrange
        UserModel userModel = new();

        UserModelValidation userValidator = new();

        Mock<IExceptionHandler> mockExceptionHandler = new();

        Mock<IUserRepository> mockRepository = new();

        UserService service = new(mockExceptionHandler.Object, userValidator, mockRepository.Object);

        // Act
        DefaultResponse result = await service.NewUser(userModel);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, result.HttpStatus);
        Assert.False(string.IsNullOrWhiteSpace(result.Message));

    }

    [Fact]
    public async Task NewUser_ShouldReturn_Error_WhenEmailAreAlreadyUsed()
    {
        // Arrange
        UserModel userModel = new() { Email = "test.email@test.com", Password = "test123" };

        UserModelValidation userValidator = new();

        Mock<IExceptionHandler> mockExceptionHandler = new();

        Mock<IUserRepository> mockRepository = new();
        _ = mockRepository.Setup(x => x.EmailInUse("test.email@test.com")).ReturnsAsync(true);

        UserService service = new(mockExceptionHandler.Object, userValidator, mockRepository.Object);

        DefaultResponse expected = new(httpStatus: HttpStatusCode.Conflict, message: "The email are currently in use.");

        // Act
        DefaultResponse result = await service.NewUser(userModel);

        // Assert

        Assert.Equivalent(expected, result);
    }

    [Fact]
    public async Task NewUser_ShouldReturn_NewUser_WhenNewUserIsValid()
    {
        // Arrange 
        UserModel userModel = new() { Email = "test.email@test.com", Password = "test123" };

        UserModelValidation userValidator = new();

        Mock<IExceptionHandler> mockExceptionHandler = new();

        Mock<IUserRepository> mockRepository = new();
        _ = mockRepository.Setup(x => x.CreateNewUser(It.IsAny<User>())).ReturnsAsync(new User());

        UserService service = new(mockExceptionHandler.Object, userValidator, mockRepository.Object);

        DefaultResponse expected = new(httpStatus: HttpStatusCode.OK);

        // Act
        DefaultResponse result = await service.NewUser(userModel);

        // Assert

        Assert.Equivalent(expected, result);
    }
}
