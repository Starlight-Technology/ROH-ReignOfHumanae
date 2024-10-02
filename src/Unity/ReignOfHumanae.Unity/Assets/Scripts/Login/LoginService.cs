using Assets.Scripts.Connection.Api;

using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Login
{
    public class LoginService : MonoBehaviour
    {
        private ApiLoginService _loginService;

        public InputField LoginField;
        public InputField PasswordField;

        void Start()
        {
            var loginService = new GameObject("loginServiceObj");
            loginService.AddComponent<ApiLoginService>();
            _loginService = loginService.GetComponent<ApiLoginService>();
            _loginService.Start();
        }

        public void Login()
        {
            string loginText = LoginField.text;
            string passwordText = PasswordField.text;

            var response = _loginService.Login(new Models.Login.LoginModel() { Login = loginText, Password = passwordText }).Result;
        }
    }
}