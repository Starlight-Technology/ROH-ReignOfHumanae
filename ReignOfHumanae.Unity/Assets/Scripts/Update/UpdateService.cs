using Assets.Scripts.Connection.ApiConfiguration;
using Assets.Scripts.Helpers;
using Assets.Scripts.Models.Version;

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Update
{
    public class UpdateService : MonoBehaviour
    {
        private readonly ObjectPropertyGetter<Text> _txtUpdate = new() { ObjectName = "txtUpdate" };
        private readonly ObjectPropertyGetter<Text> _txtUpdateBackground = new() { ObjectName = "txtUpdateBackground" };
        private readonly ApiService _apiService = new();
        private string filePath;

        private void Start()
        {
            ChangeText("Connecting to server...");
            filePath = Path.Combine(Application.persistentDataPath, "version_data");
            LookForUpdate().Wait();

        }

        private Task LookForUpdate()
        {
            GameVersionModel gameVersion = new()
            {
                Version = PlayerPrefs.GetInt("version-version"),
                Release = PlayerPrefs.GetInt("version-release"),
                Review = PlayerPrefs.GetInt("version-review")
            };

            ChangeText("Verifying version..."); 
             
            _apiService.GetCurrentVersion();

            GameVersionModel currentGameVersion = new()
            {
                Version = PlayerPrefs.GetInt("current-version-version"),
                Release = PlayerPrefs.GetInt("current-version-release"),
                Review = PlayerPrefs.GetInt("current-version-review")
            };

            if (HasNewVersion(gameVersion, currentGameVersion))
                VerifyFiles();

            return Task.CompletedTask;
        }

        private bool HasNewVersion(GameVersionModel gameVersion, GameVersionModel currentGameVersion) => currentGameVersion.Version > gameVersion.Version || currentGameVersion.Release > gameVersion.Release || currentGameVersion.Review > gameVersion.Review;

        private void VerifyFiles() => throw new NotImplementedException();

        public void SaveVersionData(List<GameVersionModel> data)
        {
            string json = JsonUtility.ToJson(data);
            File.WriteAllText(filePath, json);
        }

        private void ChangeText(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return;
            }

            _txtUpdate.SetValue(text);
            _txtUpdateBackground.SetValue(text);
        }
    }
}