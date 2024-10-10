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

using System.Threading.Tasks;

using UnityEngine;

namespace Assets.Scripts.Connection.Api
{
    public class ApiLoginService : MonoBehaviour
    {
        private ApiClient _apiClient;

        public async Task<DefaultResponse> Login(LoginModel loginModel)
        {
            TaskCompletionSource<DefaultResponse> tcs = new();

            _apiClient.Post<DefaultResponse>(
                "Api/Account/Login",
                loginModel,
                (response) =>
                {
                    if (response != null)
                    {
                        string json = JsonConvert.SerializeObject(response);
                        DefaultResponse resp = JsonConvert.DeserializeObject<DefaultResponse>(json);
                        tcs.SetResult(resp);
                    }
                    else
                    {
                        Debug.LogError("Failed to retrieve response.");
                        tcs.SetResult(null);
                    }
                });

            return await tcs.Task;
        }

        public void Start()
        {
            GameObject apiClientObject = new("apiClientObj");
            apiClientObject.AddComponent<ApiClient>();

            _apiClient = apiClientObject.GetComponent<ApiClient>();
            _apiClient.Start();
        }
    }
}
