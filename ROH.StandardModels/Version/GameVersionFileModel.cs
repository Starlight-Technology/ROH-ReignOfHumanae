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
        public string Content { get; set; }
        public virtual GameVersionModel? GameVersion { get; set; }

        public GameVersionFileModel(string name = "", string path = "", string format = "", string content = "")
        {
            Name = name;
            Path = path;
            Format = format;
            Content = content;
        }
    }
}