//-----------------------------------------------------------------------
// <copyright file="UpdateService.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using Assets.Scripts.Connection.Api;
using Assets.Scripts.Helpers;
using Assets.Scripts.Models.File;
using Assets.Scripts.Models.Response;
using Assets.Scripts.Models.Version;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Update
{
    public class UpdateService : MonoBehaviour
    {
        private ApiVersionService _apiService;
        private readonly ObjectPropertyGetter<Text> _txtUpdate = new() { ObjectName = "txtUpdate" };
        private readonly ObjectPropertyGetter<Text> _txtUpdateBackground = new() { ObjectName = "txtUpdateBackground" };
        private string filePath;

        private void ChangeText(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return;
            }

            _txtUpdate.SetValue(text);
            _txtUpdateBackground.SetValue(text);
        }

        private async Task DownloadFile(GameVersionFileModel file)
        {
            FileModel download = await _apiService.DownloadFile(file.Guid.ToString());
            if (download != null)
            {
                file.Content = download.Content;
                await SaveFileAsync(file);
            }
        }

        private bool HasNewVersion(GameVersionModel gameVersion, GameVersionModel currentGameVersion)
        {
            return (currentGameVersion.Version > gameVersion.Version) ||
                (currentGameVersion.Release > gameVersion.Release) ||
                (currentGameVersion.Review > gameVersion.Review);
        }

        private async Task LookForUpdate()
        {
            GameVersionModel gameVersion = new()
            {
                Version = PlayerPrefs.GetInt("version-version"),
                Release = PlayerPrefs.GetInt("version-release"),
                Review = PlayerPrefs.GetInt("version-review")
            };

            ChangeText("Verifying version...");

            await _apiService.GetCurrentVersion();

            GameVersionModel currentGameVersion = new()
            {
                Version = PlayerPrefs.GetInt("current-version-version"),
                Release = PlayerPrefs.GetInt("current-version-release"),
                Review = PlayerPrefs.GetInt("current-version-review")
            };

            if (HasNewVersion(gameVersion, currentGameVersion))
                await VerifyFiles();
        }

        private static bool ResponseHasGameVersions(PaginatedModel response)
        { return (response != null) && (response.ObjectResponse != null) && response.ObjectResponse.Any(); }

        private async Task SaveFileAsync(GameVersionFileModel file)
        {
            try
            {
                if (file.Content is null)
                {
                    Debug.LogWarning("File content is null");
                    return;
                }

                if (File.Exists(file.Path))
                {
                    File.Delete(file.Path);
                }

                using FileStream fs = File.Create(file.Path);
                await fs.WriteAsync(file.Content.AsMemory(), CancellationToken.None);
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }

        private async void Start()
        {
            GameObject versionService = new("verServiceObject");
            versionService.AddComponent<ApiVersionService>();
            _apiService = versionService.GetComponent<ApiVersionService>();
            _apiService.Start();

            ChangeText("Connecting to server...");
            filePath = Application.persistentDataPath;
            await LookForUpdate();
        }

        private async Task VerifyFiles()
        {
            try
            {
                PaginatedModel response = await _apiService.GetReleasedVersions();
                if (ResponseHasGameVersions(response))
                {
                    string json = JsonConvert.SerializeObject(response.ObjectResponse);
                    ICollection<GameVersionModel> versions = JsonConvert.DeserializeObject<ICollection<GameVersionModel>>(
                        json);
                    foreach (GameVersionModel version in versions)
                    {
                        await VerifyIfFileExist(version);
                    }
                }
                else
                {
                    ChangeText("A error has occurred, please contact the support.");
                }
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }

        private async Task VerifyIfFileExist(GameVersionModel version)
        {
            DefaultResponse response = await _apiService.GetVersionFiles(version.Guid.ToString());
            string json = JsonConvert.SerializeObject(response.ObjectResponse);
            ICollection<GameVersionFileModel> files = JsonConvert.DeserializeObject<ICollection<GameVersionFileModel>>(
                json);
            foreach (GameVersionFileModel file in files)
            {
                string versionFolder = $@"{filePath}/{version.Version}.{version.Release}.{version.Review}";

                if (!Directory.Exists(versionFolder))
                {
                    _ = Directory.CreateDirectory(versionFolder);
                }

                file.Path = @$"{versionFolder}/{file.Name}";

                if (file.Active)
                {
                    if (!File.Exists(file.Path))
                        await DownloadFile(file);
                }
                else if (File.Exists(file.Path))
                    File.Delete(file.Path);
            }
        }

        public void SaveVersionData(List<GameVersionModel> data)
        {
            string json = JsonUtility.ToJson(data);
            File.WriteAllText(filePath, json);
        }
    }
}