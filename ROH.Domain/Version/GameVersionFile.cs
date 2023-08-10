
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROH.Domain.Version
{
    public record GameVersionFile(long Id = 0, long IdVersion = 0, string Name = "", long Size = 0, string Path = "", string Format = "")
    {
        public virtual GameVersion? GameVersion { get; set; }
    }
}
