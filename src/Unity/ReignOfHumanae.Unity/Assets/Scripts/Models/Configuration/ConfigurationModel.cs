//-----------------------------------------------------------------------
// <copyright file="ConfigurationModel.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;

namespace Assets.Scripts.Models.Configuration
{
    [Serializable]
    public class ConfigurationModel
    {
        public string ServerUrl;

        public string JwToken;
    }
}
