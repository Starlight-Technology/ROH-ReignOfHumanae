﻿//-----------------------------------------------------------------------
// <copyright file="User.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System.Collections;
using System.Security.Cryptography;
using System.Text;

namespace ROH.Context.Account.Entity;

public record User(long Id = 0, long IdAccount = 0, Guid Guid = default, string? Email = null, string? UserName = null)
{
    public void SetPassword(string password)
    {
        if (string.IsNullOrEmpty(password))
            throw new ArgumentException("Password cannot be null or empty.");
        Salt = new byte[16];
        RandomNumberGenerator.Fill(Salt);

        byte[] combinedBytes = Encoding.UTF8.GetBytes($"{password}{Convert.ToBase64String(Salt)}");
        PasswordHash = SHA256.HashData(combinedBytes);
    }

    public bool VerifyPassword(string password)
    {
        if (string.IsNullOrEmpty(password) || PasswordHash == null || Salt == null)
            return false;
        byte[] combinedBytes = Encoding.UTF8.GetBytes($"{password}{Convert.ToBase64String(Salt)}");
        byte[] enteredPasswordHash = SHA256.HashData(combinedBytes);

        return StructuralComparisons.StructuralEqualityComparer.Equals(PasswordHash, enteredPasswordHash);
    }

    public virtual Account? Account { get; set; } = new Account();

    public byte[]? PasswordHash { get; set; }

    public byte[]? Salt { get; set; }
}