﻿//-----------------------------------------------------------------------
// <copyright file="LoginServiceTest.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using FluentValidation;

using Moq;

using ROH.Context.Account.Entity;
using ROH.Service.Account;
using ROH.Service.Account.Interface;
using ROH.Service.Authentication.Interface;
using ROH.Service.Exception.Interface;
using ROH.StandardModels.Account;
using ROH.StandardModels.Response;
using ROH.Validations.Account;

using System.Net;

namespace ROH.Test.Account;

public class LoginServiceTest
{
    [Fact]
    public async Task LoginShouldHandleException()
    {
        // Arrange
        Mock<IExceptionHandler> mockExceptionHandler = new();
        mockExceptionHandler.Setup(x => x.HandleException(It.IsAny<Exception>()))
            .Returns(
                new DefaultResponse(httpStatus: HttpStatusCode.InternalServerError, message: "Internal Server Error"));

        Mock<IValidator<LoginModel>> mockLoginValidator = new();
        mockLoginValidator.Setup(x => x.ValidateAsync(It.IsAny<LoginModel>(), default))
            .ThrowsAsync(new Exception("Validation error"));

        Mock<IUserService> mockUserService = new();

        Mock<IAuthService> mockAuthService = new();

        LoginService loginService = new(
            mockExceptionHandler.Object,
            mockLoginValidator.Object,
            mockUserService.Object,
            mockAuthService.Object);

        LoginModel loginModel = new() { Login = "test", Password = "test" };

        DefaultResponse expected = new(
            httpStatus: HttpStatusCode.InternalServerError,
            message: "Internal Server Error");

        // Act
        DefaultResponse result = await loginService.LoginAsync(loginModel, cancellationToken: CancellationToken.None).ConfigureAwait(true);

        // Assert
        Assert.Equivalent(expected, result);
        mockExceptionHandler.Verify(x => x.HandleException(It.IsAny<Exception>()), Times.Once);
    }

    [Fact]
    public async Task LoginWithEmptyCredentialsShouldReturnError()
    {
        // Arrange
        LoginModelValidator validator = new();

        Mock<IExceptionHandler> mockExceptionHandler = new();
        Mock<IUserService> mockUserService = new();
        Mock<IAuthService> mockAuthService = new();
        _ = mockAuthService.Setup(x => x.GenerateJwtToken(It.IsAny<UserModel>())).Returns(string.Empty);

        LoginService service = new(
            mockExceptionHandler.Object,
            validator,
            mockUserService.Object,
            mockAuthService.Object);

        // Act
        DefaultResponse result = await service.LoginAsync(new LoginModel(), CancellationToken.None).ConfigureAwait(true);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, result.HttpStatus);
        Assert.False(string.IsNullOrWhiteSpace(result.Message));
    }

    [Fact]
    public async Task LoginWithInvalidPasswordShouldReturnError()
    {
        // Arrange
        Guid guidTest = Guid.NewGuid();
        User user = new(1, 1, guidTest, "test", "test");
        user.SetPassword("test");
        UserModel userModelTest = new() { Email = "test@test.com", UserName = "User Name Test", Guid = guidTest };

        LoginModel loginModel = new() { Login = "test", Password = "testwrong" };

        LoginModelValidator validator = new();

        Mock<IExceptionHandler> mockExceptionHandler = new();

        Mock<IUserService> mockUserService = new();
        _ = mockUserService.Setup(x => x.FindUserByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(userModelTest);
        _ = mockUserService.Setup(x => x.FindUserByUserNameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(userModelTest);

        Mock<IAuthService> mockAuthService = new();
        _ = mockAuthService.Setup(x => x.GenerateJwtToken(It.IsAny<UserModel>())).Returns(string.Empty);

        LoginService service = new(
            mockExceptionHandler.Object,
            validator,
            mockUserService.Object,
            mockAuthService.Object);

        DefaultResponse expected = new(httpStatus: HttpStatusCode.Unauthorized, message: "Invalid password!");

        // Act
        DefaultResponse result = await service.LoginAsync(loginModel, CancellationToken.None).ConfigureAwait(true);

        // Assert
        Assert.Equivalent(expected, result);
    }

    [Fact]
    public async Task LoginWithNotFoundCredentialsShouldReturnNotFound()
    {
        // Arrange
        LoginModelValidator validator = new();

        Mock<IExceptionHandler> mockExceptionHandler = new();

        Mock<IUserService> mockUserService = new();
        _ = mockUserService.Setup(x => x.FindUserByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(() => null);
        _ = mockUserService.Setup(x => x.FindUserByUserNameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(() => null);

        Mock<IAuthService> mockAuthService = new();
        _ = mockAuthService.Setup(x => x.GenerateJwtToken(It.IsAny<UserModel>())).Returns(string.Empty);

        LoginService service = new(
            mockExceptionHandler.Object,
            validator,
            mockUserService.Object,
            mockAuthService.Object);

        LoginModel loginModel = new() { Login = "test", Password = "test" };

        // Act
        DefaultResponse result = await service.LoginAsync(loginModel, CancellationToken.None).ConfigureAwait(true);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, result.HttpStatus);
        Assert.False(string.IsNullOrWhiteSpace(result.Message));
    }

    [Fact]
    public async Task LoginWithValidCredentialsShouldReturnUserModel()
    {
        // Arrange
        string pass = "test";
        Guid guidTest = Guid.NewGuid();
        User user = new(1, 1, guidTest, "test@test.com", "User Name Test");
        user.SetPassword(pass);
        UserModel userModelTest = new() { Email = "test@test.com", UserName = "User Name Test", Guid = guidTest };

        LoginModel loginModel = new() { Login = "test", Password = pass };

        LoginModelValidator validator = new();

        Mock<IExceptionHandler> mockExceptionHandler = new();

        Mock<IUserService> mockUserService = new();
        _ = mockUserService.Setup(x => x.FindUserByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(userModelTest);
        _ = mockUserService.Setup(x => x.FindUserByUserNameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(userModelTest);
        _ = mockUserService.Setup(x => x.ValidatePasswordAsync(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);

        Mock<IAuthService> mockAuthService = new();
        _ = mockAuthService.Setup(x => x.GenerateJwtToken(It.IsAny<UserModel>())).Returns(string.Empty);

        LoginService service = new(
            mockExceptionHandler.Object,
            validator,
            mockUserService.Object,
            mockAuthService.Object);

        DefaultResponse expected = new(
            objectResponse: new UserModel
            {
                Email = user.Email,
                UserName = user.UserName,
                Guid = user.Guid,
                Token = string.Empty
            });

        // Act
        DefaultResponse result = await service.LoginAsync(loginModel, CancellationToken.None).ConfigureAwait(true);

        // Assert
        Assert.Equivalent(expected, result);
    }
}
