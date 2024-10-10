//-----------------------------------------------------------------------
// <copyright file="GameVersionFileService.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using AutoMapper;

using FluentValidation;
using FluentValidation.Results;

using ROH.Domain.Version;
using ROH.Interfaces.Repository.Version;
using ROH.Interfaces.Services.ExceptionService;
using ROH.Interfaces.Services.GameFile;
using ROH.Interfaces.Services.Version;
using ROH.StandardModels.File;
using ROH.StandardModels.Response;
using ROH.StandardModels.Version;
using ROH.Utils.Helpers;

using System.Net;

namespace ROH.Services.Version;

public class GameVersionFileService(
    IGameVersionFileRepository gameVersionFileRepository,
    IGameFileService gameFileService,
    IValidator<GameVersionFileModel> validator,
    IGameVersionService gameVersion,
    IMapper mapper,
    IExceptionHandler exceptionHandler) : IGameVersionFileService
{
    private async Task<GameVersion?> GetCurrentVersionAsync()
    {
        DefaultResponse? response = await gameVersion.GetCurrentVersion();

        if (response is null)
            return null;

        GameVersion? version = response.MapObjectResponse<GameVersion>().ObjectResponse as GameVersion;
        return version;
    }

    private static string GetFilePath(GameVersion gameVersion) =>
#if DEBUG
        @$".\ROHUpdateFiles\{gameVersion.Version}.{gameVersion.Release}.{gameVersion.Review}\";

#else
    @$"/app/ROH/updateFiles/{gameVersion.Version}.{gameVersion.Release}.{gameVersion.Review}/";
#endif

    private static string GetRejectionMessage(GameVersion gameVersion) => gameVersion.Released
        ? "File Upload Failed: This version has already been released. You cannot upload new files for a released version."
        : "File Upload Failed: This version has already been released with a yearly schedule. Uploading new files is not allowed for past versions.";

    private static Task<bool> ShouldRejectFileUpload(GameVersion gameVersion, GameVersion? currentVersion) => Task.FromResult(
        gameVersion.Released || (gameVersion.VersionDate < currentVersion?.VersionDate));

    private async Task<ValidationResult> ValidateFileAsync(GameVersionFileModel file) => await validator.ValidateAsync(file);

    public async Task<DefaultResponse> DownloadFile(Guid fileGuid)
    {
        try
        {
            GameVersionFile? versionFile = await gameVersionFileRepository.GetFile(fileGuid);

            return (versionFile?.GameFile != null)
                ? (await gameFileService.DownloadFile(versionFile.GameFile.Guid))
                : new DefaultResponse(
                    null,
                    httpStatus: HttpStatusCode.NotFound,
                    message: "Game Version File Not Found.");
        }
        catch (Exception ex)
        {
            return exceptionHandler.HandleException(ex);
        }
    }

    public async Task<DefaultResponse> DownloadFile(long id)
    {
        try
        {
            GameVersionFile? versionFile = await gameVersionFileRepository.GetFile(id);

            return (versionFile?.GameFile != null)
                ? (await gameFileService.DownloadFile(versionFile.GameFile.Id))
                : new DefaultResponse(
                    null,
                    httpStatus: HttpStatusCode.NotFound,
                    message: "Game Version File Not Found.");
        }
        catch (Exception ex)
        {
            return exceptionHandler.HandleException(ex);
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

                List<GameVersionFileModel> filesModels = [];

                foreach (GameVersionFile item in files)
                {
                    DefaultResponse result = await DownloadFile(item.IdGameFile);

                    if (result.ObjectResponse is not GameFileModel gameFileModel)
                        continue;

                    filesModels.Add(
                        new GameVersionFileModel()
                        {
                            Guid = item.Guid,
                            Name = gameFileModel.Name,
                            Size = gameFileModel.Size,
                            Active = gameFileModel.Active
                        });
                }
                return new DefaultResponse(objectResponse: filesModels);
            }

            return new DefaultResponse(httpStatus: HttpStatusCode.NotFound);
        }
        catch (Exception ex)
        {
            return exceptionHandler.HandleException(ex);
        }
    }

    public async Task<DefaultResponse> NewFile(GameVersionFileModel fileModel)
    {
        try
        {
            if (fileModel.Content is null)
                return new DefaultResponse(null, HttpStatusCode.BadRequest, "File content can't be empty.");

            ValidationResult validation = await ValidateFileAsync(fileModel);
            if ((validation != null) && !validation.IsValid && (validation.Errors.Count > 0))
                return new DefaultResponse(null, HttpStatusCode.BadRequest, validation.Errors.ToString()!);

            GameVersionFile versionFile = mapper.Map<GameVersionFile>(fileModel);
            GameVersion? currentVersion = await GetCurrentVersionAsync();

            if ((versionFile.GameVersion != null) && (await gameVersion.VerifyIfVersionExist(fileModel.GameVersion!)))
            {
                if (await ShouldRejectFileUpload(versionFile.GameVersion, currentVersion))
                    return new DefaultResponse(
                        null,
                        HttpStatusCode.BadRequest,
                        GetRejectionMessage(versionFile.GameVersion));

                Domain.GameFiles.GameFile file = mapper.Map<Domain.GameFiles.GameFile>(fileModel);
                string path = GetFilePath(versionFile.GameVersion);
                file = file with { Path = path };

                await gameFileService.SaveFileAsync(file, fileModel.Content);

                versionFile = versionFile with { GameFile = file };
                await gameVersionFileRepository.SaveFile(versionFile);

                return new DefaultResponse(HttpStatusCode.OK);
            }
            else
            {
                return new DefaultResponse(null, HttpStatusCode.BadRequest, "Game Version Not Found.");
            }
        }
        catch (Exception ex)
        {
            return exceptionHandler.HandleException(ex);
        }
    }
}
