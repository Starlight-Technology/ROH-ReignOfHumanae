﻿using Assets.Scripts.Configurations;
using Assets.Scripts.Models.Configuration;

using Newtonsoft.Json;

using System;
using System.Collections;
using System.Text;

using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.Connection.ApiConfiguration
{
    public class ApiClient : MonoBehaviour
    {
        private Uri _baseUrl;
        private InitialConfiguration _initialConfiguration = new();

        public void Start()
        {
            ConfigurationModel config = _initialConfiguration.GetInitialConfiguration();
            _baseUrl = new Uri(config.ServerUrl);
        }

        public void Get<T>(string endpoint, Action<T> callback)
        {
            StartCoroutine(GetRequest(endpoint, callback));
        }

        private IEnumerator GetRequest<T>(string endpoint, Action<T> callback)
        {
            UnityWebRequest webRequest = UnityWebRequest.Get(new Uri(_baseUrl, endpoint));
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

        public void Post<T>(string endpoint, object data, Action<T> callback)
        {
            StartCoroutine(PostRequest(endpoint, data, callback));
        }

        private IEnumerator PostRequest<T>(string endpoint, object data, Action<T> callback)
        {
            string jsonData = JsonConvert.SerializeObject(data);
            UnityWebRequest webRequest = new UnityWebRequest(new Uri(_baseUrl, endpoint), "POST");
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");

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

        public void Put<T>(string endpoint, object data, Action<T> callback)
        {
            StartCoroutine(PutRequest(endpoint, data, callback));
        }

        private IEnumerator PutRequest<T>(string endpoint, object data, Action<T> callback)
        {
            string jsonData = JsonConvert.SerializeObject(data);
            UnityWebRequest webRequest = new UnityWebRequest(new Uri(_baseUrl, endpoint), "PUT");
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");

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
        {
            StartCoroutine(DeleteRequest(endpoint, callback));
        }

        private IEnumerator DeleteRequest(string endpoint, Action<bool> callback)
        {
            UnityWebRequest webRequest = UnityWebRequest.Delete(new Uri(_baseUrl, endpoint));
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
    }
}
