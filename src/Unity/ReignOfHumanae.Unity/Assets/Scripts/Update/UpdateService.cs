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

        private async Task DownloadFileAsync(GameVersionFileModel file, CancellationToken cancellationToken = default)
        {
            FileModel download = await _apiService.DownloadFileAsync(file.Guid.ToString(), cancellationToken).ConfigureAwait(true);
            if (download != null)
            {
                file.Content = download.Content;
                await SaveFileAsync(file, cancellationToken).ConfigureAwait(true);
            }
        }

        private bool HasNewVersion(GameVersionModel gameVersion, GameVersionModel currentGameVersion)
        {
            return (currentGameVersion.Version > gameVersion.Version) ||
                (currentGameVersion.Release > gameVersion.Release) ||
                (currentGameVersion.Review > gameVersion.Review);
        }

        private async Task LookForUpdateAsync(CancellationToken cancellationToken = default)
        {
            GameVersionModel gameVersion = new()
            {
                Version = PlayerPrefs.GetInt("version-version"),
                Release = PlayerPrefs.GetInt("version-release"),
                Review = PlayerPrefs.GetInt("version-review")
            };

            ChangeText("Verifying version...");

            await _apiService.GetCurrentVersionAsync(cancellationToken).ConfigureAwait(true);

            GameVersionModel currentGameVersion = new()
            {
                Version = PlayerPrefs.GetInt("current-version-version"),
                Release = PlayerPrefs.GetInt("current-version-release"),
                Review = PlayerPrefs.GetInt("current-version-review")
            };

            if (HasNewVersion(gameVersion, currentGameVersion))
                await VerifyFilesAsync(cancellationToken).ConfigureAwait(true);
        }

        private static bool ResponseHasGameVersions(PaginatedModel response)
        { return (response != null) && (response.ObjectResponse != null) && response.ObjectResponse.Any(); }

        private async Task SaveFileAsync(GameVersionFileModel file, CancellationToken cancellationToken = default)
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
                await fs.WriteAsync(file.Content.AsMemory(), cancellationToken).ConfigureAwait(true);
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Async/await", "CRR0034:An asynchronous method's name is missing an 'Async' suffix", Justification = "<Is a default for unity>")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "<Is a default for unity.>")]
        private async void Start()
        {
            try
            {
                GameObject versionService = new("verServiceObject");
                versionService.AddComponent<ApiVersionService>();
                _apiService = versionService.GetComponent<ApiVersionService>();
                _apiService.Start();

                ChangeText("Connecting to server...");
                filePath = Application.persistentDataPath;
                await LookForUpdateAsync(CancellationToken.None).ConfigureAwait(true);
            }
            catch (Exception)
            {
                ChangeText("An unexpected error has occurred.");
            }
        }

        private async Task VerifyFilesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                PaginatedModel response = await _apiService.GetReleasedVersionsAsync(cancellationToken).ConfigureAwait(true);
                if (ResponseHasGameVersions(response))
                {
                    string json = JsonConvert.SerializeObject(response.ObjectResponse);
                    ICollection<GameVersionModel> versions = JsonConvert.DeserializeObject<ICollection<GameVersionModel>>(
                        json);
                    foreach (GameVersionModel version in versions)
                    {
                        await VerifyIfFileExistAsync(version, cancellationToken).ConfigureAwait(true);
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

        private async Task VerifyIfFileExistAsync(GameVersionModel version, CancellationToken cancellationToken = default)
        {
            DefaultResponse response = await _apiService.GetVersionFilesAsync(version.Guid.ToString(), cancellationToken).ConfigureAwait(true);
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
                        await DownloadFileAsync(file, cancellationToken).ConfigureAwait(true);
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