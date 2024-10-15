//-----------------------------------------------------------------------
// <copyright file="DataManager.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.IO;

using UnityEngine;

namespace Assets.Scripts.Helpers
{
    public class DataManager : MonoBehaviour
    {
        public T LoadData<T>(string filePath)
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);

                return JsonUtility.FromJson<T>(json);
            }
            else
            {
                Debug.LogWarning("File does not exist.");
                return default;
            }
        }

        public void SaveData<T>(object data, string filePath)
        {
            T dataToSerialize = (T)Convert.ChangeType(data, typeof(T));
            string json = JsonUtility.ToJson(dataToSerialize);
            File.WriteAllText(filePath, json);
        }

        public void Start()
        {
            // Method intentionally left empty.
        }
    }
}
