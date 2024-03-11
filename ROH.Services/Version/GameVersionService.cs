using AutoMapper;

using ROH.Domain.Version;
using ROH.Interfaces.Repository.Version;
using ROH.Interfaces.Services.ExceptionService;
using ROH.Interfaces.Services.Version;
using ROH.StandardModels.Paginator;
using ROH.StandardModels.Response;
using ROH.StandardModels.Version;
using ROH.Utils.Helpers;

namespace ROH.Services.Version
{
    public class GameVersionService(IGameVersionRepository versionRepository, IMapper mapper, IExceptionHandler exceptionHandler) : IGameVersionService
    {
        public async Task<DefaultResponse> GetVersionByGuid(string versionGuid)
        {
            try
            {
                if (Guid.TryParse(versionGuid, out Guid guid))
                {
                    GameVersion? version = await versionRepository.GetVersionByGuid(guid);

                    if (version != null)
                        return new DefaultResponse() { ObjectResponse = version };
                }

                return await ReturnGuidInvalid();
            }
            catch (Exception ex)
            {
                return exceptionHandler.HandleException(ex);
            }
        }

        public async Task<DefaultResponse> GetAllVersions(int take = 10, int page = 1)
        {
            try
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
            catch (Exception ex)
            {
                return exceptionHandler.HandleException(ex);
            }
        }

        public async Task<DefaultResponse> GetAllReleasedVersions(int take = 10, int page = 1)
        {
            try
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
            catch (Exception ex)
            {
                return exceptionHandler.HandleException(ex);
            }
        }

        public async Task<DefaultResponse> NewVersion(GameVersionModel version)
        {
            try
            {
                if (await VerifyIfVersionExist(version))
                {
                    return new DefaultResponse(httpStatus: System.Net.HttpStatusCode.Conflict,
                                               message: "This version already exist.");
                }

                _ = await versionRepository.SetNewGameVersion(mapper.Map<GameVersion>(version));

                return new DefaultResponse(httpStatus: System.Net.HttpStatusCode.Created, message: "New game version created.");
            }
            catch (Exception ex)
            {
                return exceptionHandler.HandleException(ex);
            }
        }

        public async Task<bool> VerifyIfVersionExist(GameVersionModel version) => await versionRepository.VerifyIfExist(mapper.Map<GameVersion>(version));

        public async Task<bool> VerifyIfVersionExist(string versionGuid) => Guid.TryParse(versionGuid, out Guid guid) && await versionRepository.VerifyIfExist(guid);

        public async Task<DefaultResponse> GetCurrentVersion()
        {
            try
            {
                GameVersion? version = await versionRepository.GetCurrentGameVersion();
                return new DefaultResponse(objectResponse: version);
            }
            catch (Exception ex)
            {
                return exceptionHandler.HandleException(ex);
            }
        }

        public async Task<DefaultResponse> SetReleased(string versionGuid)
        {
            try
            {
                var versionResponse = await GetVersionByGuid(versionGuid);

                return versionResponse.HttpStatus.IsSuccessStatusCode() ? await ReleaseVersion(versionResponse) : await ReturnGuidInvalid();

            }
            catch (Exception ex)
            {
                return exceptionHandler.HandleException(ex);
            }
        }

        private async Task<DefaultResponse> ReleaseVersion(DefaultResponse defaultResponse)
        {
            if(defaultResponse.ObjectResponse is null)
                return new DefaultResponse(httpStatus:System.Net.HttpStatusCode.NotFound, message:"The version has not found!");

            GameVersion version = (GameVersion)defaultResponse.ObjectResponse with { Released = true, ReleaseDate = DateTime.Now };

            await versionRepository.UpdateGameVersion(version);

            return new DefaultResponse(message:"The version has been set as release.");
        }
        private static Task<DefaultResponse> ReturnGuidInvalid() => Task.FromResult(new DefaultResponse { HttpStatus = System.Net.HttpStatusCode.ExpectationFailed, Message = "The Guid is invalid!" });
    }
}