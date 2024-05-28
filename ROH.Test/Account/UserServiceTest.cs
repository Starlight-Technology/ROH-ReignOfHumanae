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

        UserModelValidator userValidator = new();

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

        UserModelValidator userValidator = new();

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

        UserModelValidator userValidator = new();

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

    [Fact]
    public async Task FindUserByEmail_ShoulReturn_Null_WhenUserNotFound()
    {
        // Arrange 
        UserModelValidator userValidator = new();

        Mock<IExceptionHandler> mockExceptionHandler = new();

        Mock<IUserRepository> mockRepository = new();
        _ = mockRepository.Setup(x => x.FindUserByEmail(It.IsAny<string>())).ReturnsAsync(() => null);

        UserService service = new(mockExceptionHandler.Object, userValidator, mockRepository.Object);

        // Act
        User? result = await service.FindUserByEmail("test");

        // Assert

        Assert.Null(result);
    }

    [Fact]
    public async Task FindUserByEmail_ShoulReturn_User_WhenUserFound()
    {
        // Arrange 
        User userTest = new(Id: 1, IdAccount: 1, Email: "test@test.com", UserName: "User Name Test", Guid: Guid.NewGuid());

        UserModelValidator userValidator = new();

        Mock<IExceptionHandler> mockExceptionHandler = new();

        Mock<IUserRepository> mockRepository = new();
        _ = mockRepository.Setup(x => x.FindUserByEmail(It.IsAny<string>())).ReturnsAsync(userTest);

        UserService service = new(mockExceptionHandler.Object, userValidator, mockRepository.Object);

        // Act
        User? result = await service.FindUserByEmail("test");

        // Assert

        Assert.Equal(result, userTest);
    }

    [Fact]
    public async Task FindUserByUserName_ShoulReturn_Null_WhenUserNotFound()
    {
        // Arrange 

        UserModelValidator userValidator = new();

        Mock<IExceptionHandler> mockExceptionHandler = new();

        Mock<IUserRepository> mockRepository = new();
        _ = mockRepository.Setup(x => x.FindUserByUserName(It.IsAny<string>())).ReturnsAsync(() => null);

        UserService service = new(mockExceptionHandler.Object, userValidator, mockRepository.Object);

        // Act
        User? result = await service.FindUserByUserName("test");

        // Assert

        Assert.Null(result);
    }

    [Fact]
    public async Task FindUserByUserName_ShoulReturn_User_WhenUserFound()
    {
        // Arrange 
        User userTest = new(Id: 1, IdAccount: 1, Email: "test@test.com", UserName: "User Name Test", Guid: Guid.NewGuid());

        UserModelValidator userValidator = new();

        Mock<IExceptionHandler> mockExceptionHandler = new();

        Mock<IUserRepository> mockRepository = new();
        _ = mockRepository.Setup(x => x.FindUserByUserName(It.IsAny<string>())).ReturnsAsync(userTest);

        UserService service = new(mockExceptionHandler.Object, userValidator, mockRepository.Object);

        // Act
        User? result = await service.FindUserByUserName("test");

        // Assert

        Assert.Equal(result, userTest);
    }

    [Fact]
    public async Task FindUserByGuid_ShoulReturn_User()
    {
        // Arrange 
        var guidTest = Guid.NewGuid();
        User userTest = new(Id: 1, IdAccount: 1, Email: "test@test.com", UserName: "User Name Test", Guid: guidTest);

        UserModelValidator userValidator = new();

        Mock<IExceptionHandler> mockExceptionHandler = new();

        Mock<IUserRepository> mockRepository = new();
        _ = mockRepository.Setup(x => x.GetUserByGuid(It.IsAny<Guid>())).ReturnsAsync(userTest);

        UserService service = new(mockExceptionHandler.Object, userValidator, mockRepository.Object);

        // Act
        User? result = await service.GetUserByGuid(guidTest);

        // Assert

        Assert.Equal(result, userTest);
    }
}
