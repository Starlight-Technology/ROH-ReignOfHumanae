using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROH.Domain.Version
{
    public record GameVersionFile(long Id, long IdVersion, string Path, string Format)
    {
        public virtual GameVersion GameVersion { get; set; }
    }
}
