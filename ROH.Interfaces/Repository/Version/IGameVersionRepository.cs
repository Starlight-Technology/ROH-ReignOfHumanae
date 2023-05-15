using ROH.Domain.Version;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROH.Interfaces.Repository.Version
{
    public interface IGameVersionRepository
    {
        Task<IList<GameVersion>?> GetAllReleasedVersions();
        Task<IList<GameVersion>?> GetAllVersions();
        Task<GameVersion?> GetCurrentGameVersion();
        Task<GameVersion> SetNewGameVersion(GameVersion version);
        Task<GameVersion> UpdateGameVersion(GameVersion version);
    }

}
