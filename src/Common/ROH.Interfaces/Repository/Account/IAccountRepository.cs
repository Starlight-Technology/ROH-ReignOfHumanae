//-----------------------------------------------------------------------
// <copyright file="IAccountRepository.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ROH.Interfaces.Repository.Account;

public interface IAccountRepository
{
    Task<Domain.Accounts.Account?> GetAccountByGuidAsync(Guid guid, CancellationToken cancellationToken = default);

    ValueTask<Domain.Accounts.Account?> GetAccountByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<Domain.Accounts.Account?> GetAccountByUserGuidAsync(Guid guid, CancellationToken cancellationToken = default);

    Task UpdateAccountAsync(Domain.Accounts.Account account, CancellationToken cancellationToken = default);
}