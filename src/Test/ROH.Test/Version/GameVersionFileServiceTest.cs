//-----------------------------------------------------------------------
// <copyright file="GameVersionFileServiceTest.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using AutoMapper;

using FluentValidation;
using FluentValidation.Results;

using Moq;

using ROH.Context.File.Entities;
using ROH.Context.File.Interface;
using ROH.Context.Version.Entities;
using ROH.Service.Exception.Interface;
using ROH.Service.File;
using ROH.Service.File.Interface;
using ROH.StandardModels.Response;
using ROH.StandardModels.Version;
using ROH.Utils.Helpers;

using System.IO.Compression;
using System.Net;
using System.Text;

namespace ROH.Test.Version;

public class GameVersionFileServiceTest
{
    private readonly Mock<IExceptionHandler> _mockExceptionHandler;
    private readonly Mock<IGameFileService> _mockGameFileService;
    private readonly Mock<IGameVersionFileRepository> _mockGameVersionFileRepository;
    private readonly Mock<IGameVersionService> _mockGameVersionService;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IValidator<GameVersionFileModel>> _mockValidator;
    private readonly GameVersionFileService _service;

    public GameVersionFileServiceTest()
    {
        _mockGameVersionFileRepository = new Mock<IGameVersionFileRepository>();
        _mockGameFileService = new Mock<IGameFileService>();
        _mockValidator = new Mock<IValidator<GameVersionFileModel>>();
        _mockGameVersionService = new Mock<IGameVersionService>();
        _mockMapper = new Mock<IMapper>();
        _mockExceptionHandler = new Mock<IExceptionHandler>();

        _service = new GameVersionFileService(
            _mockGameFileService.Object,
            _mockGameVersionService.Object,
            _mockGameVersionFileRepository.Object,
            _mockValidator.Object,
            _mockMapper.Object,
            _mockExceptionHandler.Object);
    }

    [Fact]
    public async Task DownloadFileByGuidFileNotFound()
    {
        // Arrange
        Guid fileGuid = Guid.NewGuid();

        _mockGameVersionFileRepository.Setup(repo => repo.GetFileAsync(fileGuid, It.IsAny<CancellationToken>()))
            .ReturnsAsync((GameVersionFile?)null);

        // Act
        DefaultResponse result = await _service.DownloadFileAsync(fileGuid, CancellationToken.None).ConfigureAwait(true);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(HttpStatusCode.NotFound, result.HttpStatus);
        Assert.Equal("Game Version File Not Found.", result.Message);
    }

