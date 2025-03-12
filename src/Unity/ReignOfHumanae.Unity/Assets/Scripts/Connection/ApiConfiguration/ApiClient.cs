//-----------------------------------------------------------------------
// <copyright file="ApiClient.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using Assets.Scripts.Configurations;
using Assets.Scripts.Helpers;
using Assets.Scripts.Models.Configuration;

using Newtonsoft.Json;

using System;
using System.Collections;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Unity.VisualScripting.Antlr3.Runtime;

using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.Connection.ApiConfiguration
{
    public class ApiClient : MonoBehaviour
    {
        private Uri _baseUrl;

        private IEnumerator DeleteRequest(string endpoint, Action<bool> callback)
        {
            string token = string.Empty;

            UnityWebRequest webRequest = UnityWebRequest.Delete(new Uri(_baseUrl, endpoint));

            PlayerPrefs.GetString("Token", token);

            if (string.IsNullOrWhiteSpace(token))
                token = DataManager.LoadData<ConfigurationModel>(DataManager.configurationPath).JwToken;

            webRequest.SetRequestHeader("Authorization", $"Bearer {token}");

            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"DELETE Error: {webRequest.error}");
                callback(false);
            }
            else
            {
                callback(true);
            }
        }

        private IEnumerator GetRequest<T>(string endpoint, Action<T> callback)
        {
            string token = string.Empty;

            UnityWebRequest webRequest = UnityWebRequest.Get(new Uri(_baseUrl, endpoint));

            PlayerPrefs.GetString("Token", token);

            if (string.IsNullOrWhiteSpace(token))
                token = DataManager.GetConfiguration().JwToken;

            webRequest.SetRequestHeader("Authorization", $"Bearer {token}");

            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"GET Error: {webRequest.error}");
                callback(default);
            }
            else
            {
                T response = JsonConvert.DeserializeObject<T>(webRequest.downloadHandler.text);
                callback(response);
            }
        }

        private IEnumerator PostRequest<T>(string endpoint, object data, Action<T> callback)
        {
            string jsonData = JsonConvert.SerializeObject(data);
            string token = string.Empty;
            UnityWebRequest webRequest = new(new Uri(_baseUrl, endpoint), "POST");
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");

            PlayerPrefs.GetString("Token", token);

            if (string.IsNullOrWhiteSpace(token))
                token = DataManager.LoadData<ConfigurationModel>(DataManager.configurationPath).JwToken;

            webRequest.SetRequestHeader("Authorization", $"Bearer {token}");

            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"POST Error: {webRequest.error}");
                callback(default);
            }
            else
            {
                T response = JsonConvert.DeserializeObject<T>(webRequest.downloadHandler.text);
                callback(response);
            }
        }

        private IEnumerator PutRequest<T>(string endpoint, object data, Action<T> callback)
        {
            string jsonData = JsonConvert.SerializeObject(data);
            string token = string.Empty;
            UnityWebRequest webRequest = new(new Uri(_baseUrl, endpoint), "PUT");
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");

            PlayerPrefs.GetString("Token", token);

            if (string.IsNullOrWhiteSpace(token))
                token = DataManager.LoadData<ConfigurationModel>(DataManager.configurationPath).JwToken;

            webRequest.SetRequestHeader("Authorization", $"Bearer {token}");

            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"PUT Error: {webRequest.error}");
                callback(default);
            }
            else
            {
                T response = JsonConvert.DeserializeObject<T>(webRequest.downloadHandler.text);
                callback(response);
            }
        }

        public void Delete(string endpoint, Action<bool> callback)
        { StartCoroutine(DeleteRequest(endpoint, callback)); }

        public void Get<T>(string endpoint, Action<T> callback)
        { StartCoroutine(GetRequest(endpoint, callback)); }

        public void Post<T>(string endpoint, object data, Action<T> callback)
        { StartCoroutine(PostRequest(endpoint, data, callback)); }

        public void Put<T>(string endpoint, object data, Action<T> callback)
        { StartCoroutine(PutRequest(endpoint, data, callback)); }

        public async Task<T> PostAsync<T>(string endpoint, object data)
        {
            string jsonData = JsonConvert.SerializeObject(data);
            string token = string.Empty;
            UnityWebRequest webRequest = new UnityWebRequest(new Uri(_baseUrl, endpoint), "POST");
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");

            PlayerPrefs.GetString("Token", token);

            if (string.IsNullOrWhiteSpace(token))
                token = DataManager.LoadData<ConfigurationModel>(DataManager.configurationPath).JwToken;

            webRequest.SetRequestHeader("Authorization", $"Bearer {token}");

            // Use UnityWebRequest's SendWebRequest as a task
            await webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"POST Error: {webRequest.error}");
                return default;
            }

            // Deserialize the response
            return JsonConvert.DeserializeObject<T>(webRequest.downloadHandler.text);
        }


        public void Start()
        {
            ConfigurationModel config = InitialConfiguration.GetInitialConfiguration();
            _baseUrl = new Uri(config.ServerUrl);
        }
    }
}
