//-----------------------------------------------------------------------
// <copyright file="LoginService.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using Assets.Scripts.Connection.Api;
using Assets.Scripts.Connection.ApiConfiguration;
using Assets.Scripts.Helpers;
using Assets.Scripts.Login;
using Assets.Scripts.Models.Account;
using Assets.Scripts.Models.Login;
using Assets.Scripts.PopUp;
using Assets.Scripts.Update;

using System;
using System.Threading;
using System.Threading.Tasks;

using Unity.VisualScripting;

using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Login
{
    public class LoginService : MonoBehaviour
    {
        private ApiLoginService _loginService;

        public InputField LoginField;
        public InputField PasswordField;
        public GameObject SuccessPopUpObj;
        public GameObject LoginUiObj;
        public GameObject UpdaterObj;

        public async void Login()
        {
            string loginText = LoginField.text;
            string passwordText = PasswordField.text;

            try
            {
                var loginResult = await _loginService.LoginAsync(new LoginModel { Login = loginText, Password = passwordText }, CancellationToken.None).ConfigureAwait(true);


                if (loginResult.HttpStatus == System.Net.HttpStatusCode.OK)
                {
                    var user = loginResult.ResponseToModel<UserModel>();
                    // Ensure this runs on the main thread
                    UnityMainThreadDispatcher.Instance().Enqueue(() =>
                    {
                        PlayerPrefs.SetString("Token", user.Token);
                        PlayerPrefs.Save();

                        var config = DataManager.GetConfiguration();
                        config.JwToken = user.Token;
                        DataManager.UpdateConfiguration(config);
                        Debug.Log(PlayerPrefs.GetString("Token"));
                        SuccessPopUpObj.GetComponent<SuccessPopUp>().ShowPopup($"Welcome, {user.UserName}!");
                        LoginUiObj.SetActive(false);
                        UpdaterObj.SetActive(true);
                        UpdaterObj.GetComponent<UpdateService>().RunUpdaterAsync().ConfigureAwait(false);
                    });
                }
                else
                {
                    Debug.LogError($"Login failed: {loginResult.Message}");
                }

            }
            catch (Exception ex)
            {
                Debug.LogError($"An exception occurred during login: {ex.Message}");
            }
        }



        public void Start()
        {
            GameObject loginServiceObject = new("loginServiceObject");
            loginServiceObject.AddComponent<ApiLoginService>();

            _loginService = loginServiceObject.GetComponent<ApiLoginService>();
            _loginService.Start();
        }

    }
}