//-----------------------------------------------------------------------
// <copyright file="GameVersionService.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using AutoMapper;

using ROH.Context.Version.Entities;
using ROH.Context.Version.Interface;
using ROH.Context.Version.Paginator;
using ROH.Service.Exception.Interface;
using ROH.Service.Version.Interface;
using ROH.StandardModels.Paginator;
using ROH.StandardModels.Response;
using ROH.StandardModels.Version;
using ROH.Utils.Helpers;

using System.Net;

namespace ROH.Service.Version;

public class GameVersionService(
    IGameVersionRepository versionRepository,
    IMapper mapper,
    IExceptionHandler exceptionHandler) : IGameVersionService
{
    private async Task<DefaultResponse> ReleaseVersionAsync(DefaultResponse defaultResponse, CancellationToken cancellationToken = default)
    {
        try
        {
            if (defaultResponse.ObjectResponse is null)
                return new DefaultResponse(httpStatus: HttpStatusCode.NotFound, message: "The version has not found!");

            GameVersion? existingVersion = await versionRepository.GetVersionByGuidAsync(
                ((GameVersion)defaultResponse.ObjectResponse).Guid, cancellationToken).ConfigureAwait(true);

            existingVersion!.Released = true;
            existingVersion!.ReleaseDate = DateTime.UtcNow;

            _ = await versionRepository.UpdateGameVersionAsync(existingVersion, cancellationToken).ConfigureAwait(true);

            return new DefaultResponse(message: "The version has been set as release.");
        }
        catch (System.Exception ex)
        {
            return exceptionHandler.HandleException(ex);
        }
    }

    private static Task<DefaultResponse> ReturnGuidInvalidAsync(CancellationToken cancellationToken = default)
    {
        return cancellationToken.IsCancellationRequested
            ? Task.FromCanceled<DefaultResponse>(cancellationToken)
            : Task.FromResult(
                new DefaultResponse { HttpStatus = HttpStatusCode.ExpectationFailed, Message = "The Guid is invalid!" });
    }

    public async Task<DefaultResponse> GetAllReleasedVersionsAsync(int take = 10, int page = 1, CancellationToken cancellationToken = default)
    {
        try
        {
            int skip = Math.Max(0, take * (page - 1));

            take = Math.Max(1, take);

            Paginated result = await versionRepository.GetAllReleasedVersionsAsync(take, skip, cancellationToken).ConfigureAwait(true);

            List<GameVersion>? versions = result.ObjectResponse.Cast<GameVersion>().ToList();
            int total = result.Total;
            int pages = 0;
            if (versions.Count > 0)
                pages = (int)Math.Ceiling((double)total / take);

            List<GameVersionModel> versionModels = mapper.Map<List<GameVersionModel>>(versions);

            List<object> versionObjects = versionModels.Cast<object>().ToList();

            PaginatedModel paginatedModel = new() { TotalPages = pages, ObjectResponse = versionObjects };

            return new DefaultResponse(objectResponse: paginatedModel, message: "That are all released versions");
        }
        catch (System.Exception ex)
        {
            return exceptionHandler.HandleException(ex);
        }
    }

    public async Task<DefaultResponse> GetAllVersionsAsync(int take = 10, int page = 1, CancellationToken cancellationToken = default)
    {
        try
        {
            int skip = Math.Max(0, take * (page - 1));

            take = Math.Max(1, take);

            Paginated result = await versionRepository.GetAllVersionsAsync(take, skip, cancellationToken).ConfigureAwait(true);

            int total = result.Total;
            int pages = 0;
            List<GameVersion>? versions = result.ObjectResponse.Cast<GameVersion>().ToList();
            if (versions.Count > 0)
                pages = (int)Math.Ceiling((double)total / take);

            List<GameVersionModel> versionModels = mapper.Map<List<GameVersionModel>>(versions);
            List<object> versionObjects = versionModels.Cast<object>().ToList();

            PaginatedModel paginatedModel = new() { TotalPages = pages, ObjectResponse = versionObjects };

            return new DefaultResponse(objectResponse: paginatedModel);
        }
        catch (System.Exception ex)
        {
            return exceptionHandler.HandleException(ex);
        }
    }

    public async Task<DefaultResponse> GetCurrentVersionAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            GameVersion? version = await versionRepository.GetCurrentGameVersionAsync(cancellationToken).ConfigureAwait(true);
            return new DefaultResponse(objectResponse: version);
        }
        catch (System.Exception ex)
        {
            return exceptionHandler.HandleException(ex);
        }
    }

    public async Task<DefaultResponse> GetVersionByGuidAsync(string versionGuid, CancellationToken cancellationToken = default)
    {
        try
        {
            if (Guid.TryParse(versionGuid, out Guid guid))
            {
                GameVersion? version = await versionRepository.GetVersionByGuidAsync(guid, cancellationToken).ConfigureAwait(true);
                return new DefaultResponse { ObjectResponse = version };
            }

            return await ReturnGuidInvalidAsync(cancellationToken).ConfigureAwait(true);
        }
        catch (System.Exception ex)
        {
            return exceptionHandler.HandleException(ex);
        }
    }

    public async Task<DefaultResponse> NewVersionAsync(GameVersionModel version, CancellationToken cancellationToken = default)
    {
        try
        {
            if (await VerifyIfVersionExistAsync(version, cancellationToken).ConfigureAwait(true))
                return new DefaultResponse(httpStatus: HttpStatusCode.Conflict, message: "This version already exist.");

            _ = await versionRepository.SetNewGameVersionAsync(mapper.Map<GameVersion>(version), cancellationToken).ConfigureAwait(true);

            return new DefaultResponse(httpStatus: HttpStatusCode.Created, message: "New game version created.");
        }
        catch (System.Exception ex)
        {
            return exceptionHandler.HandleException(ex);
        }
    }

    public async Task<DefaultResponse> SetReleasedAsync(string versionGuid, CancellationToken cancellationToken = default)
    {
        DefaultResponse versionResponse = await GetVersionByGuidAsync(versionGuid, cancellationToken).ConfigureAwait(true);

        return versionResponse.HttpStatus.IsSuccessStatusCode()
            ? await ReleaseVersionAsync(versionResponse, cancellationToken).ConfigureAwait(true)
            : versionResponse;
    }

    public Task<bool> VerifyIfVersionExistAsync(GameVersionModel version, CancellationToken cancellationToken = default) => versionRepository.VerifyIfExistAsync(
        mapper.Map<GameVersion>(version), cancellationToken);

    public async Task<bool> VerifyIfVersionExistAsync(string versionGuid, CancellationToken cancellationToken = default) => Guid.TryParse(versionGuid, out Guid guid) &&
        await versionRepository.VerifyIfExistAsync(guid, cancellationToken).ConfigureAwait(true);
}