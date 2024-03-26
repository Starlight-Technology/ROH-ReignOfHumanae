using Assets.Scripts.Connection.ApiConfiguration;
using Assets.Scripts.Helpers;
using Assets.Scripts.Models.Version;

using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Update
{
    public class UpdateService : MonoBehaviour
    {
        private readonly ObjectPropertyGetter<Text> _txtUpdate = new() { ObjectName = "txtUpdate" };
        private readonly ObjectPropertyGetter<Text> _txtUpdateBackground = new() { ObjectName = "txtUpdateBackground" };
        private readonly ApiService _apiService = new ApiService();

        private void Start()
        {
            ChangeText("Connecting to server...");
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

            return Task.CompletedTask;
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