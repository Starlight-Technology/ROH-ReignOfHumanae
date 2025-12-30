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
    public static class InitialConfiguration
    {
        public static ConfigurationModel GetInitialConfiguration()
        {

            ConfigurationModel config = DataManager.LoadData<ConfigurationModel>(DataManager.configurationPath) ??
                new ConfigurationModel();
            config.ServerUrl ??= "http://localhost:9001";
            config.ServerUrlWebSocket ??= "ws://localhost:9001";

            DataManager.SaveData<ConfigurationModel>(config, DataManager.configurationPath);

            return config;
        }
    }
}
