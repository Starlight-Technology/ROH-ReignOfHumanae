//-----------------------------------------------------------------------
// <copyright file="AccountModel.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;

namespace ROH.StandardModels.Account
{
    public class AccountModel
    {
        public DateTime BirthDate { get; set; }

        public Guid Guid { get; set; }

        public string? RealName { get; set; }

        public UserModel? User { get; set; }
    }
}