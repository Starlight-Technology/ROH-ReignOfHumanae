//-----------------------------------------------------------------------
// <copyright file="GameVersionService.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using AutoMapper;

using ROH.Domain.Paginator;
using ROH.Domain.Version;
using ROH.Interfaces.Repository.Version;
using ROH.Interfaces.Services.ExceptionService;
using ROH.Interfaces.Services.Version;
using ROH.StandardModels.Paginator;
using ROH.StandardModels.Response;
using ROH.StandardModels.Version;
using ROH.Utils.Helpers;

using System.Net;

namespace ROH.Services.Version;

public class GameVersionService(
    IGameVersionRepository versionRepository,
    IMapper mapper,
    IExceptionHandler exceptionHandler) : IGameVersionService
{
    private async Task<DefaultResponse> ReleaseVersion(DefaultResponse defaultResponse)
    {
        try
        {
            if (defaultResponse.ObjectResponse is null)
                return new DefaultResponse(httpStatus: HttpStatusCode.NotFound, message: "The version has not found!");

            GameVersion? existingVersion = await versionRepository.GetVersionByGuidAsync(
                ((GameVersion)defaultResponse.ObjectResponse).Guid);

            existingVersion!.Released = true;
            existingVersion!.ReleaseDate = DateTime.UtcNow;

            _ = await versionRepository.UpdateGameVersionAsync(existingVersion);

            return new DefaultResponse(message: "The version has been set as release.");
        }
        catch (Exception ex)
        {
            return exceptionHandler.HandleException(ex);
        }
    }

    private static Task<DefaultResponse> ReturnGuidInvalid() => Task.FromResult(
        new DefaultResponse { HttpStatus = HttpStatusCode.ExpectationFailed, Message = "The Guid is invalid!" });

    public async Task<DefaultResponse> GetAllReleasedVersions(int take = 10, int page = 1)
    {
        try
        {
            int skip = take * (page - 1);

            Paginated result = await versionRepository.GetAllReleasedVersionsAsync(take, skip);

            List<GameVersion>? versions = result.ObjectResponse.Cast<GameVersion>().ToList();
            int total = result.Total;
            int pages = 0;
            if (versions.Count > 0)
            {
                pages = (int)Math.Ceiling(((double)total) / take);
            }

            List<GameVersionModel> versionModels = mapper.Map<List<GameVersionModel>>(versions);

            List<object> versionObjects = versionModels.Cast<object>().ToList();

            PaginatedModel paginatedModel = new() { TotalPages = pages, ObjectResponse = versionObjects };

            return new DefaultResponse(objectResponse: paginatedModel, message: "That are all released versions");
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

            Paginated result = await versionRepository.GetAllVersionsAsync(take, skip);

            int total = result.Total;
            int pages = 0;
            List<GameVersion>? versions = result.ObjectResponse.Cast<GameVersion>().ToList();
            if (versions.Count > 0)
            {
                pages = (int)Math.Ceiling(((double)total) / take);
            }

            List<GameVersionModel> versionModels = mapper.Map<List<GameVersionModel>>(versions);
            List<object> versionObjects = versionModels.Cast<object>().ToList();

            PaginatedModel paginatedModel = new() { TotalPages = pages, ObjectResponse = versionObjects };

            return new DefaultResponse(objectResponse: paginatedModel);
        }
        catch (Exception ex)
        {
            return exceptionHandler.HandleException(ex);
        }
    }

    public async Task<DefaultResponse> GetCurrentVersion()
    {
        try
        {
            GameVersion? version = await versionRepository.GetCurrentGameVersionAsync();
            return new DefaultResponse(objectResponse: version);
        }
        catch (Exception ex)
        {
            return exceptionHandler.HandleException(ex);
        }
    }

    public async Task<DefaultResponse> GetVersionByGuid(string versionGuid)
    {
        try
        {
            if (Guid.TryParse(versionGuid, out Guid guid))
            {
                GameVersion? version = await versionRepository.GetVersionByGuidAsync(guid);
                return new DefaultResponse { ObjectResponse = version };
            }

            return await ReturnGuidInvalid();
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
                return new DefaultResponse(httpStatus: HttpStatusCode.Conflict, message: "This version already exist.");
            }

            _ = await versionRepository.SetNewGameVersionAsync(mapper.Map<GameVersion>(version));

            return new DefaultResponse(httpStatus: HttpStatusCode.Created, message: "New game version created.");
        }
        catch (Exception ex)
        {
            return exceptionHandler.HandleException(ex);
        }
    }

    public async Task<DefaultResponse> SetReleased(string versionGuid)
    {
        DefaultResponse versionResponse = await GetVersionByGuid(versionGuid);

        return versionResponse.HttpStatus.IsSuccessStatusCode()
            ? (await ReleaseVersion(versionResponse))
            : versionResponse;
    }

    public async Task<bool> VerifyIfVersionExist(GameVersionModel version) => await versionRepository.VerifyIfExistAsync(
        mapper.Map<GameVersion>(version));

    public async Task<bool> VerifyIfVersionExist(string versionGuid) => Guid.TryParse(versionGuid, out Guid guid) &&
        (await versionRepository.VerifyIfExistAsync(guid));
}