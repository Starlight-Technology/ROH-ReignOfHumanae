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
using ROH.StandardModels.Response;
using ROH.StandardModels.Version;
using ROH.Utils.Helpers;

using System.Collections.Concurrent;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Security.Cryptography;

namespace ROH.Service.File;

public class GameVersionFileService(
    IGameFileService gameFileService,
    IGameVersionService gameVersion,
    IGameVersionFileRepository versionFileRepository,
    IValidator<GameVersionFileModel> validator,
    IMapper mapper,
    IExceptionHandler exceptionHandler) : IGameVersionFileService
{
    private static readonly ConcurrentDictionary<Guid, BuildUploadData> _pendingAnalyses = new();

    private static readonly HashSet<string> _blockedExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".asmdef", ".asmref", ".cs", ".meta", ".tmp"
    };

    private record BuildUploadData(
        Guid VersionGuid,
        string TempRoot,
        string VersionPath,
        GameVersionModel GameVersion,
        List<FileAnalysis> Files);

    private record FileAnalysis(
        string RelativePath,
        long Size,
        string Format,
        FileAction Action,
        Guid? ExistingFileGuid);

    private enum FileAction { New, Update, Identical, Deactivate }

    private static string GetTempRoot()
    {
#if DEBUG
        return @".\ROHUpdateFiles\_uploadTemp";
#else
        return "/app/ROH/updateFiles/_uploadTemp";
#endif
    }

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

    private static string BuildStoredRelativeName(GameVersionFileModel fileModel)
    {
        string normalizedName = NormalizeInstallPath(fileModel.Name);
        string fileName = Path.GetFileName(normalizedName.Replace('/', Path.DirectorySeparatorChar));
        string targetFolder = NormalizeInstallPath(fileModel.Path);

        if (string.IsNullOrWhiteSpace(fileName))
            fileName = "download.bin";

        return !string.IsNullOrWhiteSpace(targetFolder)
            ? $"{targetFolder}/{fileName}"
            : string.IsNullOrWhiteSpace(normalizedName) ? fileName : normalizedName;
    }

    private static string GetRelativeDirectory(string fileName)
    {
        string relativePath = NormalizeInstallPath(fileName);
        string? directory = Path.GetDirectoryName(relativePath.Replace('/', Path.DirectorySeparatorChar));

        return string.IsNullOrWhiteSpace(directory)
            ? string.Empty
            : directory.Replace(Path.DirectorySeparatorChar, '/');
    }

    private static string NormalizeInstallPath(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return string.Empty;

        char[] invalidChars = Path.GetInvalidFileNameChars();

        IEnumerable<string> segments = value
            .Replace('\\', '/')
            .Trim()
            .Trim('/')
            .Split('/', StringSplitOptions.RemoveEmptyEntries)
            .Where(segment => segment is not "." and not "..")
            .Select(segment => string.Join("_", segment.Split(invalidChars, StringSplitOptions.RemoveEmptyEntries)))
            .Where(segment => !string.IsNullOrWhiteSpace(segment));

        return string.Join("/", segments);
    }

    private static string GetRejectionMessage(GameVersionModel gameVersion) => gameVersion.Released
        ? "File Upload Failed: This version has already been released. You cannot upload new files for a released version."
        : "File Upload Failed: Cannot upload files to a version older than the current released version.";

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
        file = file with
        {
            Name = BuildStoredRelativeName(fileModel),
            Path = path
        };

        await gameFileService.SaveFileAsync(file, fileModel.Content!, cancellationToken).ConfigureAwait(true);

        versionFile = versionFile with { Guid = file.Guid, GameFile = file };
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
                : new DefaultResponse(
                    null,
                    httpStatus: HttpStatusCode.NotFound,
                    message: "Game Version File Not Found.");
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
                : new DefaultResponse(
                    null,
                    httpStatus: HttpStatusCode.NotFound,
                    message: "Game Version File Not Found.");
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

                    List<GameVersionFileModel> filesModels = files
                        .Where(item => item.GameFile is not null)
                        .Select(
                            item => new GameVersionFileModel
                            {
                                Guid = item.Guid,
                                Name = item.GameFile!.Name,
                                Path = GetRelativeDirectory(item.GameFile.Name),
                                Format = item.GameFile.Format,
                                Size = item.GameFile.Size,
                                Active = item.GameFile.Active
                            })
                        .ToList();

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
                : new DefaultResponse(null, HttpStatusCode.BadRequest, "Game Version Not Found.");
        }
        catch (System.Exception ex)
        {
            return exceptionHandler.HandleException(ex);
        }
    }

    public async Task<DefaultResponse> UploadBuildZipAsync(
        Stream zipStream,
        Guid versionGuid,
        CancellationToken cancellationToken = default)
    {
        try
        {
            bool versionExists = await gameVersion.VerifyIfVersionExistAsync(versionGuid, cancellationToken)
                .ConfigureAwait(true);

            if (!versionExists)
                return new DefaultResponse(null, HttpStatusCode.BadRequest, "Game Version Not Found.");

            GameVersionModel? versionInfo = await GetCurrentVersionAsync(cancellationToken).ConfigureAwait(true);

            string versionPath = GetFilePath(versionInfo ?? new GameVersionModel { Guid = versionGuid });

            Guid analysisId = Guid.NewGuid();
            string tempRoot = Path.Combine(GetTempRoot(), analysisId.ToString());

            List<FileAnalysis> files = [];
            List<string> errors = [];

            using ZipArchive archive = new(zipStream, ZipArchiveMode.Read);

            List<GameVersionFile> existingFiles = await versionFileRepository
                .GetFilesAsync(versionGuid, cancellationToken).ConfigureAwait(true);
            Dictionary<string, GameFile> existingMap = [];
            foreach (GameVersionFile vf in existingFiles)
            {
                if (vf.GameFile is not null && vf.GameFile.Active)
                    existingMap[NormalizeInstallPath(vf.GameFile.Name)] = vf.GameFile;
            }

            foreach (ZipArchiveEntry entry in archive.Entries)
            {
                if (string.IsNullOrWhiteSpace(entry.Name))
                    continue;

                string relativePath = NormalizeInstallPath(entry.FullName);
                string ext = Path.GetExtension(relativePath);

                if (_blockedExtensions.Contains(ext))
                    continue;

                string tempFilePath = Path.GetFullPath(Path.Combine(tempRoot, relativePath));
                string tempDir = Path.GetDirectoryName(tempFilePath)!;
                if (!Directory.Exists(tempDir))
                    Directory.CreateDirectory(tempDir);

                entry.ExtractToFile(tempFilePath, overwrite: true);

                long size = entry.Length;
                string format = ext;

                if (existingMap.TryGetValue(relativePath, out GameFile? existingFile))
                {
                    string existingFilePath = GetSafeFilePath(existingFile);
                    bool contentIdentical = FilesAreIdentical(tempFilePath, existingFilePath);

                    files.Add(new FileAnalysis(
                        relativePath, size, format,
                        contentIdentical ? FileAction.Identical : FileAction.Update,
                        existingFile.Guid));
                    existingMap.Remove(relativePath);
                }
                else
                {
                    files.Add(new FileAnalysis(
                        relativePath, size, format,
                        FileAction.New, null));
                }
            }

            foreach ((string _, GameFile remaining) in existingMap)
            {
                files.Add(new FileAnalysis(
                    NormalizeInstallPath(remaining.Name),
                    remaining.Size,
                    remaining.Format,
                    FileAction.Deactivate,
                    remaining.Guid));
            }

            var data = new BuildUploadData(versionGuid, tempRoot, versionPath, versionInfo ?? new(), files);
            _pendingAnalyses[analysisId] = data;

            var analysis = new BuildUploadAnalysis
            {
                AnalysisId = analysisId,
                NewFiles = files.Where(f => f.Action == FileAction.New)
                    .Select(f => new FileEntry { RelativePath = f.RelativePath, Size = f.Size, Format = f.Format }).ToList(),
                UpdatedFiles = files.Where(f => f.Action == FileAction.Update)
                    .Select(f => new FileEntry { RelativePath = f.RelativePath, Size = f.Size, Format = f.Format }).ToList(),
                IdenticalFiles = files.Where(f => f.Action == FileAction.Identical)
                    .Select(f => new FileEntry { RelativePath = f.RelativePath, Size = f.Size, Format = f.Format }).ToList(),
                DeactivatedFiles = files.Where(f => f.Action == FileAction.Deactivate)
                    .Select(f => new FileEntry { RelativePath = f.RelativePath, Size = f.Size, Format = f.Format }).ToList(),
                Errors = errors
            };

            return new DefaultResponse(objectResponse: analysis, httpStatus: HttpStatusCode.OK);
        }
        catch (System.Exception ex)
        {
            return exceptionHandler.HandleException(ex);
        }
    }

    public async Task<DefaultResponse> ConfirmBuildUploadAsync(
        BuildUploadConfirmation confirmation,
        CancellationToken cancellationToken = default)
    {
        try
        {
            if (!_pendingAnalyses.TryRemove(confirmation.AnalysisId, out BuildUploadData? data))
                return new DefaultResponse(null, HttpStatusCode.BadRequest, "Analysis not found or expired.");

            BuildUploadResult result = new() { Success = true };

            foreach (FileAnalysis file in data.Files)
            {
                try
                {
                    switch (file.Action)
                    {
                        case FileAction.New:
                            await AddNewFileFromTempAsync(data, file, cancellationToken).ConfigureAwait(true);
                            result.Added++;
                            break;

                        case FileAction.Update:
                            await UpdateExistingFileFromTempAsync(data, file, cancellationToken).ConfigureAwait(true);
                            result.Updated++;
                            break;

                        case FileAction.Identical when confirmation.ReplaceIdentical:
                            await UpdateExistingFileFromTempAsync(data, file, cancellationToken).ConfigureAwait(true);
                            result.Updated++;
                            break;

                        case FileAction.Identical:
                            result.Skipped++;
                            break;

                        case FileAction.Deactivate:
                            await gameFileService.MakeFileHasDeprecatedAsync(
                                file.ExistingFileGuid!.Value, cancellationToken).ConfigureAwait(true);
                            result.Deactivated++;
                            break;
                    }
                }
                catch (System.Exception ex)
                {
                    result.Errors.Add($"Failed to process '{file.RelativePath}': {ex.Message}");
                }
            }

            try
            {
                if (Directory.Exists(data.TempRoot))
                    Directory.Delete(data.TempRoot, recursive: true);
            }
            catch
            {
            }

            result.Success = result.Errors.Count == 0;
            return new DefaultResponse(objectResponse: result, httpStatus: HttpStatusCode.OK);
        }
        catch (System.Exception ex)
        {
            return exceptionHandler.HandleException(ex);
        }
    }

    private static string GetSafeFilePath(GameFile gameFile)
    {
        string rootPath = Path.GetFullPath(gameFile.Path);
        string relativePath = NormalizeRelativePath(gameFile.Name);
        string filePath = Path.GetFullPath(Path.Combine(rootPath, relativePath));
        string rootWithSeparator = rootPath.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
            + Path.DirectorySeparatorChar;

        return !filePath.StartsWith(rootWithSeparator, StringComparison.OrdinalIgnoreCase)
            ? throw new InvalidOperationException("Invalid file path.")
            : filePath;
    }

    private static string NormalizeRelativePath(string fileName)
    {
        string safeName = string.IsNullOrWhiteSpace(fileName) ? "download.bin" : fileName;
        return safeName
            .Replace('\\', Path.DirectorySeparatorChar)
            .Replace('/', Path.DirectorySeparatorChar)
            .TrimStart(Path.DirectorySeparatorChar);
    }

    private static bool FilesAreIdentical(string pathA, string pathB)
    {
        if (!System.IO.File.Exists(pathA) || !System.IO.File.Exists(pathB))
            return false;

        byte[] hashA = SHA256.HashData(System.IO.File.ReadAllBytes(pathA));
        byte[] hashB = SHA256.HashData(System.IO.File.ReadAllBytes(pathB));

        return CryptographicOperations.FixedTimeEquals(hashA, hashB);
    }

    private async Task AddNewFileFromTempAsync(
        BuildUploadData data,
        FileAnalysis file,
        CancellationToken cancellationToken)
    {
        string tempFilePath = Path.GetFullPath(Path.Combine(data.TempRoot, file.RelativePath));
        byte[] content = await System.IO.File.ReadAllBytesAsync(tempFilePath, cancellationToken).ConfigureAwait(true);

        var fileModel = new GameVersionFileModel
        {
            Name = file.RelativePath,
            Path = GetRelativeDirectory(file.RelativePath),
            Format = file.Format,
            Content = content,
            Size = file.Size,
            Active = true,
            GameVersion = data.GameVersion
        };

        GameVersionFile versionFile = mapper.Map<GameVersionFile>(fileModel);
        GameVersionModel? currentVersion = await GetCurrentVersionAsync(cancellationToken).ConfigureAwait(true);

        if (await ShouldRejectFileUploadAsync(data.GameVersion, currentVersion, cancellationToken).ConfigureAwait(true))
            throw new InvalidOperationException(GetRejectionMessage(data.GameVersion));

        GameFile dbFile = mapper.Map<GameFile>(fileModel);
        dbFile = dbFile with
        {
            Name = BuildStoredRelativeName(fileModel),
            Path = data.VersionPath
        };

        await gameFileService.SaveFileAsync(dbFile, content, cancellationToken).ConfigureAwait(true);

        versionFile = versionFile with { Guid = dbFile.Guid, GameFile = dbFile };
        await versionFileRepository.SaveFileAsync(versionFile, cancellationToken).ConfigureAwait(true);
    }

    private async Task UpdateExistingFileFromTempAsync(
        BuildUploadData data,
        FileAnalysis file,
        CancellationToken cancellationToken)
    {
        string tempFilePath = Path.GetFullPath(Path.Combine(data.TempRoot, file.RelativePath));
        byte[] content = await System.IO.File.ReadAllBytesAsync(tempFilePath, cancellationToken).ConfigureAwait(true);

        GameVersionFile? versionFile = await versionFileRepository.GetFileAsync(
            file.ExistingFileGuid!.Value, cancellationToken).ConfigureAwait(true);

        GameFile? existingFile = versionFile?.GameFile;

        if (existingFile is null)
            return;

        GameFile updatedFile = existingFile with { Size = file.Size };
        await gameFileService.UpdateFileContentAsync(updatedFile, content, cancellationToken).ConfigureAwait(true);
    }
}
