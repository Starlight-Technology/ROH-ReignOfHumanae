using AutoMapper;

using ROH.Domain.Version;
using ROH.Interfaces.Repository.Version;
using ROH.Interfaces.Services.Version;
using ROH.StandardModels.Paginator;
using ROH.StandardModels.Response;
using ROH.StandardModels.Version;

namespace ROH.Services.Version
{
    public class GameVersionService(IGameVersionRepository versionRepository, IMapper mapper) : IGameVersionService
    {
        public async Task<DefaultResponse> GetVersionByGuid(string versionGuid)
        {
            if (Guid.TryParse(versionGuid, out Guid guid))
            {
                GameVersion? version = await versionRepository.GetVersionByGuid(guid);
                return new DefaultResponse() { ObjectResponse = version };
            }
            return new DefaultResponse() { HttpStatus = System.Net.HttpStatusCode.ExpectationFailed, Message = "The Guid is invalid!" };
        }

        public async Task<DefaultResponse> GetAllVersions(int take = 10, int page = 1)
        {
            int skip = take * (page - 1);

            Domain.Paginator.Paginated result = await versionRepository.GetAllVersions(take, skip);

            IList<GameVersion>? versions = result.ObjectResponse.Cast<GameVersion>().ToList();
            int total = result.Total;
            int pages = 0;
            if (versions.Count > 0)
            {
                pages = (int)Math.Ceiling((double)total / take);
            }

            IList<GameVersionModel> versionModels = mapper.Map<IList<GameVersionModel>>(versions);

            List<object> versionObjects = versionModels.Cast<object>().ToList();

            PaginatedModel paginatedModel = new() { TotalPages = pages, ObjectResponse = versionObjects };

            return new DefaultResponse(objectResponse: paginatedModel);
        }

        public async Task<DefaultResponse> GetAllReleasedVersions(int take = 10, int page = 1)
        {
            int skip = take * (page - 1);

            Domain.Paginator.Paginated result = await versionRepository.GetAllReleasedVersions(take, skip);

            IList<GameVersion>? versions = result.ObjectResponse.Cast<GameVersion>().ToList();
            int total = result.Total;
            int pages = 0;
            if (versions.Count > 0)
            {
                pages = (int)Math.Ceiling((double)total / take);
            }

            IList<GameVersionModel> versionModels = mapper.Map<IList<GameVersionModel>>(versions);

            List<object> versionObjects = versionModels.Cast<object>().ToList();

            PaginatedModel paginatedModel = new() { TotalPages = pages, ObjectResponse = versionObjects };

            return new DefaultResponse(objectResponse: paginatedModel, message: "That are all released versions");
        }

        public async Task<DefaultResponse> NewVersion(GameVersionModel version)
        {
            if (await VerifyIfVersionExist(version))
            {
                return new DefaultResponse(httpStatus: System.Net.HttpStatusCode.Conflict,
                                           message: "This version already exist.");
            }

            _ = await versionRepository.SetNewGameVersion(mapper.Map<GameVersion>(version));

            return new DefaultResponse(httpStatus: System.Net.HttpStatusCode.Created, message: "New game version created.");
        }

        public async Task<bool> VerifyIfVersionExist(GameVersionModel version) => await versionRepository.VerifyIfExist(mapper.Map<GameVersion>(version));

        public async Task<bool> VerifyIfVersionExist(string versionGuid) => Guid.TryParse(versionGuid, out Guid guid) && await versionRepository.VerifyIfExist(guid);

        public async Task<DefaultResponse> GetCurrentVersion()
        {
            GameVersion? version = await versionRepository.GetCurrentGameVersion();
            return new DefaultResponse(objectResponse: version);
        }
    }
}