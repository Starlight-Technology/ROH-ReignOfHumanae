using Assets.Scripts.Connection.ApiConfiguration;
using Assets.Scripts.Models.File;
using Assets.Scripts.Models.Login;
using Assets.Scripts.Models.Response;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

namespace Assets.Scripts.Connection.Api
{
    public class ApiLoginService : MonoBehaviour
    {
        private ApiClient _apiClient;

        public void Start()
        {
            var apiClientObject = new GameObject("apiClientObj");
            apiClientObject.AddComponent<ApiClient>();

            _apiClient = apiClientObject.GetComponent<ApiClient>();
            _apiClient.Start();
        }

        public async Task<DefaultResponse> Login(LoginModel loginModel)
        {
            TaskCompletionSource<DefaultResponse> tcs = new();

            _apiClient.Post<DefaultResponse>("Api/Account/Login", loginModel, (response) =>
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
    }
}
