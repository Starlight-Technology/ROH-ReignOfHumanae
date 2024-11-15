//-----------------------------------------------------------------------
// <copyright file="IAccountRepository.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ROH.Context.Account.Interface;

public interface IAccountRepository
{
    Task<Entity.Account?> GetAccountByGuidAsync(Guid guid, CancellationToken cancellationToken = default);

    ValueTask<Entity.Account?> GetAccountByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<Entity.Account?> GetAccountByUserGuidAsync(Guid guid, CancellationToken cancellationToken = default);

    Task UpdateAccountAsync(Entity.Account account, CancellationToken cancellationToken = default);
}