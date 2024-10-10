//-----------------------------------------------------------------------
// <copyright file="LoginModel.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;

using UnityEngine;

namespace Assets.Scripts.Models.Login
{
    [Serializable]
    public class LoginModel
    {
        [SerializeField]
        public string Login { get; set; }

        [SerializeField]
        public string Password { get; set; }
    }
}
