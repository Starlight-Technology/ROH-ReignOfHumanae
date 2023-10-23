// Ignore Spelling: validator

using AutoMapper;

using FluentValidation;

using ROH.Domain.Version;
using ROH.Interfaces.Repository.Version;
using ROH.Interfaces.Services.Version;
using ROH.StandardModels.Response;
using ROH.StandardModels.Version;

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
            try
            {
                GameVersionFile? file = await _repository.GetFile(id);

                if (file != null)
                {
                    file = file with
                    {
                        GameVersion = _mapper.Map<GameVersion>(_gameVersion.GetVersionById(file.IdVersion).Result
                            ?.ObjectResponse)
                    };
                    GameVersionFileModel fileModel = _mapper.Map<GameVersionFileModel>(file);
                    FluentValidation.Results.ValidationResult validation = await _validator.ValidateAsync(fileModel);
                    if (validation.IsValid)
                    {
                        if (file.GameVersion != null &&
                            await _gameVersion.VerifyIfVersionExist(_mapper.Map<GameVersionModel>(file.GameVersion)))
                        {
                            string path = string.Empty;
#if DEBUG
                            path =
                                @$"C:\ROHUpdateFiles\{file.GameVersion.Version}.{file.GameVersion.Release}.{file.GameVersion.Review}\"; // path to file on Windows
#elif RELEASE
                         path = @$"\app\data\ROH\ROHUpdateFiles\{file.GameVersion.Version}.{file.GameVersion.Release}.{file.GameVersion.Review}\"; //path to file on Linux
#endif
                            string[] fileContent = await File.ReadAllLinesAsync(path + file.Name);

                            return new DefaultResponse(fileContent[0], HttpStatusCode.OK);
                        }
                        else
                        {
                            return new DefaultResponse(null, httpStatus: HttpStatusCode.NotFound,
                                message: "Game Version Not Found.");
                        }
                    }
                    else
                    {
                        StringBuilder errors = new();
                        foreach (FluentValidation.Results.ValidationFailure? error in validation.Errors)
                        {
                            errors.Append($"{error};");
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
            catch (Exception ex)
            {
                return new DefaultResponse(httpStatus: HttpStatusCode.BadRequest, message: ex.Message);
            }
        }

        public async Task<DefaultResponse> GetFiles(GameVersionModel version)
        {
            List<GameVersionFile> files = await _repository.GetFiles(_mapper.Map<GameVersion>(version));

            return new DefaultResponse(objectResponse: files);
        }

        public async Task<DefaultResponse> NewFile(GameVersionFileModel file)
        {
            try
            {
                FluentValidation.Results.ValidationResult validation = await _validator.ValidateAsync(file);

                if (validation.IsValid)
                {
                    var currentVersion = _gameVersion.GetCurrentVersion().Result.ObjectResponse as GameVersion;

                    if (file.GameVersion != null &&
                        await _gameVersion.VerifyIfVersionExist(file.GameVersion) &&
                        !file.GameVersion.Released &&
                        file.GameVersion.VersionDate > currentVersion?.VersionDate)
                    {
                        string path = "";
#if DEBUG
                        path = @$"C:\ROHUpdateFiles\{file.GameVersion.Version}.{file.GameVersion.Release}.{file.GameVersion.Review}\"; // path to file
#elif RELEASE
                        path = @$"\app\data\ROH\ROHUpdateFiles\{file.GameVersion.Version}.{file.GameVersion.Release}.{file.GameVersion.Review}\"; //path to file on Linux
#endif
                        file.Path = path;

                        if (!Directory.Exists(Path.GetDirectoryName(path)))
                        {
                            Directory.CreateDirectory(Path.GetDirectoryName(path) ?? Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\ROHFiles");
                        }

                        if (File.Exists(path + file.Name))
                        {
                            File.Delete(path + file.Name);
                        }

                        using FileStream fs = File.Create(path + file.Name);

                        byte[] info = new UTF8Encoding(true).GetBytes(file.Content);
                        await fs.WriteAsync(info);

                        GameVersionFile entity = _mapper.Map<GameVersionFile>(file);

                        await _repository.SaveFile(entity);

                        return new DefaultResponse(httpStatus: HttpStatusCode.OK);
                    }
                    else
                    {
                        throw new Exception("Game Version Not Found.");
                    }
                }
                else
                {
                    StringBuilder errors = new();
                    foreach (FluentValidation.Results.ValidationFailure? error in validation.Errors)
                    {
                        errors.Append($";{error}");
                    }
                    string errorString = errors.ToString();

                    throw new Exception(errorString);
                }
            }
            catch (Exception ex)
            {
                return new DefaultResponse(httpStatus: HttpStatusCode.BadRequest, message: ex.Message);
            }
        }
    }
}