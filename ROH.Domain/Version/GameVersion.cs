using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ROH.Domain.Version
{
    /// <summary>
    /// Version is the version of the game, greate changes like something in history
    /// Release is for changes like events or new itens
    /// Review is for fixes
    /// </summary>
    /// <param name="Id"></param>
    /// <param name="Version"></param>
    /// <param name="Release"></param>
    /// <param name="Review"></param>
    /// <param name="Released"></param>
    public record GameVersion(long Id = 0, int Version = 0, int Release = 0, int Review = 0, bool Released = false)
    {
        public DateTime? ReleaseDate { get; set; }
        public DateTime VersionDate { get; set; }
    }
 
}
