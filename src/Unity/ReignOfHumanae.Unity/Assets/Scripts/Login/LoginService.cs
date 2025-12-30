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
using UnityEngine.UIElements;

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
        public GameObject CharacterList;
        public GameObject NewCharacterObj;

        public UserModel user;

        public async void Login()
        {
            string loginText = LoginField.text;
            string passwordText = PasswordField.text;

            try
            {
                LoginUiObj.SetActive(false);
                var loginResult = await _loginService.LoginAsync(new LoginModel { Login = loginText, Password = passwordText }, CancellationToken.None).ConfigureAwait(true);

                Debug.Log(DataManager.configurationPath);

                if (loginResult != null && loginResult.HttpStatus == System.Net.HttpStatusCode.OK)
                {
                    user = loginResult.ResponseToModel<UserModel>();
                    // Ensure this runs on the main thread
                    UnityMainThreadDispatcher.Instance().Enqueue(async () =>
                    {
                        PlayerPrefs.SetString("Token", user.Token);
                        PlayerPrefs.Save();

                        var config = DataManager.GetConfiguration();
                        config.JwToken = user.Token;
                        DataManager.UpdateConfiguration(config);
                        SuccessPopUpObj.GetComponent<PopUpService>().ShowPopup($"Welcome, {user.UserName}!");
                        UpdaterObj.SetActive(true);
                        await UpdaterObj.GetComponent<UpdateService>().RunUpdaterAsync().ConfigureAwait(true);

                        try
                        {
                            LoadCharactersAsync(user);
                        }
                        catch (Exception ex)
                        {
                            Debug.LogError($"An exception occurred during login: {ex.Message}");

                            UnityMainThreadDispatcher.Instance().Enqueue(() =>
                            {
                                ErrorPopUpObj.GetComponent<PopUpService>().ShowPopup("Failed to load characters.");
                                LoginUiObj.SetActive(true);
                            });
                        }
                    });
                }
                else
                {
                    Debug.LogError($"Login failed: {loginResult?.Message ?? string.Empty}");
                    LoginUiObj.SetActive(true);
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

        public void CreateNewCharacter()
        {
            NewCharacterObj.SetActive(true);
            CharacterList.SetActive(false);
        }

        public async Task LoadCharactersAsync(UserModel user)
        {
            GameObject apiCharacter = GameObject.Find("apiCharacter");
            if (GameObject.Find("apiCharacter") == null)
            {
                apiCharacter = new("apiCharacter");
                apiCharacter.AddComponent<ApiCharacterService>();
                apiCharacter.GetComponent<ApiCharacterService>().Start();
            }

            SuccessPopUpObj.SetActive(false);

            var characters = await apiCharacter.GetComponent<ApiCharacterService>()
                .GetAccountCharactersAsync(user.Guid)
                .ConfigureAwait(true);

            CharacterList.SetActive(true);
            var listUi = CharacterList.GetComponent<CharacterListUI>();
            listUi.LoadCharacters(characters);
        }

        public void ShowLogin()
        {
            LoginUiObj.SetActive(true);
        }

        public void CancelNewCharacter()
        {
            NewCharacterObj.SetActive(false); 
            CharacterList.SetActive(true);
        }

        public void CancelCharacterList()
        {
            CharacterList.SetActive(false);
            ShowLogin();
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