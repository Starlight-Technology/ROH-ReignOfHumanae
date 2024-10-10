//-----------------------------------------------------------------------
// <copyright file="UserModel.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;

namespace Assets.Scripts.Models.Account
{
    [Serializable]
    public class UserModel
    {
        public string Email { get; set; }

        public Guid Guid { get; set; }

        public string Password { get; set; }

        public string Token { get; set; }

        public string UserName { get; set; }
    }
}
