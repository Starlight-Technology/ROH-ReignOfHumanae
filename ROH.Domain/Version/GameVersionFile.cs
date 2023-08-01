using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROH.Domain.Version
{
    public record GameVersionFile(long Id, long IdVersion, string Name, long Size, string Path, string Format, string Content)
    {
        public virtual GameVersion? GameVersion { get; set; }
    }
}
