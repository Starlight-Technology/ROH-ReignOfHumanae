//-----------------------------------------------------------------------
// <copyright file="IUserRepository.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using ROH.Domain.Accounts;

namespace ROH.Interfaces.Repository.Account;

public interface IUserRepository
{
    Task<User> CreateNewUserAsync(User user, CancellationToken cancellationToken = default);

    Task<bool> EmailInUseAsync(string email, CancellationToken cancellationToken = default);

    Task<User?> FindUserByEmailAsync(string email, CancellationToken cancellationToken = default);

    Task<User?> FindUserByUserNameAsync(string userName, CancellationToken cancellationToken = default);

    Task<User> GetUserByGuidAsync(Guid userGuid, CancellationToken cancellationToken = default);
}