//-----------------------------------------------------------------------
// <copyright file="GameFileService.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using ROH.Context.File.Entities;
using ROH.Context.File.Interface;
using ROH.Service.Exception.Interface;
using ROH.Service.File.Interface;
using ROH.StandardModels.File;
using ROH.StandardModels.Response;

using System.Net;

namespace ROH.Service.File;

public class GameFileService(IGameFileRepository gameFileRepository, IExceptionHandler exceptionHandler) : IGameFileService
{
    static string GetSafeFilePath(GameFile gameFile)
    {
        string rootPath = Path.GetFullPath(gameFile.Path);
        string relativePath = NormalizeRelativePath(gameFile.Name);
        string filePath = Path.GetFullPath(Path.Combine(rootPath, relativePath));
        string rootWithSeparator = rootPath.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
            + Path.DirectorySeparatorChar;

        if (!filePath.StartsWith(rootWithSeparator, StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException("Invalid file path.");

        return filePath;
    }

    static string GetRelativeDirectory(string fileName)
    {
        string relativePath = NormalizeRelativePath(fileName);
        string? directory = Path.GetDirectoryName(relativePath);
        return string.IsNullOrWhiteSpace(directory)
            ? string.Empty
            : directory.Replace(Path.DirectorySeparatorChar, '/');
    }

    static string NormalizeRelativePath(string fileName)
    {
        string safeName = string.IsNullOrWhiteSpace(fileName) ? "download.bin" : fileName;
        return safeName
            .Replace('\\', Path.DirectorySeparatorChar)
            .Replace('/', Path.DirectorySeparatorChar)
            .TrimStart(Path.DirectorySeparatorChar);
    }

    async Task<DefaultResponse> GetGameFileAsync(GameFile gameFile, CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(gameFile.Path) && !string.IsNullOrWhiteSpace(gameFile.Name))
                return new DefaultResponse(null, httpStatus: HttpStatusCode.NotFound, message: "File not found.");

            string filePath = GetSafeFilePath(gameFile);

            if (string.IsNullOrWhiteSpace(filePath))
                throw new InvalidOperationException("Cant find the file path!");

            if (System.IO.File.Exists(filePath))
            {
                byte[] fileContent = await System.IO.File
                    .ReadAllBytesAsync(filePath, cancellationToken)
                    .ConfigureAwait(true);

                return new DefaultResponse(
                    new GameFileModel(
                        name: gameFile.Name,
                        format: gameFile.Format,
                        content: fileContent,
                        size: gameFile.Size,
                        active: gameFile.Active,
                        path: GetRelativeDirectory(gameFile.Name)),
                    HttpStatusCode.OK);
            }
            else
            {
                return new DefaultResponse(null, httpStatus: HttpStatusCode.NotFound, message: "File not found.");
            }
        }
        catch (System.Exception ex)
        {
            return exceptionHandler.HandleException(ex);
        }
    }

    public async Task<DefaultResponse> DownloadFileAsync(Guid fileGuid, CancellationToken cancellationToken = default)
    {
        try
        {
            GameFile? file = await gameFileRepository.GetFileAsync(fileGuid, cancellationToken).ConfigureAwait(true);

            return (file is null)
                ? (new DefaultResponse(null, httpStatus: HttpStatusCode.NotFound, message: "File Not Found."))
                : (await GetGameFileAsync(file, cancellationToken).ConfigureAwait(true));
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
            GameFile? file = await gameFileRepository.GetFileAsync(id, cancellationToken).ConfigureAwait(true);

            return (file is null)
                ? (new DefaultResponse(null, httpStatus: HttpStatusCode.NotFound, message: "File Not Found."))
                : (await GetGameFileAsync(file, cancellationToken).ConfigureAwait(true));
        }
        catch (System.Exception ex)
        {
            return exceptionHandler.HandleException(ex);
        }
    }

    public async Task<DefaultResponse> MakeFileHasDeprecatedAsync(
        Guid fileGuid,
        CancellationToken cancellationToken = default)
    {
        try
        {
            GameFile? file = await gameFileRepository.GetFileAsync(fileGuid, cancellationToken).ConfigureAwait(true);

            if (file is null)
                return new DefaultResponse(null, httpStatus: HttpStatusCode.NotFound, message: "File not found.");

            file = file with { Active = false };

            await gameFileRepository.UpdateFileAsync(file, cancellationToken).ConfigureAwait(true);

            return new DefaultResponse(
                HttpStatusCode.OK,
                message: $"The file \"{file.Name}\" of version has marked as deprecated.");
        }
        catch (System.Exception ex)
        {
            return exceptionHandler.HandleException(ex);
        }
    }

    public async Task SaveFileAsync(GameFile file, byte[] content, CancellationToken cancellationToken = default)
    {
        try
        {
            string filePath = GetSafeFilePath(file);
            string? targetDirectory = Path.GetDirectoryName(filePath);

            if (!string.IsNullOrWhiteSpace(targetDirectory) && !Directory.Exists(targetDirectory))
                _ = Directory.CreateDirectory(targetDirectory);

            if (System.IO.File.Exists(filePath))
                System.IO.File.Delete(filePath);

            using (FileStream fs = System.IO.File.Create(filePath))
            {
                await fs.WriteAsync(content.AsMemory(), CancellationToken.None).ConfigureAwait(true);
            }

            await gameFileRepository.SaveFileAsync(file, cancellationToken).ConfigureAwait(true);
        }
        catch (System.Exception ex)
        {
            _ = exceptionHandler.HandleException(ex);
        }
    }
}
