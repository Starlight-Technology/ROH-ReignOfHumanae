using Assets.Scripts.Helpers;
using Assets.Scripts.Models.Configuration;

using UnityEngine;

namespace Assets.Scripts.Configurations
{
    public class InitialConfiguration : MonoBehaviour
    {
        private void Start()
        {
        }

        public ConfigurationModel GetInitialConfiguration()
        {
            var dataManagerObject = new GameObject("dataManagerObj");
            dataManagerObject.AddComponent<DataManager>();

            var dataManager = dataManagerObject.GetComponent<DataManager>();

            string assetsFolderPath = Application.dataPath;
            string rootFolder = System.IO.Directory.GetParent(assetsFolderPath).FullName;
            string configurationPath = rootFolder + "config";
            ConfigurationModel config = dataManager.LoadData<ConfigurationModel>(configurationPath) ?? new ConfigurationModel();
            config.ServerUrl ??= "http://localhost:9001";

            dataManager.SaveData<ConfigurationModel>(config, configurationPath);

            return config;
        }
    }
}