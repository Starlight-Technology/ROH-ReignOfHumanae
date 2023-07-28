using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROH.StandardModels.Version
{
    public class GameVersionFileModel
    {
        public string Name { get; }
        public long Size { get; }
        public string Path { get; }
        public string Format { get; }
        public virtual GameVersionModel? GameVersion { get; set; }

        public GameVersionFileModel(string name, long size, string path, string format)
        {
            Name = name;
            Size = size;
            Path = path;
            Format = format;
        }
    }


}
