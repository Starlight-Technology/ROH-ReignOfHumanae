using AutoMapper;

using Moq;

using ROH.Domain.Accounts;
using ROH.Interfaces.Repository.Account;
using ROH.Interfaces.Services.ExceptionService;
using ROH.Services.Account;
using ROH.StandardModels.Account;
using ROH.StandardModels.Response;

using System.Net;

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
            _ = cfg.CreateMap<Domain.Accounts.Account, AccountModel>()
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.BirthDate.ToDateTime(new TimeOnly(0, 0))));
            _ = cfg.CreateMap<AccountModel, Domain.Accounts.Account>()
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.BirthDate)));

            _ = cfg.CreateMap<User, UserModel>().ReverseMap();
        });

        Mapper mapper = new(config);

        Mock<IExceptionHandler> mockExceptionHandler = new();
        _ = mockExceptionHandler.Setup(x => x.HandleException(It.IsAny<Exception>())).Returns(new DefaultResponse(httpStatus: HttpStatusCode.BadRequest));

        Mock<IAccountRepository> mockRepository = new();
        _ = mockRepository.Setup(x => x.GetAccountById(It.IsAny<long>())).ReturnsAsync(() => null);

        AccountService service = new(mockExceptionHandler.Object, mockRepository.Object, mapper);

        DefaultResponse expected = new(httpStatus: HttpStatusCode.NotFound);

        // Act
        DefaultResponse result = await service.GetAccountByUserGuid(Guid.NewGuid());

        // Assert

        Assert.Equivalent(expected, result);
    }

    [Fact]
    public async Task GetAccountByUserGuid_ShouldReturn_Account_WhenUserFound()
    {
        // Arrange
        MapperConfiguration config = new(cfg =>
        {
            // Configure your mappings here
            _ = cfg.CreateMap<Domain.Accounts.Account, AccountModel>()
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.BirthDate.ToDateTime(new TimeOnly(0, 0))));
            _ = cfg.CreateMap<AccountModel, Domain.Accounts.Account>()
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.BirthDate)));

            _ = cfg.CreateMap<User, UserModel>().ReverseMap();
        });

        Mapper mapper = new(config);

        Guid guidTest = Guid.NewGuid();
        string realNameTest = "Test";
        DateTime birthDateTest = DateTime.Today;

        AccountModel accountModel = new() { Guid = guidTest, RealName = realNameTest, BirthDate = birthDateTest };
        Domain.Accounts.Account account = new(Guid: guidTest, RealName: realNameTest, BirthDate: DateOnly.FromDateTime(birthDateTest));

        Mock<IExceptionHandler> mockExceptionHandler = new();

        Mock<IAccountRepository> mockRepository = new();
        _ = mockRepository.Setup(x => x.GetAccountByUserGuid(It.IsAny<Guid>())).ReturnsAsync(account);

        AccountService service = new(mockExceptionHandler.Object, mockRepository.Object, mapper);

        DefaultResponse expected = new(httpStatus: HttpStatusCode.OK, objectResponse: accountModel);

        // Act
        DefaultResponse result = await service.GetAccountByUserGuid(Guid.NewGuid());

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
            _ = cfg.CreateMap<Domain.Accounts.Account, AccountModel>()
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.BirthDate.ToDateTime(new TimeOnly(0, 0))));
            _ = cfg.CreateMap<AccountModel, Domain.Accounts.Account>()
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
        _ = mockRepository.Setup(x => x.GetAccountByGuid(It.IsAny<Guid>())).ReturnsAsync(() => null);

        AccountService service = new(mockExceptionHandler.Object, mockRepository.Object, mapper);

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
            _ = cfg.CreateMap<Domain.Accounts.Account, AccountModel>()
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.BirthDate.ToDateTime(new TimeOnly(0, 0))));
            _ = cfg.CreateMap<AccountModel, Domain.Accounts.Account>()
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.BirthDate)));

            _ = cfg.CreateMap<User, UserModel>().ReverseMap();
        });

        Mapper mapper = new(config);

        Guid guidTest = Guid.NewGuid();
        string realNameTest = "Test";
        DateTime birthDateTest = DateTime.Today;

        AccountModel accountModel = new() { Guid = guidTest, RealName = realNameTest, BirthDate = birthDateTest };

        Domain.Accounts.Account account = new(Guid: guidTest, RealName: realNameTest, BirthDate: DateOnly.FromDateTime(birthDateTest));

        Mock<IExceptionHandler> mockExceptionHandler = new();

        Mock<IAccountRepository> mockRepository = new();
        _ = mockRepository.Setup(x => x.GetAccountByGuid(It.IsAny<Guid>())).ReturnsAsync(account);

        AccountService service = new(mockExceptionHandler.Object, mockRepository.Object, mapper);

        DefaultResponse expected = new();

        // Act
        DefaultResponse result = await service.UpdateAccount(accountModel);

        // Assert

        Assert.Equivalent(expected, result);
    }

    [Fact]
    public async Task GetAccountByUserGuid_ShouldHandle_Exception()
    {
        // Arrange
        MapperConfiguration config = new(cfg =>
        {
            cfg.CreateMap<Domain.Accounts.Account, AccountModel>()
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.BirthDate.ToDateTime(new TimeOnly(0, 0))));
            cfg.CreateMap<AccountModel, Domain.Accounts.Account>()
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.BirthDate)));

            cfg.CreateMap<User, UserModel>().ReverseMap();
        });

        Mapper mapper = new(config);

        Mock<IExceptionHandler> mockExceptionHandler = new();
        mockExceptionHandler.Setup(x => x.HandleException(It.IsAny<Exception>()))
            .Returns(new DefaultResponse(httpStatus: HttpStatusCode.InternalServerError, message: "Internal Server Error"));

        Mock<IAccountRepository> mockRepository = new();
        mockRepository.Setup(x => x.GetAccountByUserGuid(It.IsAny<Guid>()))
            .ThrowsAsync(new Exception("Database error"));

        AccountService service = new(mockExceptionHandler.Object, mockRepository.Object, mapper);

        DefaultResponse expected = new(httpStatus: HttpStatusCode.InternalServerError, message: "Internal Server Error");

        // Act
        DefaultResponse result = await service.GetAccountByUserGuid(Guid.NewGuid());

        // Assert
        Assert.Equivalent(expected, result);
    }

    [Fact]
    public async Task UpdateAccount_ShouldHandle_Exception()
    {
        // Arrange
        MapperConfiguration config = new(cfg =>
        {
            cfg.CreateMap<Domain.Accounts.Account, AccountModel>()
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.BirthDate.ToDateTime(new TimeOnly(0, 0))));
            cfg.CreateMap<AccountModel, Domain.Accounts.Account>()
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
            .Returns(new DefaultResponse(httpStatus: HttpStatusCode.InternalServerError, message: "Internal Server Error"));

        Mock<IAccountRepository> mockRepository = new();
        mockRepository.Setup(x => x.GetAccountByGuid(It.IsAny<Guid>()))
            .ThrowsAsync(new Exception("Database error"));

        AccountService service = new(mockExceptionHandler.Object, mockRepository.Object, mapper);

        DefaultResponse expected = new(httpStatus: HttpStatusCode.InternalServerError, message: "Internal Server Error");

        // Act
        DefaultResponse result = await service.UpdateAccount(accountModel);

        // Assert
        Assert.Equivalent(expected, result);
    }
}
