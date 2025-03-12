//-----------------------------------------------------------------------
// <copyright file="ApiLoginService.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using Assets.Scripts.Connection.ApiConfiguration;
using Assets.Scripts.Models.Login;
using Assets.Scripts.Models.Response;

using Newtonsoft.Json;

using System;
using System.Threading;
using System.Threading.Tasks;

using UnityEngine;

namespace Assets.Scripts.Connection.Api
{
    public class ApiLoginService : MonoBehaviour
    {
        private ApiClient _apiClient;

        public async Task<DefaultResponse> LoginAsync(LoginModel loginModel, CancellationToken cancellationToken = default)
        {
            // Cancel the task if requested
            if (cancellationToken.IsCancellationRequested)
            {
                return null;
            }

            try
            {
                var response = await _apiClient.PostAsync<DefaultResponse>("Api/Account/Login", loginModel);

                if (response == null)
                {
                    Debug.LogError("Failed to retrieve response.");
                }

                return response;
            }
            catch (Exception ex)
            {
                Debug.LogError($"An error occurred: {ex.Message}");
                return null;
            }
        }

        public void Start()
        {
            GameObject apiClientObject = GameObject.Find("apiClientObj");

            if (apiClientObject == null)
            {
                apiClientObject = new("apiClientObj");
                apiClientObject.AddComponent<ApiClient>();
            }

            _apiClient = apiClientObject.GetComponent<ApiClient>();
            _apiClient.Start();
        }
    }
}
