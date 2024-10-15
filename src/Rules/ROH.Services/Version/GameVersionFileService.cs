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
    private async Task<GameVersion?> GetCurrentVersionAsync(CancellationToken cancellationToken = default)
    {
        DefaultResponse? response = await gameVersion.GetCurrentVersionAsync(cancellationToken).ConfigureAwait(true);

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

    private static Task<bool> ShouldRejectFileUploadAsync(
        GameVersion gameVersion,
        GameVersion? currentVersion,
        CancellationToken cancellationToken = default)
    {
        // Task was cancelled, throw OperationCanceledException to respect the cancellation request
        cancellationToken.ThrowIfCancellationRequested();

        // Perform the original logic
        return Task.FromResult(gameVersion.Released || (gameVersion.VersionDate < currentVersion?.VersionDate));
    }

    private Task<ValidationResult> ValidateFileAsync(GameVersionFileModel file, CancellationToken cancellationToken = default)
        => validator.ValidateAsync(file, cancellationToken);

    public async Task<DefaultResponse> DownloadFileAsync(Guid fileGuid, CancellationToken cancellationToken = default)
    {
        try
        {
            GameVersionFile? versionFile = await gameVersionFileRepository.GetFileAsync(fileGuid, cancellationToken).ConfigureAwait(true);

            return (versionFile?.GameFile != null)
                ? (await gameFileService.DownloadFileAsync(versionFile.GameFile.Guid, cancellationToken).ConfigureAwait(true))
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

    public async Task<DefaultResponse> DownloadFileAsync(long id, CancellationToken cancellationToken = default)
    {
        try
        {
            GameVersionFile? versionFile = await gameVersionFileRepository.GetFileAsync(id, cancellationToken).ConfigureAwait(true);

            return (versionFile?.GameFile != null)
                ? (await gameFileService.DownloadFileAsync(versionFile.GameFile.Id, cancellationToken).ConfigureAwait(true))
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

    public async Task<DefaultResponse> GetFilesAsync(string versionGuid, CancellationToken cancellationToken = default)
    {
        try
        {
            bool response = await gameVersion.VerifyIfVersionExistAsync(versionGuid, cancellationToken).ConfigureAwait(true);

            if (response && Guid.TryParse(versionGuid, out Guid guid))
            {
                List<GameVersionFile> files = await gameVersionFileRepository.GetFilesAsync(guid, cancellationToken).ConfigureAwait(true);

                List<GameVersionFileModel> filesModels = [];

                foreach (GameVersionFile item in files)
                {
                    DefaultResponse result = await DownloadFileAsync(item.IdGameFile, cancellationToken).ConfigureAwait(true);

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

    public async Task<DefaultResponse> NewFileAsync(GameVersionFileModel fileModel, CancellationToken cancellationToken = default)
    {
        try
        {
            if (fileModel.Content is null)
                return new DefaultResponse(null, HttpStatusCode.BadRequest, "File content can't be empty.");

            ValidationResult validation = await ValidateFileAsync(fileModel, cancellationToken).ConfigureAwait(true);
            if ((validation is not null) && !validation.IsValid && (validation.Errors.Count > 0))
                return new DefaultResponse(null, HttpStatusCode.BadRequest, validation.Errors.ToString()!);

            GameVersionFile versionFile = mapper.Map<GameVersionFile>(fileModel);
            GameVersion? currentVersion = await GetCurrentVersionAsync(cancellationToken).ConfigureAwait(true);

            if ((versionFile.GameVersion is not null) && (await gameVersion.VerifyIfVersionExistAsync(fileModel.GameVersion!, cancellationToken).ConfigureAwait(true)))
            {
                return await SaveFileAsync(gameVersionFileRepository, gameFileService, mapper, fileModel, versionFile, currentVersion, cancellationToken).ConfigureAwait(true);
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

    private static async Task<DefaultResponse> SaveFileAsync(IGameVersionFileRepository gameVersionFileRepository, IGameFileService gameFileService, IMapper mapper, GameVersionFileModel fileModel, GameVersionFile versionFile, GameVersion? currentVersion, CancellationToken cancellationToken)
    {
        if (await ShouldRejectFileUploadAsync(versionFile!.GameVersion!, currentVersion, cancellationToken).ConfigureAwait(true))
            return new DefaultResponse(
                null,
                HttpStatusCode.BadRequest,
                GetRejectionMessage(versionFile.GameVersion!));

        Domain.GameFiles.GameFile file = mapper.Map<Domain.GameFiles.GameFile>(fileModel);
        string path = GetFilePath(versionFile.GameVersion!);
        file = file with { Path = path };

        await gameFileService.SaveFileAsync(file, fileModel.Content!, cancellationToken).ConfigureAwait(true);

        versionFile = versionFile with { GameFile = file };
        await gameVersionFileRepository.SaveFileAsync(versionFile, cancellationToken).ConfigureAwait(true);

        return new DefaultResponse(HttpStatusCode.OK);
    }
}
