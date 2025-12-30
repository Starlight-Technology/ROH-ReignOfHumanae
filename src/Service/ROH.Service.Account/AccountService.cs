//-----------------------------------------------------------------------
// <copyright file="AccountService.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using AutoMapper;

using ROH.Context.Account.Interface;
using ROH.Service.Account.Interface;
using ROH.Service.Exception.Interface;
using ROH.StandardModels.Account;
using ROH.StandardModels.Response;

using System.Net;

namespace ROH.Service.Account;

public class AccountService(IExceptionHandler handler, IAccountRepository repository, IMapper mapper) : IAccountService
{
    public async Task<DefaultResponse> GetAccountByUserGuidAsync(Guid userGuid, CancellationToken cancellationToken = default)
    {
        try
        {
            Context.Account.Entity.Account? account = await repository.GetAccountByUserGuidAsync(userGuid, cancellationToken).ConfigureAwait(true);

            if (account is null)
                return new DefaultResponse(httpStatus: HttpStatusCode.NotFound);

            AccountModel model = mapper.Map<AccountModel>(account);

            return new DefaultResponse(objectResponse: model);
        }
        catch (System.Exception ex)
        {
            return handler.HandleException(ex);
        }
    }

    public async Task<DefaultResponse> UpdateAccountAsync(AccountModel accountModel, CancellationToken cancellationToken = default)
    {
        try
        {
            Context.Account.Entity.Account? account = await repository.GetAccountByGuidAsync(accountModel.Guid, cancellationToken).ConfigureAwait(true);
            if (account is null)
                return new DefaultResponse(httpStatus: HttpStatusCode.NotFound, message: "Account not found.");

            account = account with
            {
                BirthDate = DateOnly.FromDateTime(accountModel.BirthDate),
                RealName =
                                                                                                    accountModel.RealName,
            };

            await repository.UpdateAccountAsync(account, cancellationToken).ConfigureAwait(true);

            return new DefaultResponse(message: "Account has been updated.");
        }
        catch (System.Exception ex)
        {
            return handler.HandleException(ex);
        }
    }
}