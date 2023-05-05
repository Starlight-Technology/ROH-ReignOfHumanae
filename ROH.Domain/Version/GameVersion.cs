using System;
using System.Collections.Generic;
using System.Linq;
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
    public record GameVersion(long Id, int Version, int Release, int Review, bool Released)
    {
        private DateTime versionDate;
        private DateTime releaseDate;

        public DateTime VersionDate { get => versionDate; set => versionDate = DateTime.Now; }
        public DateTime ReleaseDate { get => releaseDate; set => releaseDate = DateTime.Now; }
    }
}
