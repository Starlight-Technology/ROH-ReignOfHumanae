using Assets.Scripts.Models.Version;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROH.Models.Version
{
    public record GameVersionFileModel
    {
        public string Name { get; set; }
        public long Size { get; set; }
        public string Path { get; set; }
        public string Format { get; set; }
        public string Content { get; set; }
        public virtual GameVersionModel GameVersion { get; set; }
    }

}
