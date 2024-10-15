//-----------------------------------------------------------------------
// <copyright file="CameraMovements.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class CameraMovements : MonoBehaviour
    {
        public float Distance { get; set; } = 10.0f;

        public Transform LookAt { get; set; }

        public Transform Player { get; set; }

        public float Sensitivity { get; set; } = 4.0f;
    }
}