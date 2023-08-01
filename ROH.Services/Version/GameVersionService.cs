using ROH.Domain.Version;
using ROH.Interfaces.Repository.Version;
using ROH.Interfaces.Services.Version;
using ROH.Models.Response;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROH.Services.Version
{
    public class GameVersionService : IGameVersionService
    {
        private readonly IGameVersionRepository _versionRepository;

        public GameVersionService(IGameVersionRepository versionRepository)
        {
            _versionRepository = versionRepository;
        }

        public async Task<DefaultResponse> GetAllVersions()
        {
            var versions = await _versionRepository.GetAllVersions();
            return new DefaultResponse(ObjectResponse: versions);

        }

        public async Task<DefaultResponse> GetAllReleasedVersions()
        {
            var versions = await _versionRepository.GetAllReleasedVersions();

            return new DefaultResponse(ObjectResponse: versions, Message: "That are all released versions");
        }

        public async Task<DefaultResponse> NewVersion(GameVersion version)
        {
            bool valid = await VerifyIfVersionExist(version);
            if (valid)
                return new DefaultResponse(HttpStatus: System.Net.HttpStatusCode.Conflict,
                                           Message: "This version already exist.");

            await _versionRepository.SetNewGameVersion(version);

            return new DefaultResponse(Message: "New game version created.");
        }

        public async Task<bool> VerifyIfVersionExist(GameVersion version)
        {
            var versions = await _versionRepository.GetAllVersions();
            if (versions != null && versions.Count > 0)
                return versions.Any(v => v == version);

            return false;
        }

        public async Task<DefaultResponse> GetCurrentVersion()
        {
            var version = await _versionRepository.GetCurrentGameVersion();
            return new DefaultResponse(ObjectResponse: version);
        }
    }
}
