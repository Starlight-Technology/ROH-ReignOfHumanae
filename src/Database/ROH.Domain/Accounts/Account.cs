//-----------------------------------------------------------------------
// <copyright file="Account.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using ROH.Domain.Characters;

namespace ROH.Domain.Accounts;

public record Account(
    long Id = 0,
    long IdUser = 0,
    Guid Guid = default,
    string? RealName = null,
    DateOnly BirthDate = new())
{
    public ICollection<Character>? Characters { get; init; }

    public User? User { get; init; }
}