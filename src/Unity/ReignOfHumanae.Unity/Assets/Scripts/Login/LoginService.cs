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
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Unity.VisualScripting;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.Login
{
    public class LoginService : MonoBehaviour
    {
        private ApiLoginService _loginService;

        public InputField LoginField;
        public InputField PasswordField;
        public GameObject SuccessPopUpObj;
        public GameObject ErrorPopUpObj;
        public GameObject LoginUiObj;
        public GameObject UpdaterObj;

        public async void Login()
        {
            string loginText = LoginField.text;
            string passwordText = PasswordField.text;

            try
            {
                LoginUiObj.SetActive(false);
                var loginResult = await _loginService.LoginAsync(new LoginModel { Login = loginText, Password = passwordText }, CancellationToken.None).ConfigureAwait(true);

                Debug.LogError(DataManager.configurationPath);

                if (loginResult != null && loginResult.HttpStatus == System.Net.HttpStatusCode.OK)
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
                        SuccessPopUpObj.GetComponent<PopUpService>().ShowPopup($"Welcome, {user.UserName}!");
                        UpdaterObj.SetActive(true);
                        UpdaterObj.GetComponent<UpdateService>().RunUpdaterAsync().ConfigureAwait(false);
                    });
                }
                else
                {
                    Debug.LogError($"Login failed: {loginResult?.Message ?? string.Empty}");
                    ErrorPopUpObj.GetComponent<PopUpService>().ShowPopup($"Login failed: {loginResult?.Message ?? string.Empty}");
                }

            }
            catch (Exception ex)
            {
                Debug.LogError($"An exception occurred during login: {ex.Message}");

                ErrorPopUpObj.GetComponent<PopUpService>().ShowPopup("An error occurred during login. Please try again.");

                LoginUiObj.SetActive(true);
            }
        }

        public void ShowLogin()
        {
            LoginUiObj.SetActive(true);
        }

        public async void LoadGameScene()
        {
            await SceneManager.LoadSceneAsync("MainWorld");
        }

        public void ExitGame()
        {
            Application.Quit();
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