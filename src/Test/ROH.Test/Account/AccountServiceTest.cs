//-----------------------------------------------------------------------
// <copyright file="AccountServiceTest.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using AutoMapper;

using Moq;

using ROH.Context.Account.Entity;

using ROH.Context.Account.Interface;
using ROH.Service.Account;
using ROH.Service.Exception.Interface;
using ROH.StandardModels.Account;
using ROH.StandardModels.Response;

using System.Net;

namespace ROH.Test.Account;

public class AccountServiceTest
{
    [Fact]
    public async Task GetAccountByUserGuidShouldHandleException()
    {
        // Arrange
        MapperConfiguration config = new(
            cfg =>
            {
                cfg.CreateMap<Context.Account.Entity.Account, AccountModel>()
                    .ForMember(
                        dest => dest.BirthDate,
                        opt => opt.MapFrom(src => src.BirthDate.ToDateTime(new TimeOnly(0, 0))));
                cfg.CreateMap<AccountModel, Context.Account.Entity.Account>()
                    .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.BirthDate)));

                cfg.CreateMap<User, UserModel>().ReverseMap();
            });

        Mapper mapper = new(config);

        Mock<IExceptionHandler> mockExceptionHandler = new();
        mockExceptionHandler.Setup(x => x.HandleException(It.IsAny<Exception>()))
            .Returns(
                new DefaultResponse(httpStatus: HttpStatusCode.InternalServerError, message: "Internal Server Error"));

        Mock<IAccountRepository> mockRepository = new();
        mockRepository.Setup(x => x.GetAccountByUserGuidAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception("Database error"));

        AccountService service = new(mockExceptionHandler.Object, mockRepository.Object, mapper);

        DefaultResponse expected = new(httpStatus: HttpStatusCode.InternalServerError, message: "Internal Server Error");

        // Act
        DefaultResponse result = await service.GetAccountByUserGuidAsync(Guid.NewGuid(), CancellationToken.None).ConfigureAwait(true);

        // Assert
        Assert.Equivalent(expected, result);
    }

    [Fact]
    public async Task GetAccountByUserGuidShouldReturnAccountWhenUserFound()
    {
        // Arrange
        MapperConfiguration config = new(
            cfg =>
            {
                // Configure your mappings here
                _ = cfg.CreateMap<Context.Account.Entity.Account, AccountModel>()
                    .ForMember(
                        dest => dest.BirthDate,
                        opt => opt.MapFrom(src => src.BirthDate.ToDateTime(new TimeOnly(0, 0))));
                _ = cfg.CreateMap<AccountModel, Context.Account.Entity.Account>()
                    .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.BirthDate)));

                _ = cfg.CreateMap<User, UserModel>().ReverseMap();
            });

        Mapper mapper = new(config);

        Guid guidTest = Guid.NewGuid();
        string realNameTest = "Test";
        DateTime birthDateTest = DateTime.Today;

        AccountModel accountModel = new() { Guid = guidTest, RealName = realNameTest, BirthDate = birthDateTest };
        Context.Account.Entity.Account account = new(
            Guid: guidTest,
            RealName: realNameTest,
            BirthDate: DateOnly.FromDateTime(birthDateTest));

        Mock<IExceptionHandler> mockExceptionHandler = new();

        Mock<IAccountRepository> mockRepository = new();
        _ = mockRepository.Setup(x => x.GetAccountByUserGuidAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(account);

        AccountService service = new(mockExceptionHandler.Object, mockRepository.Object, mapper);

        DefaultResponse expected = new(httpStatus: HttpStatusCode.OK, objectResponse: accountModel);

        // Act
        DefaultResponse result = await service.GetAccountByUserGuidAsync(Guid.NewGuid(), CancellationToken.None).ConfigureAwait(true);

        // Assert

        Assert.Equivalent(expected, result);
    }

    [Fact]
    public async Task GetAccountByUserGuidShouldReturnErrorWhenUserNotFound()
    {
        // Arrange
        MapperConfiguration config = new(
            cfg =>
            {
                // Configure your mappings here
                _ = cfg.CreateMap<Context.Account.Entity.Account, AccountModel>()
                    .ForMember(
                        dest => dest.BirthDate,
                        opt => opt.MapFrom(src => src.BirthDate.ToDateTime(new TimeOnly(0, 0))));
                _ = cfg.CreateMap<AccountModel, Context.Account.Entity.Account>()
                    .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.BirthDate)));

                _ = cfg.CreateMap<User, UserModel>().ReverseMap();
            });

        Mapper mapper = new(config);

        Mock<IExceptionHandler> mockExceptionHandler = new();

        Mock<IAccountRepository> mockRepository = new();
        _ = mockRepository.Setup(x => x.GetAccountByGuidAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(() => null);

        AccountService service = new(mockExceptionHandler.Object, mockRepository.Object, mapper);

        DefaultResponse expected = new(httpStatus: HttpStatusCode.NotFound);

        // Act
        DefaultResponse result = await service.GetAccountByUserGuidAsync(Guid.NewGuid(), CancellationToken.None).ConfigureAwait(true);

        // Assert

        Assert.Equivalent(expected, result);
    }

    [Fact]
    public async Task UpdateAccountShouldHandleException()
    {
        // Arrange
        MapperConfiguration config = new(
            cfg =>
            {
                cfg.CreateMap<Context.Account.Entity.Account, AccountModel>()
                    .ForMember(
                        dest => dest.BirthDate,
                        opt => opt.MapFrom(src => src.BirthDate.ToDateTime(new TimeOnly(0, 0))));
                cfg.CreateMap<AccountModel, Context.Account.Entity.Account>()
                    .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.BirthDate)));

                cfg.CreateMap<User, UserModel>().ReverseMap();
            });

        Mapper mapper = new(config);

        Guid guidTest = Guid.NewGuid();
        string realNameTest = "Test";
        DateTime birthDateTest = DateTime.Today;

        AccountModel accountModel = new() { Guid = guidTest, RealName = realNameTest, BirthDate = birthDateTest };

        Mock<IExceptionHandler> mockExceptionHandler = new();
        mockExceptionHandler.Setup(x => x.HandleException(It.IsAny<Exception>()))
            .Returns(
                new DefaultResponse(httpStatus: HttpStatusCode.InternalServerError, message: "Internal Server Error"));

        Mock<IAccountRepository> mockRepository = new();
        mockRepository.Setup(x => x.GetAccountByGuidAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception("Database error"));

        AccountService service = new(mockExceptionHandler.Object, mockRepository.Object, mapper);

        DefaultResponse expected = new(httpStatus: HttpStatusCode.InternalServerError, message: "Internal Server Error");

        // Act
        DefaultResponse result = await service.UpdateAccountAsync(accountModel, CancellationToken.None).ConfigureAwait(true);

        // Assert
        Assert.Equivalent(expected, result);
    }

    [Fact]
    public async Task UpdateAccountShouldReturnNotFoundWhenAccountNotFound()
    {
        // Arrange
        MapperConfiguration config = new(
            cfg =>
            {
                // Configure your mappings here
                _ = cfg.CreateMap<Context.Account.Entity.Account, AccountModel>()
                    .ForMember(
                        dest => dest.BirthDate,
                        opt => opt.MapFrom(src => src.BirthDate.ToDateTime(new TimeOnly(0, 0))));
                _ = cfg.CreateMap<AccountModel, Context.Account.Entity.Account>()
                    .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.BirthDate)));

                _ = cfg.CreateMap<User, UserModel>().ReverseMap();
            });

        Mapper mapper = new(config);

        Guid guidTest = Guid.NewGuid();
        string realNameTest = "Test";
        DateTime birthDateTest = DateTime.Today;

        AccountModel accountModel = new() { Guid = guidTest, RealName = realNameTest, BirthDate = birthDateTest };

        Mock<IExceptionHandler> mockExceptionHandler = new();

        Mock<IAccountRepository> mockRepository = new();
        _ = mockRepository.Setup(x => x.GetAccountByGuidAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(() => null);

        AccountService service = new(mockExceptionHandler.Object, mockRepository.Object, mapper);

        DefaultResponse expected = new(httpStatus: HttpStatusCode.NotFound, message: "Account not found.");

        // Act
        DefaultResponse result = await service.UpdateAccountAsync(accountModel, CancellationToken.None).ConfigureAwait(true);

        // Assert

        Assert.Equivalent(expected, result);
    }

    [Fact]
    public async Task UpdateAccountShouldReturnSuccessWhenAccountFound()
    {
        // Arrange
        MapperConfiguration config = new(
            cfg =>
            {
                // Configure your mappings here
                _ = cfg.CreateMap<Context.Account.Entity.Account, AccountModel>()
                    .ForMember(
                        dest => dest.BirthDate,
                        opt => opt.MapFrom(src => src.BirthDate.ToDateTime(new TimeOnly(0, 0))));
                _ = cfg.CreateMap<AccountModel, Context.Account.Entity.Account>()
                    .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.BirthDate)));

                _ = cfg.CreateMap<User, UserModel>().ReverseMap();
            });

        Mapper mapper = new(config);

        Guid guidTest = Guid.NewGuid();
        string realNameTest = "Test";
        DateTime birthDateTest = DateTime.Today;

        AccountModel accountModel = new() { Guid = guidTest, RealName = realNameTest, BirthDate = birthDateTest };

        Context.Account.Entity.Account account = new(
            Guid: guidTest,
            RealName: realNameTest,
            BirthDate: DateOnly.FromDateTime(birthDateTest));

        Mock<IExceptionHandler> mockExceptionHandler = new();

        Mock<IAccountRepository> mockRepository = new();
        _ = mockRepository.Setup(x => x.GetAccountByGuidAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(account);

        AccountService service = new(mockExceptionHandler.Object, mockRepository.Object, mapper);

        DefaultResponse expected = new(message: "Account has been updated.");

        // Act
        DefaultResponse result = await service.UpdateAccountAsync(accountModel, CancellationToken.None).ConfigureAwait(true);

        // Assert

        Assert.Equivalent(expected, result);
    }
}
