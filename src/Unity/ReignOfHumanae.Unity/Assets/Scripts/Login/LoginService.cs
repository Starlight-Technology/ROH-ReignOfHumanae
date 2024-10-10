//-----------------------------------------------------------------------
// <copyright file="LoginService.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using Assets.Scripts.Connection.Api;
using Assets.Scripts.Models.Login;

using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Login
{
    public class LoginService : MonoBehaviour
    {
        private readonly ApiLoginService _loginService;
        public InputField LoginField;
        public InputField PasswordField;

        public void Login()
        {
            string loginText = LoginField.text;
            string passwordText = PasswordField.text;

            _ = _loginService.Login(new LoginModel { Login = loginText, Password = passwordText }).Result;
        }
    }
}