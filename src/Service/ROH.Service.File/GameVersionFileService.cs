//-----------------------------------------------------------------------
// <copyright file="GameVersionFileService.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using AutoMapper;

using FluentValidation;
using FluentValidation.Results;

using ROH.Context.File.Entities;
using ROH.Context.File.Interface;
using ROH.Service.Exception.Interface;
using ROH.Service.File.Interface;
using ROH.StandardModels.File;
using ROH.StandardModels.Response;
using ROH.StandardModels.Version;
using ROH.Utils.Helpers;

using System.Net;

namespace ROH.Service.File;

public class GameVersionFileService(
    IGameFileService gameFileService,
    IGameVersionService gameVersion,
    IGameVersionFileRepository versionFileRepository,
    IValidator<GameVersionFileModel> validator,
    IMapper mapper,
    IExceptionHandler exceptionHandler) : IGameVersionFileService
{
    private async Task<GameVersionModel?> GetCurrentVersionAsync(CancellationToken cancellationToken = default)
    {
        VersionServiceApi.DefaultResponse? response = await gameVersion.GetCurrentVersionAsync(cancellationToken)
            .ConfigureAwait(true);

        if (response is null)
            return null;

        if (string.IsNullOrWhiteSpace(response.ObjectResponse))
            return null;

        DefaultResponse defaultResponse = new()
        {
            HttpStatus = (HttpStatusCode)response.StatusCode,
            Message = response.Message,
            ObjectResponse = response.ObjectResponse.JsonToObject<dynamic?>()
        };

        GameVersionModel? version = defaultResponse.MapObjectResponse<GameVersionModel>().ObjectResponse as GameVersionModel;
        return version;
    }

    private static string GetFilePath(GameVersionModel gameVersion) =>
#if DEBUG
        @$".\ROHUpdateFiles\{gameVersion.Version}.{gameVersion.Release}.{gameVersion.Review}\";

#else
    @$"/app/ROH/updateFiles/{gameVersion.Version}.{gameVersion.Release}.{gameVersion.Review}/";
#endif

    private static string GetRejectionMessage(GameVersionModel gameVersion) => gameVersion.Released
        ? "File Upload Failed: This version has already been released. You cannot upload new files for a released version."
        : "File Upload Failed: This version has already been released with a yearly schedule. Uploading new files is not allowed for past versions.";

    private async Task<DefaultResponse> SaveFileAsync(
        GameVersionFileModel fileModel,
        GameVersionFile versionFile,
        GameVersionModel? currentVersion,
        CancellationToken cancellationToken)
    {
        if (await ShouldRejectFileUploadAsync(fileModel!.GameVersion!, currentVersion, cancellationToken)
            .ConfigureAwait(true))
            return new DefaultResponse(null, HttpStatusCode.BadRequest, GetRejectionMessage(fileModel!.GameVersion!));

        GameFile file = mapper.Map<GameFile>(fileModel);
        string path = GetFilePath(fileModel.GameVersion!);
        file = file with { Path = path };

        await gameFileService.SaveFileAsync(file, fileModel.Content!, cancellationToken).ConfigureAwait(true);

        versionFile = versionFile with { GameFile = file };
        await versionFileRepository.SaveFileAsync(versionFile, cancellationToken).ConfigureAwait(true);

        return new DefaultResponse(HttpStatusCode.OK);
    }

    private static Task<bool> ShouldRejectFileUploadAsync(
        GameVersionModel gameVersion,
        GameVersionModel? currentVersion,
        CancellationToken cancellationToken = default)
    {
        // Task was cancelled, throw OperationCanceledException to respect the cancellation request
        cancellationToken.ThrowIfCancellationRequested();

        // Perform the original logic
        return Task.FromResult(gameVersion.Released || (gameVersion.VersionDate < currentVersion?.VersionDate));
    }

    private Task<ValidationResult> ValidateFileAsync(GameVersionFileModel file, CancellationToken cancellationToken = default) => validator.ValidateAsync(
        file,
        cancellationToken);

    public async Task<DefaultResponse> DownloadFileAsync(Guid fileGuid, CancellationToken cancellationToken = default)
    {
        try
        {
            GameVersionFile? versionFile = await versionFileRepository.GetFileAsync(fileGuid, cancellationToken)
                .ConfigureAwait(true);

            return (versionFile?.GameFile != null)
                ? (await gameFileService.DownloadFileAsync(versionFile.GameFile.Guid, cancellationToken)
                    .ConfigureAwait(true))
                : (new DefaultResponse(
                    null,
                    httpStatus: HttpStatusCode.NotFound,
                    message: "Game Version File Not Found."));
        }
        catch (System.Exception ex)
        {
            return exceptionHandler.HandleException(ex);
        }
    }

    public async Task<DefaultResponse> DownloadFileAsync(long id, CancellationToken cancellationToken = default)
    {
        try
        {
            GameVersionFile? versionFile = await versionFileRepository.GetFileAsync(id, cancellationToken)
                .ConfigureAwait(true);

            return (versionFile?.GameFile != null)
                ? (await gameFileService.DownloadFileAsync(versionFile.GameFile.Id, cancellationToken)
                    .ConfigureAwait(true))
                : (new DefaultResponse(
                    null,
                    httpStatus: HttpStatusCode.NotFound,
                    message: "Game Version File Not Found."));
        }
        catch (System.Exception ex)
        {
            return exceptionHandler.HandleException(ex);
        }
    }

    public async Task<DefaultResponse> GetFilesAsync(string versionGuid, CancellationToken cancellationToken = default)
    {
        try
        {
            if (Guid.TryParse(versionGuid, out Guid guid))
            {
                bool response = await gameVersion.VerifyIfVersionExistAsync(guid, cancellationToken)
                    .ConfigureAwait(true);

                if (response)
                {
                    List<GameVersionFile> files = await versionFileRepository.GetFilesAsync(guid, cancellationToken)
                        .ConfigureAwait(true);

                    List<GameVersionFileModel> filesModels = [];

                    foreach (GameVersionFile item in files)
                    {
                        DefaultResponse result = await DownloadFileAsync(item.IdGameFile, cancellationToken)
                            .ConfigureAwait(true);

                        if (result.ObjectResponse is not GameFileModel gameFileModel)
                            continue;

                        filesModels.Add(
                            new GameVersionFileModel
                            {
                                Guid = item.Guid,
                                Name = gameFileModel.Name,
                                Size = gameFileModel.Size,
                                Active = gameFileModel.Active
                            });
                    }
                    return new DefaultResponse(objectResponse: filesModels);
                }
            }

            return new DefaultResponse(httpStatus: HttpStatusCode.NotFound);
        }
        catch (System.Exception ex)
        {
            return exceptionHandler.HandleException(ex);
        }
    }

    public async Task<DefaultResponse> NewFileAsync(
        GameVersionFileModel fileModel,
        CancellationToken cancellationToken = default)
    {
        try
        {
            if (fileModel.Content is null)
                return new DefaultResponse(null, HttpStatusCode.BadRequest, "File content can't be empty.");

            ValidationResult validation = await ValidateFileAsync(fileModel, cancellationToken).ConfigureAwait(true);
            if ((validation is not null) && !validation.IsValid && (validation.Errors.Count > 0))
            {
                string errorMessages = string.Join("; ", validation.Errors.Select(e => e.ErrorMessage));
                return new DefaultResponse(null, HttpStatusCode.BadRequest, errorMessages);
            }

            GameVersionFile versionFile = mapper.Map<GameVersionFile>(fileModel);
            GameVersionModel? currentVersion = await GetCurrentVersionAsync(cancellationToken).ConfigureAwait(true);

            return ((versionFile is not null) &&
                    (await gameVersion.VerifyIfVersionExistAsync(fileModel.GameVersion!.Guid, cancellationToken)
                        .ConfigureAwait(true)))
                ? (await SaveFileAsync(fileModel, versionFile, currentVersion, cancellationToken).ConfigureAwait(true))
                : (new DefaultResponse(null, HttpStatusCode.BadRequest, "Game Version Not Found."));
        }
        catch (System.Exception ex)
        {
            return exceptionHandler.HandleException(ex);
        }
    }
}