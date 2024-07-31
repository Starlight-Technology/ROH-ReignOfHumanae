using Assets.Scripts.Helpers;
using Assets.Scripts.Models.File;
using Assets.Scripts.Models.Response;
using Assets.Scripts.Models.Version;

using Newtonsoft.Json;

using ROH.StandardModels.Paginator;

using System.Threading.Tasks;

using UnityEngine;

namespace Assets.Scripts.Connection.ApiConfiguration
{
    public class ApiVersionService
    {
        private readonly ApiClient _apiClient = new();

        public Task GetCurrentVersion()
        {
            _apiClient.Get<DefaultResponse>("Api/Version/GetCurrentVersion", (response) =>
            {
                if (response != null)
                {
                    GameVersionModel gameVersion = response.ResponseToModel<GameVersionModel>();

                    if (gameVersion != null && !gameVersion.Released)
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

        public async Task<PaginatedModel> GetReleasedVersions()
        {
            TaskCompletionSource<PaginatedModel> tcs = new();

            _apiClient.Get<DefaultResponse>("Api/Version/GetAllReleasedVersionsPaginated", (response) =>
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

            return await tcs.Task;
        }

        public async Task<DefaultResponse> GetVersionFiles(string guid)
        {
            TaskCompletionSource<DefaultResponse> tcs = new();

            _apiClient.Get<DefaultResponse>("api/VersionFile/GetAllVersionFiles?versionGuid=" + guid, (response) =>
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

            return await tcs.Task;
        }

        public async Task<FileModel> DownloadFile(string guid)
        {
            TaskCompletionSource<FileModel> tcs = new();

            _apiClient.Get<DefaultResponse>("api/VersionFile/DownloadFile?fileGuid=" + guid, (response) =>
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

            return await tcs.Task;
        }
    }
}
