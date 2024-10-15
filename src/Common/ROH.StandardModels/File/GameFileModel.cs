//-----------------------------------------------------------------------
// <copyright file="GameFileModel.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace ROH.StandardModels.File
{
    public class GameFileModel
    {
        public GameFileModel(string name, string format, byte[]? content, long size, bool active)
        {
            Name = name;
            Format = format;
            Content = content;
            Size = size;
            Active = active;
        }

        public bool Active { get; set; }

        public byte[]? Content { get; set; }

        public string Format { get; set; }

        public string Name { get; set; }

        public long Size { get; set; }
    }
}
