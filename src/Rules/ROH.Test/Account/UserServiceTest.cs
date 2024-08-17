using AutoMapper;

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
    private readonly IMapper _mapper;
    private readonly Mock<IExceptionHandler> _mockExceptionHandler;
    private readonly Mock<IUserRepository> _mockRepository;
    private readonly UserModelValidator _userValidator;

    public UserServiceTest()
    {
        MapperConfiguration config = new(cfg =>
        {
            _ = cfg.CreateMap<Domain.Accounts.Account, AccountModel>()
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.BirthDate.ToDateTime(new TimeOnly(0, 0))));
            _ = cfg.CreateMap<AccountModel, Domain.Accounts.Account>()
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.BirthDate)));

            _ = cfg.CreateMap<User, UserModel>().ReverseMap();
        });

        _mapper = new Mapper(config);
        _mockExceptionHandler = new Mock<IExceptionHandler>();
        _mockRepository = new Mock<IUserRepository>();
        _userValidator = new UserModelValidator();
    }

    [Fact]
    public async Task NewUser_ShouldReturn_Error_WhenUserModelNotValid()
    {
        // Arrange
        UserService service = new(_mockExceptionHandler.Object, _userValidator, _mockRepository.Object, _mapper);
        UserModel userModel = new();

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
        UserService service = new(_mockExceptionHandler.Object, _userValidator, _mockRepository.Object, _mapper);
        UserModel userModel = new() { Email = "test.email@test.com", Password = "test123" };
        _ = _mockRepository.Setup(x => x.EmailInUse("test.email@test.com")).ReturnsAsync(true);

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
        UserService service = new(_mockExceptionHandler.Object, _userValidator, _mockRepository.Object, _mapper);
        UserModel userModel = new() { Email = "test.email@test.com", Password = "test123" };
        _ = _mockRepository.Setup(x => x.CreateNewUser(It.IsAny<User>())).ReturnsAsync(new User());

        DefaultResponse expected = new(httpStatus: HttpStatusCode.OK, message:"Account has been created!");

        // Act
        DefaultResponse result = await service.NewUser(userModel);

        // Assert
        Assert.Equivalent(expected, result);
    }

    [Fact]
    public async Task NewUser_ShouldHandle_Exception()
    {
        // Arrange
        UserService service = new(_mockExceptionHandler.Object, _userValidator, _mockRepository.Object, _mapper);
        UserModel userModel = new() { Email = "test.email@test.com", Password = "test123" };
        _ = _mockRepository.Setup(x => x.CreateNewUser(It.IsAny<User>())).ThrowsAsync(new Exception("Database error"));

        _mockExceptionHandler.Setup(x => x.HandleException(It.IsAny<Exception>()))
            .Returns(new DefaultResponse(httpStatus: HttpStatusCode.InternalServerError, message: "Internal Server Error"));

        // Act
        DefaultResponse result = await service.NewUser(userModel);

        // Assert
        Assert.Equal(HttpStatusCode.InternalServerError, result.HttpStatus);
        _mockExceptionHandler.Verify(x => x.HandleException(It.IsAny<Exception>()), Times.Once);
    }

    [Fact]
    public async Task FindUserByEmail_ShouldReturn_Null_WhenUserNotFound()
    {
        // Arrange
        UserService service = new(_mockExceptionHandler.Object, _userValidator, _mockRepository.Object, _mapper);
        _ = _mockRepository.Setup(x => x.FindUserByEmail(It.IsAny<string>())).ReturnsAsync(() => null);

        // Act
        UserModel? result = await service.FindUserByEmail("test");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task FindUserByEmail_ShouldReturn_User_WhenUserFound()
    {
        // Arrange
        UserService service = new(_mockExceptionHandler.Object, _userValidator, _mockRepository.Object, _mapper);
        Guid guidTest = Guid.NewGuid();
        User userTest = new(Id: 1, IdAccount: 1, Email: "test@test.com", UserName: "User Name Test", Guid: guidTest);
        UserModel userModelTest = new() { Email = "test@test.com", UserName = "User Name Test", Guid = guidTest };
        _ = _mockRepository.Setup(x => x.FindUserByEmail(It.IsAny<string>())).ReturnsAsync(userTest);

        // Act
        UserModel? result = await service.FindUserByEmail("test");

        // Assert
        Assert.Equivalent(result, userModelTest);
    }

    [Fact]
    public async Task FindUserByUserName_ShouldReturn_Null_WhenUserNotFound()
    {
        // Arrange
        UserService service = new(_mockExceptionHandler.Object, _userValidator, _mockRepository.Object, _mapper);
        _ = _mockRepository.Setup(x => x.FindUserByUserName(It.IsAny<string>())).ReturnsAsync(() => null);

        // Act
        UserModel? result = await service.FindUserByUserName("test");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task FindUserByUserName_ShouldReturn_User_WhenUserFound()
    {
        // Arrange
        UserService service = new(_mockExceptionHandler.Object, _userValidator, _mockRepository.Object, _mapper);
        Guid guidTest = Guid.NewGuid();
        User userTest = new(Id: 1, IdAccount: 1, Email: "test@test.com", UserName: "User Name Test", Guid: guidTest);
        UserModel userModelTest = new() { Email = "test@test.com", UserName = "User Name Test", Guid = guidTest };
        _ = _mockRepository.Setup(x => x.FindUserByUserName(It.IsAny<string>())).ReturnsAsync(userTest);

        // Act
        UserModel? result = await service.FindUserByUserName("test");

        // Assert
        Assert.Equivalent(result, userModelTest);
    }

    [Fact]
    public async Task FindUserByGuid_ShouldReturn_User()
    {
        // Arrange
        UserService service = new(_mockExceptionHandler.Object, _userValidator, _mockRepository.Object, _mapper);
        Guid guidTest = Guid.NewGuid();
        User userTest = new(Id: 1, IdAccount: 1, Email: "test@test.com", UserName: "User Name Test", Guid: guidTest);
        UserModel userModelTest = new() { Email = "test@test.com", UserName = "User Name Test", Guid = guidTest };
        _ = _mockRepository.Setup(x => x.GetUserByGuid(It.IsAny<Guid>())).ReturnsAsync(userTest);

        // Act
        UserModel? result = await service.GetUserByGuid(guidTest);

        // Assert
        Assert.Equivalent(result, userModelTest);
    }

    [Fact]
    public async Task ValidatePassword_ShouldReturn_True_WhenPasswordIsValid()
    {
        // Arrange
        UserService service = new(_mockExceptionHandler.Object, _userValidator, _mockRepository.Object, _mapper);
        Guid guidTest = Guid.NewGuid();
        User userTest = new(Id: 1, IdAccount: 1, Email: "test@test.com", UserName: "User Name Test", Guid: guidTest);
        userTest.SetPassword("test123");
        _ = _mockRepository.Setup(x => x.GetUserByGuid(guidTest)).ReturnsAsync(userTest);

        // Act
        bool isValid = await service.ValidatePassword("test123", guidTest);

        // Assert
        Assert.True(isValid);
    }

    [Fact]
    public async Task ValidatePassword_ShouldReturn_False_WhenPasswordIsInvalid()
    {
        // Arrange
        UserService service = new(_mockExceptionHandler.Object, _userValidator, _mockRepository.Object, _mapper);
        Guid guidTest = Guid.NewGuid();
        User userTest = new(Id: 1, IdAccount: 1, Email: "test@test.com", UserName: "User Name Test", Guid: guidTest);
        userTest.SetPassword("test123");
        _ = _mockRepository.Setup(x => x.GetUserByGuid(guidTest)).ReturnsAsync(userTest);

        // Act
        bool isValid = await service.ValidatePassword("wrongPassword", guidTest);

        // Assert
        Assert.False(isValid);
    }
}
