//-----------------------------------------------------------------------
// <copyright file="AccountService.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using AutoMapper;

using ROH.Interfaces.Repository.Account;
using ROH.Interfaces.Services.Account;
using ROH.Interfaces.Services.ExceptionService;
using ROH.StandardModels.Account;
using ROH.StandardModels.Response;

using System.Net;

namespace ROH.Services.Account;

public class AccountService(IExceptionHandler handler, IAccountRepository repository, IMapper mapper) : IAccountService
{
    public async Task<DefaultResponse> GetAccountByUserGuid(Guid userGuid)
    {
        try
        {
            Domain.Accounts.Account? account = await repository.GetAccountByUserGuidAsync(userGuid);

            if (account is null)
                return new DefaultResponse(httpStatus: HttpStatusCode.NotFound);

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
            Domain.Accounts.Account? account = await repository.GetAccountByGuidAsync(accountModel.Guid);
            if (account is null)
                return new DefaultResponse(httpStatus: HttpStatusCode.NotFound, message: "Account not found.");

            account = account with
            {
                BirthDate = DateOnly.FromDateTime(accountModel.BirthDate),
                RealName =
                                                                                                    accountModel.RealName,
            };

            await repository.UpdateAccountAsync(account);

            return new DefaultResponse(message: "Account has been updated.");
        }
        catch (Exception ex)
        {
            return handler.HandleException(ex);
        }
    }
}
