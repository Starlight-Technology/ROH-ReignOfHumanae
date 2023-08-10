// Ignore Spelling: validator

using AutoMapper;

using FluentValidation;

using ROH.Domain.Version;
using ROH.Interfaces.Repository.Version;
using ROH.Interfaces.Services.Version;
using ROH.StandardModels.Response;
using ROH.StandardModels.Version;

using System.IO;
using System.Net;
using System.Text;

namespace ROH.Services.Version
{
    public class GameVersionFileService : IGameVersionFileService
    {
        private readonly IGameVersionFileRepository _repository;
        private readonly IGameVersionService _gameVersion;
        private readonly IValidator<GameVersionFileModel> _validator;
        private readonly IMapper _mapper;

        public GameVersionFileService(IGameVersionFileRepository gameVersionFileRepository, IValidator<GameVersionFileModel> validator, IGameVersionService gameVersion, IMapper mapper)
        {
            _repository = gameVersionFileRepository;
            _validator = validator;
            _gameVersion = gameVersion;
            _mapper = mapper;
        }

        public async Task<DefaultResponse> DownloadFile(long id)
        {
            var file = await _repository.GetFile(id);

            if (file != null)
            {
                file = file with { GameVersion = _mapper.Map<GameVersion>(_gameVersion.GetVersionById(file.IdVersion).Result?.ObjectResponse) };
                var fileModel = _mapper.Map<GameVersionFileModel>(file);
                var validation = await _validator.ValidateAsync(fileModel);
                if (validation.IsValid)
                {
                    if (file.GameVersion != null &&
                        await _gameVersion.VerifyIfVersionExist(_mapper.Map<GameVersionModel>(file.GameVersion)))
                    {
                        string path = string.Empty;
#if DEBUG
                        path = @$"C:\ROHUpdateFiles\{file.GameVersion.Version}.{file.GameVersion.Release}.{file.GameVersion.Review}\"; // path to file
#elif RELEASE
                throw NotImplementedException();
#endif
                        var fileContent = await File.ReadAllLinesAsync(path + file.Name);

                        return new DefaultResponse(fileContent[0], HttpStatusCode.OK);
                    }
                    else
                    {
                        return new DefaultResponse(null, httpStatus: HttpStatusCode.NotFound, message: "Game Version Not Found.");
                    }
                }
                else
                {
                    StringBuilder errors = new();
                    foreach (var error in validation.Errors)
                    {
                        errors.Append($";{error}");
                    }
                    string errorString = errors.ToString();

                    throw new Exception(errorString);
                }
            }
            else
            {
                return new DefaultResponse(null, httpStatus: HttpStatusCode.NotFound, message: "File Not Found.");
            }
        }

        public async Task<DefaultResponse> GetFiles(GameVersionModel version)
        {
            var files = await _repository.GetFiles(_mapper.Map<GameVersion>(version));

            return new DefaultResponse(objectResponse: files);
        }

        public async Task NewFile(GameVersionFileModel file)
        {

            var validation = await _validator.ValidateAsync(file);

            if (validation.IsValid)
            {
                if (file.GameVersion != null &&
                    await _gameVersion.VerifyIfVersionExist(file.GameVersion))
                {
                    string path = "";
#if DEBUG
                    path = @$"C:\ROHUpdateFiles\{file.GameVersion.Version}.{file.GameVersion.Release}.{file.GameVersion.Review}\"; // path to file
#elif RELEASE
                throw NotImplementedException();
#endif
                    file.Path = path;

                    if (!Directory.Exists(Path.GetDirectoryName(path)))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(path) ?? Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\ROHFiles");
                    }

                    if (File.Exists(path + file.Name))
                        File.Delete(path + file.Name);

                    using FileStream fs = File.Create(path + file.Name);

                    byte[] info = new UTF8Encoding(true).GetBytes(file.Content);
                    await fs.WriteAsync(info);

                    var entity = _mapper.Map<GameVersionFile>(file);

                    await _repository.SaveFile(entity);
                }
                else
                {
                    throw new Exception("Game Version Not Found.");
                }
            }
            else
            {
                StringBuilder errors = new();
                foreach (var error in validation.Errors)
                {
                    errors.Append($";{error}");
                }
                string errorString = errors.ToString();

                throw new Exception(errorString);
            }




        }

    }
}
