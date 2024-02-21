using ROH.StandardModels.File;
using ROH.StandardModels.Response;

using System;
using System.Threading.Tasks;

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

        public GameVersionFileModel(string name = "", string path = "", string format = "", string content = "", long size = 0, GameVersionModel? gameVersion = null)
        {
            Name = name;
            Path = path;
            Format = format;
            Content = content;
            Size = size;
            GameVersion = gameVersion;
        }

        public FileModel ToFileModel() => new FileModel(Name, Format, Content);

        public GameVersionFileListModel ToListModel() => new GameVersionFileListModel(Name, Size, Guid);
    }

    public class GameVersionFileListModel
    {
        public string Name { get; set; }
        public string Size { get; set; }
        public Guid FileGuid { get; set; }
        public Func<Task<DefaultResponse?>>? DownloadFile { get; set; }

        public GameVersionFileListModel(string name, long size, Guid fileGuid)
        {
            double sizeInMegaBytes = (double)size / 1024 / 1024;

            Name = name;
            Size = $"{Math.Round(sizeInMegaBytes, 2)} Mb";
            FileGuid = fileGuid;
        }
    }
}