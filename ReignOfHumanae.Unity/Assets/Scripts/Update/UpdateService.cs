using Assets.Scripts.Models.Version;

using System;
using System.Collections;
using System.Collections.Generic;
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
        private GameVersionModel gameVersion;

        // Start is called before the first frame update
        async void Start()
        {
            TxtUpdate.text = "Looking for updates...";
            TxtUpdate.color = Color.red;
            await LookForUpdate();
        }

        private Task LookForUpdate()
        {
            gameVersion = new GameVersionModel();

            gameVersion.Version = PlayerPrefs.GetInt("version-version");
            gameVersion.Release = PlayerPrefs.GetInt("version-release");
            gameVersion.Review = PlayerPrefs.GetInt("version-review");



            return Task.CompletedTask;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}