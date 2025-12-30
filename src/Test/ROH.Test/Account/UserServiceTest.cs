//-----------------------------------------------------------------------
// <copyright file="UserServiceTest.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using AutoMapper;

using Microsoft.Extensions.Logging.Abstractions;

using Moq;

using ROH.Context.Account.Entity;
using ROH.Context.Account.Interface;
using ROH.Service.Account;
using ROH.Service.Exception.Interface;
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
        MapperConfiguration config = new(
            cfg =>
            {
                _ = cfg.CreateMap<Context.Account.Entity.Account, AccountModel>()
                    .ForMember(
                        dest => dest.BirthDate,
                        opt => opt.MapFrom(src => src.BirthDate.ToDateTime(new TimeOnly(0, 0))));
                _ = cfg.CreateMap<AccountModel, Context.Account.Entity.Account>()
                    .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.BirthDate)));

                _ = cfg.CreateMap<User, UserModel>().ReverseMap();
            }, NullLoggerFactory.Instance);

        _mapper = new Mapper(config);
        _mockExceptionHandler = new Mock<IExceptionHandler>();
        _mockRepository = new Mock<IUserRepository>();
        _userValidator = new UserModelValidator();
    }

    [Fact]
    public async Task FindUserByEmailShouldReturnNullWhenUserNotFound()
    {
        // Arrange
        UserService service = new(_mockExceptionHandler.Object, _userValidator, _mockRepository.Object, _mapper);
        _ = _mockRepository.Setup(x => x.FindUserByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(() => null);

        // Act
        UserModel? result = await service.FindUserByEmailAsync("test", CancellationToken.None).ConfigureAwait(true);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task FindUserByEmailShouldReturnUserWhenUserFound()
    {
        // Arrange
        UserService service = new(_mockExceptionHandler.Object, _userValidator, _mockRepository.Object, _mapper);
        Guid guidTest = Guid.NewGuid();
        User userTest = new(Id: 1, IdAccount: 1, Email: "test@test.com", UserName: "User Name Test", Guid: guidTest);
        UserModel userModelTest = new() { Email = "test@test.com", UserName = "User Name Test", Guid = guidTest };
        _ = _mockRepository.Setup(x => x.FindUserByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(userTest);

        // Act
        UserModel? result = await service.FindUserByEmailAsync("test", CancellationToken.None).ConfigureAwait(true);

        // Assert
        Assert.Equivalent(result, userModelTest);
    }

    [Fact]
    public async Task FindUserByGuidShouldReturnUser()
    {
        // Arrange
        UserService service = new(_mockExceptionHandler.Object, _userValidator, _mockRepository.Object, _mapper);
        Guid guidTest = Guid.NewGuid();
        User userTest = new(Id: 1, IdAccount: 1, Email: "test@test.com", UserName: "User Name Test", Guid: guidTest);
        UserModel userModelTest = new() { Email = "test@test.com", UserName = "User Name Test", Guid = guidTest };
        _ = _mockRepository.Setup(x => x.GetUserByGuidAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(userTest);

        // Act
        UserModel? result = await service.GetUserByGuidAsync(guidTest, CancellationToken.None).ConfigureAwait(true);

        // Assert
        Assert.Equivalent(result, userModelTest);
    }

    [Fact]
    public async Task FindUserByUserNameShouldReturnNullWhenUserNotFound()
    {
        // Arrange
        UserService service = new(_mockExceptionHandler.Object, _userValidator, _mockRepository.Object, _mapper);
        _ = _mockRepository.Setup(x => x.FindUserByUserNameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(() => null);

        // Act
        UserModel? result = await service.FindUserByUserNameAsync("test", CancellationToken.None).ConfigureAwait(true);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task FindUserByUserNameShouldReturnUserWhenUserFound()
    {
        // Arrange
        UserService service = new(_mockExceptionHandler.Object, _userValidator, _mockRepository.Object, _mapper);
        Guid guidTest = Guid.NewGuid();
        User userTest = new(Id: 1, IdAccount: 1, Email: "test@test.com", UserName: "User Name Test", Guid: guidTest);
        UserModel userModelTest = new() { Email = "test@test.com", UserName = "User Name Test", Guid = guidTest };
        _ = _mockRepository.Setup(x => x.FindUserByUserNameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(userTest);

        // Act
        UserModel? result = await service.FindUserByUserNameAsync("test", CancellationToken.None).ConfigureAwait(true);

        // Assert
        Assert.Equivalent(result, userModelTest);
    }

    [Fact]
    public async Task NewUserShouldHandleException()
    {
        // Arrange
        UserService service = new(_mockExceptionHandler.Object, _userValidator, _mockRepository.Object, _mapper);
        UserModel userModel = new() { Email = "test.email@test.com", Password = "test123" };
        _ = _mockRepository.Setup(x => x.CreateNewUserAsync(It.IsAny<User>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception("Database error"));

        _mockExceptionHandler.Setup(x => x.HandleException(It.IsAny<Exception>()))
            .Returns(
                new DefaultResponse(httpStatus: HttpStatusCode.InternalServerError, message: "Internal Server Error"));

        // Act
        DefaultResponse result = await service.NewUserAsync(userModel, CancellationToken.None).ConfigureAwait(true);

        // Assert
        Assert.Equal(HttpStatusCode.InternalServerError, result.HttpStatus);
        _mockExceptionHandler.Verify(x => x.HandleException(It.IsAny<Exception>()), Times.Once);
    }

    [Fact]
    public async Task NewUserShouldReturnErrorWhenEmailAreAlreadyUsed()
    {
        // Arrange
        UserService service = new(_mockExceptionHandler.Object, _userValidator, _mockRepository.Object, _mapper);
        UserModel userModel = new() { Email = "test.email@test.com", Password = "test123" };
        _ = _mockRepository.Setup(x => x.EmailInUseAsync("test.email@test.com", CancellationToken.None)).ReturnsAsync(true);

        DefaultResponse expected = new(httpStatus: HttpStatusCode.Conflict, message: "The email are currently in use.");

        // Act
        DefaultResponse result = await service.NewUserAsync(userModel, CancellationToken.None).ConfigureAwait(true);

        // Assert
        Assert.Equivalent(expected, result);
    }

    [Fact]
    public async Task NewUserShouldReturnErrorWhenUserModelNotValid()
    {
        // Arrange
        UserService service = new(_mockExceptionHandler.Object, _userValidator, _mockRepository.Object, _mapper);
        UserModel userModel = new();

        // Act
        DefaultResponse result = await service.NewUserAsync(userModel, CancellationToken.None).ConfigureAwait(true);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, result.HttpStatus);
        Assert.False(string.IsNullOrWhiteSpace(result.Message));
    }

    [Fact]
    public async Task NewUserShouldReturnNewUserWhenNewUserIsValid()
    {
        // Arrange
        UserService service = new(_mockExceptionHandler.Object, _userValidator, _mockRepository.Object, _mapper);
        UserModel userModel = new() { Email = "test.email@test.com", Password = "test123" };
        _ = _mockRepository.Setup(x => x.CreateNewUserAsync(It.IsAny<User>(), It.IsAny<CancellationToken>())).ReturnsAsync(new User());

        DefaultResponse expected = new(httpStatus: HttpStatusCode.OK, message: "Account has been created!");

        // Act
        DefaultResponse result = await service.NewUserAsync(userModel, CancellationToken.None).ConfigureAwait(true);

        // Assert
        Assert.Equivalent(expected, result);
    }

    [Fact]
    public async Task ValidatePasswordShouldReturnFalseWhenPasswordIsInvalid()
    {
        // Arrange
        UserService service = new(_mockExceptionHandler.Object, _userValidator, _mockRepository.Object, _mapper);
        Guid guidTest = Guid.NewGuid();
        User userTest = new(Id: 1, IdAccount: 1, Email: "test@test.com", UserName: "User Name Test", Guid: guidTest);
        userTest.SetPassword("test123");
        _ = _mockRepository.Setup(x => x.GetUserByGuidAsync(guidTest, CancellationToken.None)).ReturnsAsync(userTest);

        // Act
        bool isValid = await service.ValidatePasswordAsync("wrongPassword", guidTest, CancellationToken.None).ConfigureAwait(true);

        // Assert
        Assert.False(isValid);
    }

    [Fact]
    public async Task ValidatePasswordShouldReturnTrueWhenPasswordIsValid()
    {
        // Arrange
        UserService service = new(_mockExceptionHandler.Object, _userValidator, _mockRepository.Object, _mapper);
        Guid guidTest = Guid.NewGuid();
        User userTest = new(Id: 1, IdAccount: 1, Email: "test@test.com", UserName: "User Name Test", Guid: guidTest);
        userTest.SetPassword("test123");
        _ = _mockRepository.Setup(x => x.GetUserByGuidAsync(guidTest, CancellationToken.None)).ReturnsAsync(userTest);

        // Act
        bool isValid = await service.ValidatePasswordAsync("test123", guidTest, CancellationToken.None).ConfigureAwait(true);

        // Assert
        Assert.True(isValid);
    }
}
