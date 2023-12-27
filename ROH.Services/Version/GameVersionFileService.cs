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
    public class GameVersionFileService(IGameVersionFileRepository gameVersionFileRepository, IValidator<GameVersionFileModel> validator, IGameVersionService gameVersion, IMapper mapper) : IGameVersionFileService
    {
        public async Task<DefaultResponse> DownloadFile(long id)
        {
            try
            {
                GameVersionFile? file = await gameVersionFileRepository.GetFile(id);

                if (file != null)
                {
                    file = file with
                    {
                        GameVersion = mapper.Map<GameVersion>(gameVersion.GetVersionByGuid(file.Guid.ToString()).Result
                            ?.ObjectResponse)
                    };
                    GameVersionFileModel fileModel = mapper.Map<GameVersionFileModel>(file);
                    FluentValidation.Results.ValidationResult validation = await validator.ValidateAsync(fileModel);
                    if (validation.IsValid)
                    {
                        if (file.GameVersion != null &&
                            await gameVersion.VerifyIfVersionExist(mapper.Map<GameVersionModel>(file.GameVersion)))
                        {
                            string path = string.Empty;
#if DEBUG
                            path =
                                @$"C:\ROHUpdateFiles\{file.GameVersion.Version}.{file.GameVersion.Release}.{file.GameVersion.Review}\"; // path to file on Windows
#else
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

        public async Task<DefaultResponse> GetFiles(string versionGuid)
        {
            bool response = await gameVersion.VerifyIfVersionExist(versionGuid);

            if (response && Guid.TryParse(versionGuid, out Guid guid))
            {
                List<GameVersionFileModel> files = mapper.Map<List<GameVersionFileModel>>(await gameVersionFileRepository.GetFiles(guid));

                return new DefaultResponse(objectResponse: files);
            }

            return new DefaultResponse(httpStatus: HttpStatusCode.NotFound);
        }

        public async Task<DefaultResponse> NewFile(GameVersionFileModel fileModel)
        {
            try
            {
                FluentValidation.Results.ValidationResult validation = await ValidateFileAsync(fileModel);
                if (validation != null && !validation.IsValid && validation.Errors.Count > 0)
                {
                    return new DefaultResponse(null, HttpStatusCode.BadRequest, validation.Errors.ToString()!);
                }

                GameVersionFile file = mapper.Map<GameVersionFile>(fileModel);
                GameVersion? currentVersion = await GetCurrentVersionAsync();

                if (file.GameVersion != null && await gameVersion.VerifyIfVersionExist(fileModel.GameVersion!))
                {
                    if (await ShouldRejectFileUpload(file.GameVersion, currentVersion))
                    {
                        return new DefaultResponse(null, HttpStatusCode.BadRequest, GetRejectionMessage(file.GameVersion));
                    }

                    string path = GetFilePath(file.GameVersion);
                    await EnsureDirectoryExists(path);

                    await SaveFileAsync(path, fileModel);

                    return new DefaultResponse(HttpStatusCode.OK);
                }
                else
                {
                    throw new Exception("Game Version Not Found.");
                }
            }
            catch (Exception ex)
            {
                return new DefaultResponse(null, HttpStatusCode.BadRequest, ex.Message);
            }
        }

        private async Task<FluentValidation.Results.ValidationResult> ValidateFileAsync(GameVersionFileModel file)
        {
            return await validator.ValidateAsync(file);
        }

        private async Task<GameVersion?> GetCurrentVersionAsync()
        {
            return (await gameVersion.GetCurrentVersion()).ObjectResponse as GameVersion;
        }

        private static Task<bool> ShouldRejectFileUpload(GameVersion gameVersion, GameVersion? currentVersion)
        {
            return Task.FromResult(gameVersion.Released || (gameVersion.VersionDate < currentVersion?.VersionDate));
        }

        private static string GetRejectionMessage(GameVersion gameVersion)
        {
            return gameVersion.Released
                ? "File Upload Failed: This version has already been released. You cannot upload new files for a released version."
                : "File Upload Failed: This version has already been released with a yearly schedule. Uploading new files is not allowed for past versions.";
        }

        private static string GetFilePath(GameVersion gameVersion)
        {
#if DEBUG
            return @$"C:\ROHUpdateFiles\{gameVersion.Version}.{gameVersion.Release}.{gameVersion.Review}\";
#else
    return @$"\app\data\ROH\ROHUpdateFiles\{gameVersion.Version}.{gameVersion.Release}.{gameVersion.Review}\";
#endif
        }

        private static Task EnsureDirectoryExists(string path)
        {
            if (!Directory.Exists(Path.GetDirectoryName(path)))
            {
                _ = Directory.CreateDirectory(Path.GetDirectoryName(path) ?? Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\ROHFiles");
            }

            return Task.CompletedTask;
        }

        private async Task SaveFileAsync(string path, GameVersionFileModel file)
        {
            string filePath = Path.Combine(path, file.Name);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            using (FileStream fs = File.Create(filePath))
            {
                byte[] info = new UTF8Encoding(true).GetBytes(file.Content);
                await fs.WriteAsync(info);
            }

            GameVersionFile entity = mapper.Map<GameVersionFile>(file);

            await gameVersionFileRepository.SaveFile(entity);
        }
    }
}