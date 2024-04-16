using Assets.Scripts.Configurations;
using Assets.Scripts.Helpers;
using Assets.Scripts.Models.Configuration;

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
                    callback(JsonUtility.FromJson<T>(webRequest.downloadHandler.text));
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
            webRequest.SendWebRequest();
            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"POST Error: {webRequest.error}");
                return default;
            }
            return JsonUtility.FromJson<T>(webRequest.downloadHandler.text);
        }

        public T Put<T>(string endpoint, object data)
        {
            string jsonData = JsonUtility.ToJson(data);
            using UnityWebRequest webRequest = new(new Uri(_baseUrl, endpoint), "PUT");
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");
            webRequest.SendWebRequest();
            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"PUT Error: {webRequest.error}");
                return default;
            }
            return JsonUtility.FromJson<T>(webRequest.downloadHandler.text);
        }

        public bool Delete(string endpoint)
        {
            using UnityWebRequest webRequest = UnityWebRequest.Delete(new Uri(_baseUrl, endpoint));
            webRequest.SendWebRequest();
            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"DELETE Error: {webRequest.error}");
                return false;
            }
            return true;
        }
    }
}