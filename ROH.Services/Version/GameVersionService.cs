using AutoMapper;

using ROH.Domain.Version;
using ROH.Interfaces.Repository.Version;
using ROH.Interfaces.Services.Version;
using ROH.StandardModels.Paginator;
using ROH.StandardModels.Response;
using ROH.StandardModels.Version;

using System;

namespace ROH.Services.Version
{
    public class GameVersionService : IGameVersionService
    {
        private readonly IGameVersionRepository _versionRepository;
        private readonly IMapper _mapper;

        public GameVersionService(IGameVersionRepository versionRepository, IMapper mapper)
        {
            _versionRepository = versionRepository;
            _mapper = mapper;
        }

        public async Task<DefaultResponse?> GetVersionById(long idVersion)
        {
            GameVersion? version = await _versionRepository.GetVersionById(idVersion);
            GameVersionModel model = _mapper.Map<GameVersionModel>(version);
            return new DefaultResponse() { ObjectResponse = model };
        }

        public async Task<DefaultResponse> GetAllVersions(int take = 10, int page = 1)
        {
            int skip = take * (page - 1);

            var result = await _versionRepository.GetAllVersions(take, skip);

            IList<GameVersion>? versions = result.ObjectResponse.Cast<GameVersion>().ToList();
            int total = result.Total;
            int pages = 0;
            if (versions.Count > 0)
                pages = (int)Math.Ceiling((double)total / take);


            IList<GameVersionModel> versionModels = _mapper.Map<IList<GameVersionModel>>(versions);

            List<object> versionObjects = versionModels.Cast<object>().ToList();

            PaginatedModel paginatedModel = new() { TotalPages = pages, ObjectResponse = versionObjects };

            return new DefaultResponse(objectResponse: paginatedModel);
        }

        public async Task<DefaultResponse> GetAllReleasedVersions(int take = 10, int page = 1)
        {
            int skip = take * (page - 1);

            var result = await _versionRepository.GetAllReleasedVersions(take, skip);

            IList<GameVersion>? versions = result.ObjectResponse.Cast<GameVersion>().ToList();
            int total = result.Total;
            int pages = 0;
            if (versions.Count > 0)
                pages = (int)Math.Ceiling((double)total / take);


            IList<GameVersionModel> versionModels = _mapper.Map<IList<GameVersionModel>>(versions);

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

            _ = await _versionRepository.SetNewGameVersion(_mapper.Map<GameVersion>(version));

            return new DefaultResponse(httpStatus: System.Net.HttpStatusCode.Created, message: "New game version created.");
        }

        public async Task<bool> VerifyIfVersionExist(GameVersionModel version)
        {
            return await _versionRepository.VerifyIfExist(_mapper.Map<GameVersion>(version));
        }

        public async Task<DefaultResponse> GetCurrentVersion()
        {
            GameVersion? version = await _versionRepository.GetCurrentGameVersion();
            return new DefaultResponse(objectResponse: _mapper.Map<GameVersionModel>(version));
        }
    }
}