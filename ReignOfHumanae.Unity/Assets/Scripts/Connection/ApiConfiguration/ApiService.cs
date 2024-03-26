using Assets.Scripts.Models.Response;
using Assets.Scripts.Models.Version;

using UnityEngine;

namespace Assets.Scripts.Connection.ApiConfiguration
{
    public class ApiService
    {

        public void GetCurrentVersion()
        {
            ApiClient apiClient = new();
            apiClient.Get<DefaultResponse>("Api/Version/GetCurrentVersion", (response) =>
            {
                if (response != null)
                {
                    // Handle the response here
                    Debug.Log($"Received response: {response}");

                    GameVersionModel gameVersion = response.ObjectResponse as GameVersionModel;

                    if (!gameVersion.Released)
                        return;

                    PlayerPrefs.SetInt("current-version-version", gameVersion.Version);
                    PlayerPrefs.SetInt("current-version-release", gameVersion.Release);
                    PlayerPrefs.SetInt("current-version-review", gameVersion.Review);
                }
                else
                {
                    Debug.LogError("Failed to retrieve response.");
                }
            });
        }
    }
}
