//-----------------------------------------------------------------------
// <copyright file="DataManager.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using Assets.Scripts.Models.Configuration;

using System;
using System.IO;

using UnityEngine;

namespace Assets.Scripts.Helpers
{
    public static class DataManager
    {
        public static string assetsFolderPath = Application.dataPath;
        public static string rootFolder = Directory.GetParent(assetsFolderPath).FullName;
        public static string configurationPath = $"{rootFolder}config";

        public static T LoadData<T>(string filePath)
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);

                return JsonUtility.FromJson<T>(json);
            }
            else
            {
                Debug.LogWarning("File does not exist.");
                return default;
            }
        }

        public static ConfigurationModel GetConfiguration() => LoadData<ConfigurationModel>(configurationPath);

        public static void UpdateConfiguration(ConfigurationModel config) => SaveData<ConfigurationModel>(
            config,
            configurationPath);

        public static void SaveData<T>(this object data, string filePath)
        {
            T dataToSerialize = (T)Convert.ChangeType(data, typeof(T));
            string json = JsonUtility.ToJson(dataToSerialize);
            File.WriteAllText(filePath, json);
        }
    }
}
