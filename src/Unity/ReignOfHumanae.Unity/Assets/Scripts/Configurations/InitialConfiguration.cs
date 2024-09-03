using Assets.Scripts.Helpers;
using Assets.Scripts.Models.Configuration;

using UnityEngine;

namespace Assets.Scripts.Configurations
{
    public class InitialConfiguration : MonoBehaviour
    {
        private readonly DataManager _dataManager = new();

        [RuntimeInitializeOnLoadMethod]
        public ConfigurationModel GetInitialConfiguration()
        {
            string assetsFolderPath = Application.dataPath;
            string rootFolder = System.IO.Directory.GetParent(assetsFolderPath).FullName;
            string configurationPath = rootFolder + "config";
            ConfigurationModel config = _dataManager.LoadData<ConfigurationModel>(configurationPath) ?? new ConfigurationModel();
            config.ServerUrl ??= "http://192.168.0.65:9001";

            _dataManager.SaveData<ConfigurationModel>(config, configurationPath);

            return config;
        }
    }
}
