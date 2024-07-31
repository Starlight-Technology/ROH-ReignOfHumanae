using AutoMapper;

using ROH.Interfaces.Repository.Account;
using ROH.Interfaces.Services.Account;
using ROH.Interfaces.Services.ExceptionService;
using ROH.StandardModels.Account;
using ROH.StandardModels.Response;

namespace ROH.Services.Account;

public class AccountService(IExceptionHandler handler, IAccountRepository repository, IMapper mapper) : IAccountService
{
    public async Task<DefaultResponse> GetAccountByUserGuid(Guid userGuid)
    {
        try
        {
            Domain.Accounts.Account? account = await repository.GetAccountByUserGuid(userGuid);

            if (account is null)
                return new DefaultResponse(httpStatus: System.Net.HttpStatusCode.NotFound);

            AccountModel model = mapper.Map<AccountModel>(account);

            return new DefaultResponse(objectResponse: model);
        }
        catch (Exception ex)
        {
            return handler.HandleException(ex);
        }
    }

    public async Task<DefaultResponse> UpdateAccount(AccountModel accountModel)
    {
        try
        {
            Domain.Accounts.Account? account = await repository.GetAccountByGuid(accountModel.Guid);
            if (account is null)
                return new DefaultResponse(httpStatus: System.Net.HttpStatusCode.NotFound, message: "Account not found.");

            account = account with { BirthDate = DateOnly.FromDateTime(accountModel.BirthDate), RealName = accountModel.RealName, };

            await repository.UpdateAccount(account);

            return new DefaultResponse();
        }
        catch (Exception ex)
        {
            return handler.HandleException(ex);
        }
    }
}
