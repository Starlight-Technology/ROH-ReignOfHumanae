using AutoMapper;

using FluentValidation;

using Microsoft.AspNetCore.Identity;

using Moq;

using ROH.Domain.Accounts;
using ROH.Interfaces.Repository.Account;
using ROH.Interfaces.Services.ExceptionService;
using ROH.Services.Account;
using ROH.StandardModels.Account;
using ROH.StandardModels.Response;
using ROH.Validations.Account;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ROH.Test.Account;
public class UserServiceTest
{
    [Fact]
    public async Task NewUser_ShouldReturn_Error_WhenUserModelNotValid()
    {
        // Arrange
        MapperConfiguration config = new(cfg =>
        {
            // Configure your mappings here
            _ = cfg.CreateMap<User, UserModel>().ReverseMap();
        });

        Mapper mapper = new(config);

        UserModel userModel = new();

        var userValidator = new UserModelValidation();

        Mock<IExceptionHandler> mockExceptionHandler = new();

        Mock<IUserRepository> mockRepository = new();

        var service = new UserService(mockExceptionHandler.Object, userValidator, mapper, mockRepository.Object);

        // Act
        var result = await service.NewUser(userModel);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, result.HttpStatus);
        Assert.False(string.IsNullOrWhiteSpace(result.Message));

    }

    [Fact]
    public async Task NewUser_ShouldReturn_Error_WhenEmailAreAlreadyUsed()
    {
        // Arrange
        MapperConfiguration config = new(cfg =>
        {
            // Configure your mappings here
            _ = cfg.CreateMap<User, UserModel>().ReverseMap();
        });

        Mapper mapper = new(config);

        UserModel userModel = new() { Email = "test.email@test.com", Password = "test123" };

        var userValidator = new UserModelValidation();

        Mock<IExceptionHandler> mockExceptionHandler = new();

        Mock<IUserRepository> mockRepository = new();
        mockRepository.Setup(x => x.EmailInUse("test.email@test.com")).ReturnsAsync(true);

        var service = new UserService(mockExceptionHandler.Object, userValidator, mapper, mockRepository.Object);

        var expected = new DefaultResponse(httpStatus: HttpStatusCode.Conflict, message: "The email are currently in use.");

        // Act
        var result = await service.NewUser(userModel);

        // Assert

        Assert.Equivalent(expected, result);
    }    
    
    [Fact]
    public async Task NewUser_ShouldReturn_NewUser_WhenNewUserIsValid()
    {
        // Arrange
        MapperConfiguration config = new(cfg =>
        {
            // Configure your mappings here
            _ = cfg.CreateMap<User, UserModel>().ReverseMap();
        });

        Mapper mapper = new(config);

        UserModel userModel = new() { Email = "test.email@test.com", Password = "test123" };

        var userValidator = new UserModelValidation();

        Mock<IExceptionHandler> mockExceptionHandler = new();

        Mock<IUserRepository> mockRepository = new();
        mockRepository.Setup(x => x.EmailInUse("test.email@test.com")).ReturnsAsync(true);

        var service = new UserService(mockExceptionHandler.Object, userValidator, mapper, mockRepository.Object);

        var expected = new DefaultResponse(httpStatus: HttpStatusCode.Conflict, message: "The email are currently in use.");

        // Act
        var result = await service.NewUser(userModel);

        // Assert

        Assert.Equivalent(expected, result);
    }
}
