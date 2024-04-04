using System.IO;

using UnityEngine;

namespace Assets.Scripts.Helpers
{
    public class DataManager
    {
        public void SaveData(dynamic data, string filePath)
        {
            string json = JsonUtility.ToJson(data);
            File.WriteAllText(filePath, json);
        }

        public T LoadData<T>(string filePath)
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);

                return JsonUtility.FromJson<T>(json);
            }
            else
            {
                Debug.LogWarning("Version data file does not exist.");
                return default;
            }
        }
    }
}