    [Fact]
    public async Task DownloadFileByGuidReturnsFileSuccessfully()
    {
        // Arrange
        Guid fileGuid = Guid.NewGuid();
        Context.File.Entities.GameFile gameFile = new() { Guid = fileGuid };
        GameVersionFile versionFile = new() { GameFile = gameFile };

        _mockGameVersionFileRepository.Setup(repo => repo.GetFileAsync(fileGuid, It.IsAny<CancellationToken>()))
            .ReturnsAsync(versionFile);

        _mockGameFileService.Setup(service => service.DownloadFileAsync(fileGuid, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DefaultResponse(HttpStatusCode.OK));

        // Act
        DefaultResponse result = await _service.DownloadFileAsync(fileGuid, CancellationToken.None).ConfigureAwait(true);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(HttpStatusCode.OK, result.HttpStatus);
    }

    [Fact]
    public async Task DownloadFileByGuidShouldHandleException()
    {
        // Arrange
        Guid fileGuid = Guid.NewGuid();

        _mockGameVersionFileRepository.Setup(repo => repo.GetFileAsync(fileGuid, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception());
        _mockExceptionHandler.Setup(x => x.HandleException(It.IsAny<Exception>()))
            .Returns(
                new DefaultResponse(httpStatus: HttpStatusCode.InternalServerError, message: "Internal Server Error"));

        // Act
        DefaultResponse result = await _service.DownloadFileAsync(fileGuid, CancellationToken.None).ConfigureAwait(true);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(HttpStatusCode.InternalServerError, result.HttpStatus);
        Assert.Equal("Internal Server Error", result.Message);
    }

    [Fact]
    public async Task DownloadFileByIdFileNotFound()
    {
        // Arrange
        long fileId = 1;

        _mockGameVersionFileRepository.Setup(repo => repo.GetFileAsync(fileId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((GameVersionFile?)null);

        // Act
        DefaultResponse result = await _service.DownloadFileAsync(fileId, CancellationToken.None).ConfigureAwait(true);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(HttpStatusCode.NotFound, result.HttpStatus);
        Assert.Equal("Game Version File Not Found.", result.Message);
    }

    [Fact]
    public async Task DownloadFileByIdReturnsFileSuccessfully()
    {
        // Arrange
        long fileId = 1;
        Context.File.Entities.GameFile gameFile = new() { Id = fileId };
        GameVersionFile versionFile = new() { GameFile = gameFile };

        _mockGameVersionFileRepository.Setup(repo => repo.GetFileAsync(fileId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(versionFile);

        _mockGameFileService.Setup(service => service.DownloadFileAsync(fileId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DefaultResponse(HttpStatusCode.OK));

        // Act
        DefaultResponse result = await _service.DownloadFileAsync(fileId, CancellationToken.None).ConfigureAwait(true);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(HttpStatusCode.OK, result.HttpStatus);
    }

    [Fact]
    public async Task DownloadFileByIdShouldHandleException()
    {
        // Arrange
        long fileId = 1;

        _mockGameVersionFileRepository.Setup(repo => repo.GetFileAsync(fileId, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception());
        _mockExceptionHandler.Setup(x => x.HandleException(It.IsAny<Exception>()))
            .Returns(
                new DefaultResponse(httpStatus: HttpStatusCode.InternalServerError, message: "Internal Server Error"));

        // Act
        DefaultResponse result = await _service.DownloadFileAsync(fileId, CancellationToken.None).ConfigureAwait(true);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(HttpStatusCode.InternalServerError, result.HttpStatus);
        Assert.Equal("Internal Server Error", result.Message);
    }

    [Fact]
    public async Task GetFilesShouldHandleException()
    {
        // Arrange
        Guid versionGuid = Guid.NewGuid();

        _mockGameVersionService.Setup(
            service => service.VerifyIfVersionExistAsync(versionGuid, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception());

        _mockExceptionHandler.Setup(x => x.HandleException(It.IsAny<Exception>()))
            .Returns(
                new DefaultResponse(httpStatus: HttpStatusCode.InternalServerError, message: "Internal Server Error"));

        // Act
        DefaultResponse result = await _service.GetFilesAsync(versionGuid.ToString(), CancellationToken.None)
            .ConfigureAwait(true);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(HttpStatusCode.InternalServerError, result.HttpStatus);
        Assert.Equal("Internal Server Error", result.Message);
    }

    [Fact]
    public async Task GetFilesVersionExistsReturnsFiles()
    {
        // Arrange
        Guid versionGuid = Guid.NewGuid();
        List<GameVersionFile> files = [new()];
        List<GameVersionFileModel> filesModel = [new()];

        _mockMapper.Setup(mapper => mapper.Map<List<GameVersionFileModel>>(files)).Returns(filesModel);

        _mockGameVersionService.Setup(
            service => service.VerifyIfVersionExistAsync(versionGuid, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _mockGameVersionFileRepository.Setup(
            repo => repo.GetFilesAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(files);

        // Act
        DefaultResponse result = await _service.GetFilesAsync(versionGuid.ToString(), CancellationToken.None)
            .ConfigureAwait(true);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(HttpStatusCode.OK, result.HttpStatus);
        Assert.NotNull(result.ObjectResponse);
    }

    [Fact]
    public async Task GetFilesVersionExistsReturnsManifestWithoutFileContent()
    {
        // Arrange
        Guid versionGuid = Guid.NewGuid();
        Guid fileGuid = Guid.NewGuid();
        List<GameVersionFile> files =
        [
            new(GuidVersion: versionGuid, Guid: fileGuid)
            {
                GameFile = new Context.File.Entities.GameFile(
                    Name: "ReignOfHumanae_Data/StreamingAssets/aa/StandaloneWindows64/catalog.json",
                    Format: ".json",
                    Size: 123,
                    Active: true)
            }
        ];

        _mockGameVersionService.Setup(
            service => service.VerifyIfVersionExistAsync(versionGuid, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _mockGameVersionFileRepository.Setup(
            repo => repo.GetFilesAsync(versionGuid, It.IsAny<CancellationToken>()))
            .ReturnsAsync(files);

        // Act
        DefaultResponse result = await _service.GetFilesAsync(versionGuid.ToString(), CancellationToken.None)
            .ConfigureAwait(true);

        // Assert
        List<GameVersionFileModel> manifest = Assert.IsType<List<GameVersionFileModel>>(result.ObjectResponse);
        GameVersionFileModel manifestFile = Assert.Single(manifest);
        Assert.Equal(fileGuid, manifestFile.Guid);
        Assert.Equal("ReignOfHumanae_Data/StreamingAssets/aa/StandaloneWindows64/catalog.json", manifestFile.Name);
        Assert.Equal("ReignOfHumanae_Data/StreamingAssets/aa/StandaloneWindows64", manifestFile.Path);
        Assert.Null(manifestFile.Content);
        Assert.True(manifestFile.Active);
        Assert.Equal(123, manifestFile.Size);
    }

    [Fact]
    public async Task GetFilesVersionNotExistsReturnsNotFound()
    {
        // Arrange
        Guid versionGuid = Guid.NewGuid();

        _mockGameVersionService.Setup(
            service => service.VerifyIfVersionExistAsync(versionGuid, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        DefaultResponse result = await _service.GetFilesAsync(versionGuid.ToString(), CancellationToken.None)
            .ConfigureAwait(true);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(HttpStatusCode.NotFound, result.HttpStatus);
    }

    [Fact]
    public async Task NewFileFileContentIsNullReturnsBadRequest()
    {
        // Arrange
        GameVersionFileModel fileModel = new();

        // Act
        DefaultResponse result = await _service.NewFileAsync(fileModel, CancellationToken.None).ConfigureAwait(true);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(HttpStatusCode.BadRequest, result.HttpStatus);
        Assert.Equal("File content can't be empty.", result.Message);
    }

    [Fact]
    public async Task NewFileOldVersionReturnsBadRequest()
    {
        // Arrange
        Guid testGuid = Guid.NewGuid();

        GameVersionFileModel fileModel = new()
        {
            Content = [1, 2, 3],
            GameVersion = new GameVersionModel { Guid = testGuid, Released = true }
        };
        GameVersion gameVersion = new(VersionDate: DateTime.UtcNow.AddDays(1), Guid: testGuid);
        GameVersionFile versionFile = new() { GuidVersion = gameVersion.Guid };

        _mockMapper.Setup(mapper => mapper.Map<GameVersionFile>(fileModel)).Returns(versionFile);

        _mockValidator.Setup(validator => validator.ValidateAsync(fileModel, default))
            .ReturnsAsync(new ValidationResult());

        _mockGameVersionService.Setup(
            service => service.VerifyIfVersionExistAsync(gameVersion.Guid, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _mockGameVersionService.Setup(service => service.GetCurrentVersionAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(
                new VersionServiceApi.DefaultResponse
                {
                    ObjectResponse =
                        new GameVersion(VersionDate: DateTime.UtcNow.AddDays(2)) { Released = true }.ToJson()
                });

        // Act
        DefaultResponse result = await _service.NewFileAsync(fileModel, CancellationToken.None).ConfigureAwait(true);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(HttpStatusCode.BadRequest, result.HttpStatus);
        Assert.Equal(
            "File Upload Failed: This version has already been released. You cannot upload new files for a released version.",
            result.Message);
    }

    [Fact]
    public async Task NewFileShouldHandleException()
    {
        // Arrange
        GameVersionFileModel fileModel = new() { Content = [1, 2, 3], GameVersion = null };
        GameVersion gameVersion = new(VersionDate: DateTime.UtcNow.AddDays(1), Guid: Guid.NewGuid());
        GameVersionFile versionFile = new() { GuidVersion = gameVersion.Guid };

        _mockMapper.Setup(mapper => mapper.Map<GameVersionFile>(fileModel)).Returns(versionFile);

        _mockValidator.Setup(validator => validator.ValidateAsync(fileModel, default))
            .ReturnsAsync(new ValidationResult());

        _mockGameVersionService.Setup(
            service => service.VerifyIfVersionExistAsync(gameVersion.Guid, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        _mockGameVersionService.Setup(service => service.GetCurrentVersionAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception());

        _mockExceptionHandler.Setup(x => x.HandleException(It.IsAny<Exception>()))
            .Returns(
                new DefaultResponse(httpStatus: HttpStatusCode.InternalServerError, message: "Internal Server Error"));

        // Act
        DefaultResponse result = await _service.NewFileAsync(fileModel, CancellationToken.None).ConfigureAwait(true);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(HttpStatusCode.InternalServerError, result.HttpStatus);
        Assert.Equal("Internal Server Error", result.Message);
    }

    [Fact]
    public async Task NewFileSuccessfullySavesFile()
    {
        // Arrange
        Guid testGuid = Guid.NewGuid();

        GameVersionFileModel fileModel = new()
        {
            Content = [1, 2, 3],
            GameVersion = new GameVersionModel { Guid = testGuid, VersionDate = DateTime.UtcNow.AddDays(2) }
        };
        GameVersion gameVersion = new(VersionDate: DateTime.UtcNow.AddDays(1), Guid: testGuid);
        GameVersionFile versionFile = new() { GuidVersion = gameVersion.Guid };

        _mockValidator.Setup(validator => validator.ValidateAsync(fileModel, default))
            .ReturnsAsync(new ValidationResult());

        _mockMapper.Setup(mapper => mapper.Map<GameVersionFile>(fileModel)).Returns(versionFile);

        _mockGameVersionService.Setup(
            service => service.VerifyIfVersionExistAsync(gameVersion.Guid, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _mockGameVersionService.Setup(service => service.GetCurrentVersionAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new VersionServiceApi.DefaultResponse { ObjectResponse = gameVersion.ToJson() });

        _mockMapper.Setup(mapper => mapper.Map<Context.File.Entities.GameFile>(fileModel))
            .Returns(new Context.File.Entities.GameFile());

        _mockGameFileService.Setup(
            service => service.SaveFileAsync(
                It.IsAny<Context.File.Entities.GameFile>(),
                fileModel.Content!,
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _mockGameVersionFileRepository.Setup(repo => repo.SaveFileAsync(versionFile, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        DefaultResponse result = await _service.NewFileAsync(fileModel, CancellationToken.None).ConfigureAwait(true);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(HttpStatusCode.OK, result.HttpStatus);
    }

    [Fact]
    public async Task NewFileValidationFailsReturnsBadRequest()
    {
        // Arrange
        GameVersionFileModel fileModel = new() { Content = [1, 2, 3] };
        ValidationResult validationResult = new(new List<ValidationFailure> { new("Name", "Name is required.") });

        _mockValidator.Setup(validator => validator.ValidateAsync(fileModel, default)).ReturnsAsync(validationResult);

        // Act
        DefaultResponse result = await _service.NewFileAsync(fileModel, CancellationToken.None).ConfigureAwait(true);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(HttpStatusCode.BadRequest, result.HttpStatus);
        Assert.Equal("Name is required.", result.Message);
    }

    [Fact]
    public async Task NewFileVersionNotFoundReturnsBadRequest()
    {
        // Arrange
        GameVersionFileModel fileModel = new()
        {
            Content = [1, 2, 3],
            GameVersion = new GameVersionModel { Guid = Guid.NewGuid() }
        };
        GameVersion gameVersion = new(VersionDate: DateTime.UtcNow.AddDays(1), Guid: Guid.NewGuid());
        GameVersionFile versionFile = new() { GuidVersion = gameVersion.Guid };

        _mockMapper.Setup(mapper => mapper.Map<GameVersionFile>(fileModel)).Returns(versionFile);

        _mockValidator.Setup(validator => validator.ValidateAsync(fileModel, default))
            .ReturnsAsync(new ValidationResult());

        _mockGameVersionService.Setup(
            service => service.VerifyIfVersionExistAsync(gameVersion.Guid, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        DefaultResponse result = await _service.NewFileAsync(fileModel, CancellationToken.None).ConfigureAwait(true);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(HttpStatusCode.BadRequest, result.HttpStatus);
        Assert.Equal("Game Version Not Found.", result.Message);
    }

    [Fact]
    public async Task NewFileVersionReleasedReturnsBadRequest()
    {
        // Arrange
        Guid testGuid = Guid.NewGuid();

        GameVersionFileModel fileModel = new()
        {
            Content = [1, 2, 3],
            GameVersion = new GameVersionModel { Guid = testGuid, Released = true }
        };
        GameVersion gameVersion = new(VersionDate: DateTime.UtcNow.AddDays(1), Guid: testGuid) { Released = true };
        GameVersionFile versionFile = new() { GuidVersion = gameVersion.Guid };

        _mockMapper.Setup(mapper => mapper.Map<GameVersionFile>(fileModel)).Returns(versionFile);

        _mockValidator.Setup(validator => validator.ValidateAsync(fileModel, default))
            .ReturnsAsync(new ValidationResult());

        _mockGameVersionService.Setup(
            service => service.VerifyIfVersionExistAsync(gameVersion.Guid, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _mockGameVersionService.Setup(service => service.GetCurrentVersionAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new VersionServiceApi.DefaultResponse { ObjectResponse = gameVersion.ToJson() });

        // Act
        DefaultResponse result = await _service.NewFileAsync(fileModel, CancellationToken.None).ConfigureAwait(true);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(HttpStatusCode.BadRequest, result.HttpStatus);
        Assert.Equal(
            "File Upload Failed: This version has already been released. You cannot upload new files for a released version.",
            result.Message);
    }

    [Fact]
    public async Task UploadBuildZipVersionNotFoundReturnsBadRequest()
    {
        // Arrange
        Guid versionGuid = Guid.NewGuid();

        using MemoryStream emptyZip = new();
        using (ZipArchive archive = new(emptyZip, ZipArchiveMode.Create, leaveOpen: true))
        {
        }
        emptyZip.Position = 0;

        _mockGameVersionService.Setup(
            service => service.VerifyIfVersionExistAsync(versionGuid, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        DefaultResponse result = await _service.UploadBuildZipAsync(emptyZip, versionGuid, CancellationToken.None)
            .ConfigureAwait(true);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(HttpStatusCode.BadRequest, result.HttpStatus);
        Assert.Equal("Game Version Not Found.", result.Message);
    }

    [Fact]
    public async Task UploadBuildZipShouldHandleException()
    {
        // Arrange
        using MemoryStream emptyZip = new();

        _mockGameVersionService.Setup(
            service => service.VerifyIfVersionExistAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Test error"));

        _mockExceptionHandler.Setup(x => x.HandleException(It.IsAny<Exception>()))
            .Returns(new DefaultResponse(httpStatus: HttpStatusCode.InternalServerError, message: "Internal Server Error"));

        // Act
        DefaultResponse result = await _service.UploadBuildZipAsync(emptyZip, Guid.NewGuid(), CancellationToken.None)
            .ConfigureAwait(true);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(HttpStatusCode.InternalServerError, result.HttpStatus);
    }

    [Fact]
    public async Task UploadBuildZipWithNewFilesReturnsAnalysis()
    {
        // Arrange
        Guid versionGuid = Guid.NewGuid();

        using MemoryStream zipStream = new();
        using (ZipArchive archive = new(zipStream, ZipArchiveMode.Create, leaveOpen: true))
        {
            ZipArchiveEntry entry = archive.CreateEntry("testFile.txt");
            using Stream writer = entry.Open();
            byte[] content = Encoding.UTF8.GetBytes("test content");
            writer.Write(content, 0, content.Length);
        }
        zipStream.Position = 0;

        _mockGameVersionService.Setup(
            service => service.VerifyIfVersionExistAsync(versionGuid, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _mockGameVersionService.Setup(service => service.GetCurrentVersionAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new VersionServiceApi.DefaultResponse { ObjectResponse = new GameVersion(VersionDate: DateTime.UtcNow).ToJson() });

        _mockGameVersionFileRepository.Setup(
            repo => repo.GetFilesAsync(versionGuid, It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        // Act
        DefaultResponse result = await _service.UploadBuildZipAsync(zipStream, versionGuid, CancellationToken.None)
            .ConfigureAwait(true);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(HttpStatusCode.OK, result.HttpStatus);
        Assert.NotNull(result.ObjectResponse);

        BuildUploadAnalysis analysis = Assert.IsType<BuildUploadAnalysis>(result.ObjectResponse);
        Assert.Single(analysis.NewFiles);
        Assert.Equal("testFile.txt", analysis.NewFiles[0].RelativePath);
        Assert.Empty(analysis.UpdatedFiles);
        Assert.Empty(analysis.IdenticalFiles);
        Assert.Empty(analysis.DeactivatedFiles);
        Assert.Empty(analysis.Errors);

        // Cleanup temp files created during analysis
        CleanupAnalysisTemp(analysis.AnalysisId);
    }

    [Fact]
    public async Task UploadBuildZipFiltersBlockedExtensions()
    {
        // Arrange
        Guid versionGuid = Guid.NewGuid();

        using MemoryStream zipStream = new();
        using (ZipArchive archive = new(zipStream, ZipArchiveMode.Create, leaveOpen: true))
        {
            ZipArchiveEntry entry1 = archive.CreateEntry("validFile.txt");
            using (Stream writer1 = entry1.Open())
            {
                byte[] content1 = Encoding.UTF8.GetBytes("valid");
                writer1.Write(content1, 0, content1.Length);
            }

            ZipArchiveEntry entry2 = archive.CreateEntry("blockedFile.cs");
            using (Stream writer2 = entry2.Open())
            {
                byte[] content2 = Encoding.UTF8.GetBytes("blocked");
                writer2.Write(content2, 0, content2.Length);
            }
        }
        zipStream.Position = 0;

        _mockGameVersionService.Setup(
            service => service.VerifyIfVersionExistAsync(versionGuid, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _mockGameVersionService.Setup(service => service.GetCurrentVersionAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new VersionServiceApi.DefaultResponse { ObjectResponse = new GameVersion(VersionDate: DateTime.UtcNow).ToJson() });

        _mockGameVersionFileRepository.Setup(
            repo => repo.GetFilesAsync(versionGuid, It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        // Act
        DefaultResponse result = await _service.UploadBuildZipAsync(zipStream, versionGuid, CancellationToken.None)
            .ConfigureAwait(true);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(HttpStatusCode.OK, result.HttpStatus);

        BuildUploadAnalysis analysis = Assert.IsType<BuildUploadAnalysis>(result.ObjectResponse);
        Assert.Single(analysis.NewFiles);
        Assert.Equal("validFile.txt", analysis.NewFiles[0].RelativePath);

        // Cleanup
        CleanupAnalysisTemp(analysis.AnalysisId);
    }

    [Fact]
    public async Task ConfirmBuildUploadAnalysisNotFoundReturnsBadRequest()
    {
        // Arrange
        BuildUploadConfirmation confirmation = new()
        {
            AnalysisId = Guid.NewGuid(),
            ReplaceIdentical = false
        };

        // Act
        DefaultResponse result = await _service.ConfirmBuildUploadAsync(confirmation, CancellationToken.None)
            .ConfigureAwait(true);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(HttpStatusCode.BadRequest, result.HttpStatus);
        Assert.Equal("Analysis not found or expired.", result.Message);
    }

    [Fact]
    public async Task ConfirmBuildUploadWithNewFileSucceeds()
    {
        // Arrange
        Guid versionGuid = Guid.NewGuid();

        using MemoryStream zipStream = new();
        using (ZipArchive archive = new(zipStream, ZipArchiveMode.Create, leaveOpen: true))
        {
            ZipArchiveEntry entry = archive.CreateEntry("testFile.txt");
            using (Stream writer = entry.Open())
            {
                byte[] content = Encoding.UTF8.GetBytes("test content");
                writer.Write(content, 0, content.Length);
            }
        }
        zipStream.Position = 0;

        _mockGameVersionService.Setup(
            service => service.VerifyIfVersionExistAsync(versionGuid, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _mockGameVersionService.Setup(service => service.GetCurrentVersionAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new VersionServiceApi.DefaultResponse { ObjectResponse = new GameVersion(VersionDate: DateTime.UtcNow).ToJson() });

        _mockGameVersionFileRepository.Setup(
            repo => repo.GetFilesAsync(versionGuid, It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        DefaultResponse uploadResult = await _service.UploadBuildZipAsync(zipStream, versionGuid, CancellationToken.None)
            .ConfigureAwait(true);
        BuildUploadAnalysis analysis = Assert.IsType<BuildUploadAnalysis>(uploadResult.ObjectResponse);

        _mockMapper.Setup(m => m.Map<GameVersionFile>(It.IsAny<GameVersionFileModel>()))
            .Returns(new GameVersionFile { GuidVersion = versionGuid });

        _mockMapper.Setup(m => m.Map<ROH.Context.File.Entities.GameFile>(It.IsAny<GameVersionFileModel>()))
            .Returns(new ROH.Context.File.Entities.GameFile(Guid: Guid.NewGuid(), Name: "testFile.txt"));

        // Act
        DefaultResponse result = await _service.ConfirmBuildUploadAsync(
            new BuildUploadConfirmation { AnalysisId = analysis.AnalysisId, ReplaceIdentical = false },
            CancellationToken.None)
            .ConfigureAwait(true);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(HttpStatusCode.OK, result.HttpStatus);
        Assert.NotNull(result.ObjectResponse);

        BuildUploadResult confirmResult = Assert.IsType<BuildUploadResult>(result.ObjectResponse);
        Assert.True(confirmResult.Success);
        Assert.Equal(1, confirmResult.Added);

        // Cleanup
        CleanupAnalysisTemp(analysis.AnalysisId);
    }

    /// <summary>
    /// Cleans up temp files created during build upload analysis.
    /// </summary>
    private static void CleanupAnalysisTemp(Guid analysisId)
    {
        string tempRoot = @".\ROHUpdateFiles\_uploadTemp";
        string analysisDir = Path.Combine(tempRoot, analysisId.ToString());
        if (Directory.Exists(analysisDir))
            Directory.Delete(analysisDir, recursive: true);
    }
}
