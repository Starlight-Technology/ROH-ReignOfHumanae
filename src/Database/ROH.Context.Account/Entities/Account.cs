//-----------------------------------------------------------------------
// <copyright file="Account.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace ROH.Context.Account.Entities;

public record Account(
    long Id = 0,
    long IdUser = 0,
    Guid Guid = default,
    string? RealName = null,
    DateOnly BirthDate = new())
{
    public User? User { get; init; }
}