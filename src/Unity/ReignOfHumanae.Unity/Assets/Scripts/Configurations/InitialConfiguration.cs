//-----------------------------------------------------------------------
// <copyright file="InitialConfiguration.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using Assets.Scripts.Helpers;
using Assets.Scripts.Models.Configuration;

using System.IO;

using UnityEngine;

namespace Assets.Scripts.Configurations
{
    public class InitialConfiguration : MonoBehaviour
    {
        public ConfigurationModel GetInitialConfiguration()
        {
            GameObject dataManagerObject = new("dataManagerObj");
            dataManagerObject.AddComponent<DataManager>();

            DataManager dataManager = dataManagerObject.GetComponent<DataManager>();

            string assetsFolderPath = Application.dataPath;
            string rootFolder = Directory.GetParent(assetsFolderPath).FullName;
            string configurationPath = $"{rootFolder}config";
            ConfigurationModel config = dataManager.LoadData<ConfigurationModel>(configurationPath) ??
                new ConfigurationModel();
            config.ServerUrl ??= "http://localhost:9001";

            dataManager.SaveData<ConfigurationModel>(config, configurationPath);

            return config;
        }
    }
}