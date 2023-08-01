using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROH.Models.Version
{
    public record GameVersionFileModel(string Name, long Size, string Path, string Format)
    {
        public virtual GameVersionModel? GameVersion { get; set; }
    }

}
