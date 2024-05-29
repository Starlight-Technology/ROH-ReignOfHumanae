using AutoMapper;

using Moq;

using ROH.Domain.Accounts;
using ROH.Interfaces.Repository.Account;
using ROH.Interfaces.Services.Account;
using ROH.Interfaces.Services.ExceptionService;
using ROH.Services.Account;
using ROH.StandardModels.Account;
using ROH.StandardModels.Response;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ROH.Test.Account;
public class AccountServiceTest
{
    [Fact]
    public async Task GetAccountByUserGuid_ShouldReturn_Error_WhenUserNotFound()
    {
        // Arrange 
        MapperConfiguration config = new(cfg =>
        {
            // Configure your mappings here
            cfg.CreateMap<Domain.Accounts.Account, AccountModel>()
    .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.BirthDate.ToDateTime(new TimeOnly(0, 0))));
            cfg.CreateMap<AccountModel, Domain.Accounts.Account>()
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.BirthDate)));
        });

        Mapper mapper = new(config);

        Mock<IExceptionHandler> mockExceptionHandler = new();
        _ = mockExceptionHandler.Setup(x => x.HandleException(It.IsAny<Exception>())).Returns(new DefaultResponse(httpStatus: HttpStatusCode.BadRequest));

        Mock<IAccountRepository> mockRepository = new();
        _ = mockRepository.Setup(x => x.GetAccountById(It.IsAny<long>())).ReturnsAsync(() => null);

        Mock<IUserService> mockUserService = new();
        _ = mockUserService.Setup(x => x.GetUserByGuid(It.IsAny<Guid>())).ThrowsAsync(new Exception());

        AccountService service = new(mockExceptionHandler.Object, mockUserService.Object, mockRepository.Object, mapper);

        DefaultResponse expected = new(httpStatus: HttpStatusCode.BadRequest);

        // Act
        DefaultResponse result = await service.GetAccounByUserGuid(Guid.NewGuid());

        // Assert

        Assert.Equivalent(expected, result);
    }

    [Fact]
    public async Task GetAccountByUserGuid_ShouldReturn_User_WhenUserFound()
    {
        // Arrange 
        MapperConfiguration config = new(cfg =>
        {
            // Configure your mappings here
            cfg.CreateMap<Domain.Accounts.Account, AccountModel>()
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.BirthDate.ToDateTime(new TimeOnly(0, 0))));
            cfg.CreateMap<AccountModel, Domain.Accounts.Account>()
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.BirthDate)));
        });

        Mapper mapper = new(config);

        Guid guidTest = Guid.NewGuid();
        string userNameTest = "Test";
        string realNameTest = "Test";
        DateTime birthDateTest = DateTime.Today;

        User mockedUser = new(UserName: userNameTest);

        AccountModel accountModel = new() { Guid = guidTest, UserName = userNameTest, RealName = realNameTest, BirthDate = birthDateTest };
        Domain.Accounts.Account account = new(Guid: guidTest, RealName: realNameTest, BirthDate: DateOnly.FromDateTime(birthDateTest));

        Mock<IExceptionHandler> mockExceptionHandler = new();

        Mock<IAccountRepository> mockRepository = new();
        _ = mockRepository.Setup(x => x.GetAccountById(It.IsAny<long>())).ReturnsAsync(account);

        Mock<IUserService> mockUserService = new();
        _ = mockUserService.Setup(x => x.GetUserByGuid(It.IsAny<Guid>())).ReturnsAsync(mockedUser);

        AccountService service = new(mockExceptionHandler.Object, mockUserService.Object, mockRepository.Object, mapper);

        DefaultResponse expected = new(httpStatus: HttpStatusCode.OK, objectResponse: accountModel);

        // Act
        DefaultResponse result = await service.GetAccounByUserGuid(Guid.NewGuid());

        // Assert

        Assert.Equivalent(expected, result);
    }

    [Fact]
    public async Task UpdateAccount_ShouldReturn_NotFound_WhenAccountNotFound()
    {
        // Arrange 
        MapperConfiguration config = new(cfg =>
        {
            // Configure your mappings here
            cfg.CreateMap<Domain.Accounts.Account, AccountModel>()
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.BirthDate.ToDateTime(new TimeOnly(0, 0))));
            cfg.CreateMap<AccountModel, Domain.Accounts.Account>()
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.BirthDate)));
        });

        Mapper mapper = new(config);

        Guid guidTest = Guid.NewGuid();
        string userNameTest = "Test";
        string realNameTest = "Test";
        DateTime birthDateTest = DateTime.Today;

        AccountModel accountModel = new() { Guid = guidTest, UserName = userNameTest, RealName = realNameTest, BirthDate = birthDateTest };

        Mock<IExceptionHandler> mockExceptionHandler = new();

        Mock<IAccountRepository> mockRepository = new();
        _ = mockRepository.Setup(x => x.GetAccountByGuid(It.IsAny<Guid>())).ReturnsAsync(() => null);

        Mock<IUserService> mockUserService = new();

        AccountService service = new(mockExceptionHandler.Object, mockUserService.Object, mockRepository.Object, mapper);

        DefaultResponse expected = new(httpStatus: HttpStatusCode.NotFound, message: "Account not found.");

        // Act
        DefaultResponse result = await service.UpdateAccount(accountModel);

        // Assert

        Assert.Equivalent(expected, result);
    }

    [Fact]
    public async Task UpdateAccount_ShouldReturn_Success_WhenAccountFound()
    {
        // Arrange 
        MapperConfiguration config = new(cfg =>
        {
            // Configure your mappings here
            cfg.CreateMap<Domain.Accounts.Account, AccountModel>()
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.BirthDate.ToDateTime(new TimeOnly(0, 0))));
            cfg.CreateMap<AccountModel, Domain.Accounts.Account>()
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.BirthDate)));
        });

        Mapper mapper = new(config);

        Guid guidTest = Guid.NewGuid();
        string userNameTest = "Test";
        string realNameTest = "Test";
        DateTime birthDateTest = DateTime.Today;

        User mockedUser = new(UserName: userNameTest);

        AccountModel accountModel = new() { Guid = guidTest, UserName = userNameTest, RealName = realNameTest, BirthDate = birthDateTest };

        Domain.Accounts.Account account = new(Guid: guidTest, RealName: realNameTest, BirthDate: DateOnly.FromDateTime(birthDateTest));

        Mock<IExceptionHandler> mockExceptionHandler = new();

        Mock<IAccountRepository> mockRepository = new();
        _ = mockRepository.Setup(x => x.GetAccountByGuid(It.IsAny<Guid>())).ReturnsAsync(account);

        Mock<IUserService> mockUserService = new();
        _ = mockUserService.Setup(x => x.GetUserByGuid(It.IsAny<Guid>())).ReturnsAsync(mockedUser);

        AccountService service = new(mockExceptionHandler.Object, mockUserService.Object, mockRepository.Object, mapper);

        DefaultResponse expected = new();
         
        // Act
        DefaultResponse result = await service.UpdateAccount(accountModel);

        // Assert

        Assert.Equivalent(expected, result);
    }
}
