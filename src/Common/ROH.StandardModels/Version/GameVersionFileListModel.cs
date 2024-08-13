using ROH.StandardModels.Response;

using System;
using System.Threading.Tasks;

namespace ROH.StandardModels.Version
{
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