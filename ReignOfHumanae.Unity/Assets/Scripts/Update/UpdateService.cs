using Assets.Scripts.Models.Version;

using System.Threading.Tasks;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Update
{
    public class UpdateService : MonoBehaviour
    {
        public TextMeshProUGUI TxtUpdate { get; set; }
        public Button BtnLogin { get; set; }

        // Start is called before the first frame update
        private void Start()
        {
            TxtUpdate.text = "Looking for updates...";
            TxtUpdate.color = Color.red;
            LookForUpdate().Wait();
        }

        private Task LookForUpdate()
        {
            GameVersionModel gameVersion = new ()
            {
                Version = PlayerPrefs.GetInt("version-version"),
                Release = PlayerPrefs.GetInt("version-release"),
                Review = PlayerPrefs.GetInt("version-review")
            };

            return Task.CompletedTask;
        }
    }
}