﻿using Assets.Scripts.Configurations;
using Assets.Scripts.Models.Configuration;

using Newtonsoft.Json;

using System;
using System.Text;

using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.Connection.ApiConfiguration
{
    public class ApiClient : MonoBehaviour
    {
        private readonly Uri _baseUrl;
        private readonly InitialConfiguration _initialConfiguration = new();

        public ApiClient()
        {
            ConfigurationModel config = _initialConfiguration.GetInitialConfiguration();
            _baseUrl = new(config.ServerUrl);
        }

        public void Get<T>(string endpoint, Action<T> callback)
        {
            UnityWebRequest webRequest = UnityWebRequest.Get(new Uri(_baseUrl, endpoint));
            webRequest.SendWebRequest().completed += (asyncOperation) =>
            {
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
            };
        }

        public T Post<T>(string endpoint, object data)
        {
            string jsonData = JsonUtility.ToJson(data);
            using UnityWebRequest webRequest = new(new Uri(_baseUrl, endpoint), "POST");
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");
            _ = webRequest.SendWebRequest();
            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"POST Error: {webRequest.error}");
                return default;
            }
            return JsonConvert.DeserializeObject<T>(webRequest.downloadHandler.text);
        }

        public T Put<T>(string endpoint, object data)
        {
            string jsonData = JsonUtility.ToJson(data);
            using UnityWebRequest webRequest = new(new Uri(_baseUrl, endpoint), "PUT");
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");
            _ = webRequest.SendWebRequest();
            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"PUT Error: {webRequest.error}");
                return default;
            }
            return JsonConvert.DeserializeObject<T>(webRequest.downloadHandler.text);
        }

        public bool Delete(string endpoint)
        {
            using UnityWebRequest webRequest = UnityWebRequest.Delete(new Uri(_baseUrl, endpoint));
            _ = webRequest.SendWebRequest();
            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"DELETE Error: {webRequest.error}");
                return false;
            }
            return true;
        }
    }
}