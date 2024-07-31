using ROH.StandardModels.Account;
using ROH.StandardModels.Response;

namespace ROH.Interfaces.Services.Account;

public interface IAccountService
{
    Task<DefaultResponse> GetAccountByUserGuid(Guid userGuid);

    Task<DefaultResponse> UpdateAccount(AccountModel accountModel);
}