//-----------------------------------------------------------------------
// <copyright file="FileModel.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;

namespace Assets.Scripts.Models.File
{
    [Serializable]
    public class FileModel
    {
        public FileModel(string name, string format, byte[] content)
        {
            Name = name;
            Format = format;
            Content = content;
        }

        public byte[] Content { get; set; }

        public string Format { get; set; }

        public string Name { get; set; }
    }
}