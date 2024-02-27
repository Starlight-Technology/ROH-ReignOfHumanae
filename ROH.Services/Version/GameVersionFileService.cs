// Ignore Spelling: validator

using AutoMapper;

using FluentValidation;

using ROH.Domain.Version;
using ROH.Interfaces.Repository.Version;
using ROH.Interfaces.Services.Version;
using ROH.StandardModels.Response;
using ROH.StandardModels.Version;

using System.Net;

namespace ROH.Services.Version
{
    public class GameVersionFileService(IGameVersionFileRepository gameVersionFileRepository, IValidator<GameVersionFileModel> validator, IGameVersionService gameVersion, IMapper mapper) : IGameVersionFileService
    {
        public async Task<DefaultResponse> DownloadFile(Guid fileGuid)
        {
            try
            {
                GameVersionFile? file = await gameVersionFileRepository.GetFile(fileGuid);

                return file is null ? new DefaultResponse(null, httpStatus: HttpStatusCode.NotFound, message: "Game Version Not Found.") : await GetFile(file);
            }
            catch (Exception e)
            {
                return new DefaultResponse(null, httpStatus: HttpStatusCode.BadRequest, message: e.Message);
            }
        }

        public async Task<DefaultResponse> DownloadFile(long id)
        {
            try
            {
                GameVersionFile? file = await gameVersionFileRepository.GetFile(id);

                return file is null ? new DefaultResponse(null, httpStatus: HttpStatusCode.NotFound, message: "Game Version Not Found.") : await GetFile(file);
            }
            catch (Exception e)
            {
                return new DefaultResponse(null, httpStatus: HttpStatusCode.BadRequest, message: e.Message);
            }
        }

        private async Task<DefaultResponse> GetFile(GameVersionFile file)
        {
            try
            {
                if (file != null)
                {
                    file = file with
                    {
                        GameVersion = mapper.Map<GameVersion>(gameVersion.GetVersionByGuid(file.Guid.ToString()).Result?.ObjectResponse)
                    };

                    string filePath = Path.Combine(file.Path, file.Name);

                    if (File.Exists(filePath))
                    {
                        byte[] fileContent = await File.ReadAllBytesAsync(filePath);

                        return new DefaultResponse(new GameVersionFileModel(
                            name: file.Name,
                            format: file.Format,
                            content: fileContent).ToFileModel(), HttpStatusCode.OK);
                    }
                    else
                    {
                        return new DefaultResponse(null, httpStatus: HttpStatusCode.NotFound, message: "File Not Found.");
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
            try
            {
                bool response = await gameVersion.VerifyIfVersionExist(versionGuid);

                if (response && Guid.TryParse(versionGuid, out Guid guid))
                {
                    List<GameVersionFile> files = await gameVersionFileRepository.GetFiles(guid);

                    foreach (GameVersionFile item in files)
                    {
                        item.GameVersion = null;
                    }
                    return new DefaultResponse(objectResponse: files);
                }

                return new DefaultResponse(httpStatus: HttpStatusCode.NotFound);
            }
            catch (Exception e)
            {
                return new DefaultResponse(null, httpStatus: HttpStatusCode.BadRequest, message: e.Message);
            }
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
                    return new DefaultResponse(null, HttpStatusCode.BadRequest, "Game Version Not Found.");
                }
            }
            catch (Exception ex)
            {
                return new DefaultResponse(null, HttpStatusCode.BadRequest, ex.Message);
            }
        }

        private async Task<FluentValidation.Results.ValidationResult> ValidateFileAsync(GameVersionFileModel file) => await validator.ValidateAsync(file);

        private async Task<GameVersion?> GetCurrentVersionAsync() => (await gameVersion.GetCurrentVersion()).ObjectResponse as GameVersion;

        private static Task<bool> ShouldRejectFileUpload(GameVersion gameVersion, GameVersion? currentVersion) => Task.FromResult(gameVersion.Released || (gameVersion.VersionDate < currentVersion?.VersionDate));

        private static string GetRejectionMessage(GameVersion gameVersion) => gameVersion.Released
                ? "File Upload Failed: This version has already been released. You cannot upload new files for a released version."
                : "File Upload Failed: This version has already been released with a yearly schedule. Uploading new files is not allowed for past versions.";

        private static string GetFilePath(GameVersion gameVersion) =>
            
#if DEBUG
            @$".\ROHUpdateFiles\{gameVersion.Version}.{gameVersion.Release}.{gameVersion.Review}\";
#else
            @$"/app/ROH/updateFiles/{gameVersion.Version}.{gameVersion.Release}.{gameVersion.Review}/";           
#endif

        private static Task EnsureDirectoryExists(string path)
        {
            if (!Directory.Exists(Path.GetDirectoryName(path)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path) ?? Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\ROHFiles");
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
                await fs.WriteAsync(file.Content.AsMemory(), CancellationToken.None);
            }

            GameVersionFile entity = mapper.Map<GameVersionFile>(file);
            entity = entity with { Path = path };

            await gameVersionFileRepository.SaveFile(entity);
        }
    }
}