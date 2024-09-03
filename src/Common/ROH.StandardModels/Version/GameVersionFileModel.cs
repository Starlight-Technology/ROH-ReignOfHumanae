using ROH.StandardModels.File;

using System;

namespace ROH.StandardModels.Version
{
    public class GameVersionFileModel
    {
        public Guid Guid { get; set; }
        public string Name { get; set; }
        public long Size { get; set; }
        public string Path { get; set; }
        public string Format { get; set; }
        public byte[]? Content { get; set; }
        public virtual GameVersionModel? GameVersion { get; set; }

        public GameVersionFileModel(Guid? guid, string name = "", string path = "", string format = "", byte[]? content = null, long size = 0, GameVersionModel? gameVersion = null)
        {
            Guid = guid.GetValueOrDefault();
            Name = name;
            Path = path;
            Format = format;
            Content = content;
            Size = size;
            GameVersion = gameVersion;
        }

        public GameFileModel ToFileModel() => new GameFileModel(Name, Format, Content);

        public GameVersionFileListModel ToListModel() => new GameVersionFileListModel(Name, Size, Guid);
    }
}