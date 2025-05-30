//-----------------------------------------------------------------------
// <copyright file="ConfigurationModel.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using Assets.Scripts.Models.Version;

using System;

namespace Assets.Scripts.Models.Configuration
{
    [Serializable]
    public class ConfigurationModel
    {
        public string ServerUrl;

        public string ServerUrlGrpc;

        public string JwToken;

        public GameVersionModel LocalVersion;

        public GameVersionModel ServerVersion;
    }
}
