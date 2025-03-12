//-----------------------------------------------------------------------
// <copyright file="ApiVersionService.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using Assets.Scripts.Connection.ApiConfiguration;
using Assets.Scripts.Helpers;
using Assets.Scripts.Models.File;
using Assets.Scripts.Models.Response;

using Assets.Scripts.Models.Version;

using Newtonsoft.Json;

using System.Threading;
using System.Threading.Tasks;

using UnityEngine;

namespace Assets.Scripts.Connection.Api
{
    public class ApiVersionService : MonoBehaviour
    {
        private ApiClient _apiClient;

        public Task<FileModel> DownloadFileAsync(string guid, CancellationToken cancellationToken = default)
        {
            TaskCompletionSource<FileModel> tcs = new();

            if (cancellationToken.IsCancellationRequested)
            {
                tcs.SetCanceled();
                return tcs.Task;
            }

            _apiClient.Get<DefaultResponse>(
                $"api/VersionFile/DownloadFile?fileGuid={guid}",
                (response) =>
                {
                    if (response != null)
                    {
                        string json = JsonConvert.SerializeObject(response.ObjectResponse);
                        FileModel file = JsonConvert.DeserializeObject<FileModel>(json);
                        tcs.SetResult(file);
                    }
                    else
                    {
                        Debug.LogError("Failed to retrieve response.");
                        tcs.SetResult(null);
                    }
                });

            return tcs.Task;
        }

        public Task GetCurrentVersionAsync(CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
                return Task.FromCanceled(cancellationToken);

            _apiClient.Get<DefaultResponse>(
                "Api/Version/GetCurrentVersion",
                (response) =>
                {
                    if (response != null)
                    {
                        GameVersionModel gameVersion = response.ResponseToModel<GameVersionModel>();

                        if ((gameVersion != null) && !gameVersion.Released)
                            return;

                        PlayerPrefs.SetInt("current-version-version", gameVersion!.Version);
                        PlayerPrefs.SetInt("current-version-release", gameVersion.Release);
                        PlayerPrefs.SetInt("current-version-review", gameVersion.Review);
                    }
                    else
                    {
                        Debug.LogError("Failed to retrieve response.");
                    }
                });

            return Task.CompletedTask;
        }

        public Task<PaginatedModel> GetReleasedVersionsAsync(CancellationToken cancellationToken = default)
        {
            TaskCompletionSource<PaginatedModel> tcs = new();

            if (cancellationToken.IsCancellationRequested)
            {
                tcs.SetCanceled();
                return tcs.Task;
            }

            _apiClient.Get<DefaultResponse>(
                "Api/Version/GetAllReleasedVersionsPaginated",
                (response) =>
                {
                    if (response != null)
                    {
                        PaginatedModel paginatedModel = response.ResponseToModel<PaginatedModel>();
                        tcs.SetResult(paginatedModel); // Set the result when the response is received
                    }
                    else
                    {
                        Debug.LogError("Failed to retrieve response.");
                        tcs.SetResult(null); // Set null if there's an error
                    }
                });

            return tcs.Task;
        }

        public Task<DefaultResponse> GetVersionFilesAsync(string guid, CancellationToken cancellationToken = default)
        {
            TaskCompletionSource<DefaultResponse> tcs = new();

            if (cancellationToken.IsCancellationRequested)
            {
                tcs.SetCanceled();
                return tcs.Task;
            }

            _apiClient.Get<DefaultResponse>(
                $"api/VersionFile/GetAllVersionFiles?versionGuid={guid}",
                (response) =>
                {
                    if (response != null)
                    {
                        tcs.SetResult(response);
                    }
                    else
                    {
                        Debug.LogError("Failed to retrieve response.");
                        tcs.SetResult(null);
                    }
                });

            return tcs.Task;
        }

        public void Start()
        {
            GameObject apiClientObject = GameObject.Find("apiClientObj");

            if (apiClientObject == null)
            {
                apiClientObject = new("apiClientObj");
                apiClientObject.AddComponent<ApiClient>();
            }

            _apiClient = apiClientObject.GetComponent<ApiClient>();
            _apiClient.Start();
        }
    }
}
