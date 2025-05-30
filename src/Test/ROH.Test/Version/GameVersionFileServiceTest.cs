﻿//-----------------------------------------------------------------------
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

using System.Net;

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

        _mockGameVersionFileRepository.Setup(repo => repo.GetFileAsync(fileGuid, It.IsAny<CancellationToken>())).ReturnsAsync((GameVersionFile?)null);

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

        _mockGameVersionFileRepository.Setup(repo => repo.GetFileAsync(fileGuid, It.IsAny<CancellationToken>())).ReturnsAsync(versionFile);

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

        _mockGameVersionFileRepository.Setup(repo => repo.GetFileAsync(fileGuid, It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());
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

        _mockGameVersionFileRepository.Setup(repo => repo.GetFileAsync(fileId, It.IsAny<CancellationToken>())).ReturnsAsync((GameVersionFile?)null);

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

        _mockGameVersionFileRepository.Setup(repo => repo.GetFileAsync(fileId, It.IsAny<CancellationToken>())).ReturnsAsync(versionFile);

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

        _mockGameVersionFileRepository.Setup(repo => repo.GetFileAsync(fileId, It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());
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

        _mockGameVersionService.Setup(service => service.VerifyIfVersionExistAsync(versionGuid, It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());

        _mockExceptionHandler.Setup(x => x.HandleException(It.IsAny<Exception>()))
            .Returns(
                new DefaultResponse(httpStatus: HttpStatusCode.InternalServerError, message: "Internal Server Error"));

        // Act
        DefaultResponse result = await _service.GetFilesAsync(versionGuid.ToString(), CancellationToken.None).ConfigureAwait(true);

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

        _mockGameVersionService.Setup(service => service.VerifyIfVersionExistAsync(versionGuid, It.IsAny<CancellationToken>())).ReturnsAsync(true);

        _mockGameVersionFileRepository.Setup(repo => repo.GetFilesAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(files);

        // Act
        DefaultResponse result = await _service.GetFilesAsync(versionGuid.ToString(), CancellationToken.None).ConfigureAwait(true);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(HttpStatusCode.OK, result.HttpStatus);
        Assert.NotNull(result.ObjectResponse);
    }

    [Fact]
    public async Task GetFilesVersionNotExistsReturnsNotFound()
    {
        // Arrange
        Guid versionGuid = Guid.NewGuid();

        _mockGameVersionService.Setup(service => service.VerifyIfVersionExistAsync(versionGuid, It.IsAny<CancellationToken>())).ReturnsAsync(false);

        // Act
        DefaultResponse result = await _service.GetFilesAsync(versionGuid.ToString(), CancellationToken.None).ConfigureAwait(true);

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
            GameVersion = new GameVersionModel() { Guid = testGuid }
        };
        GameVersion gameVersion = new(VersionDate: DateTime.UtcNow.AddDays(1), Guid: testGuid);
        GameVersionFile versionFile = new() { GuidVersion = gameVersion.Guid };

        _mockMapper.Setup(mapper => mapper.Map<GameVersionFile>(fileModel)).Returns(versionFile);

        _mockValidator.Setup(validator => validator.ValidateAsync(fileModel, default))
            .ReturnsAsync(new ValidationResult());

        _mockGameVersionService.Setup(service => service.VerifyIfVersionExistAsync(gameVersion.Guid, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _mockGameVersionService.Setup(service => service.GetCurrentVersionAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(
                new VersionServiceApi.DefaultResponse()
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
            "File Upload Failed: This version has already been released with a yearly schedule. Uploading new files is not allowed for past versions.",
            result.Message);
    }

    [Fact]
    public async Task NewFileShouldHandleException()
    {
        // Arrange
        GameVersionFileModel fileModel = new()
        {
            Content = [1, 2, 3],
            GameVersion = null
        };
        GameVersion gameVersion = new(VersionDate: DateTime.UtcNow.AddDays(1), Guid: Guid.NewGuid());
        GameVersionFile versionFile = new() { GuidVersion = gameVersion.Guid };

        _mockMapper.Setup(mapper => mapper.Map<GameVersionFile>(fileModel)).Returns(versionFile);

        _mockValidator.Setup(validator => validator.ValidateAsync(fileModel, default))
            .ReturnsAsync(new ValidationResult());

        _mockGameVersionService.Setup(service => service.VerifyIfVersionExistAsync(gameVersion.Guid, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        _mockGameVersionService.Setup(service => service.GetCurrentVersionAsync(It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());

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
            GameVersion = new GameVersionModel() { Guid = testGuid, VersionDate = DateTime.UtcNow.AddDays(2) }
        };
        GameVersion gameVersion = new(VersionDate: DateTime.UtcNow.AddDays(1), Guid: testGuid);
        GameVersionFile versionFile = new() { GuidVersion = gameVersion.Guid };

        _mockValidator.Setup(validator => validator.ValidateAsync(fileModel, default))
            .ReturnsAsync(new ValidationResult());

        _mockMapper.Setup(mapper => mapper.Map<GameVersionFile>(fileModel)).Returns(versionFile);

        _mockGameVersionService.Setup(service => service.VerifyIfVersionExistAsync(gameVersion.Guid, It.IsAny<CancellationToken>())).ReturnsAsync(true);

        _mockGameVersionService.Setup(service => service.GetCurrentVersionAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new VersionServiceApi.DefaultResponse() { ObjectResponse = gameVersion.ToJson() });

        _mockMapper.Setup(mapper => mapper.Map<Context.File.Entities.GameFile>(fileModel))
            .Returns(new Context.File.Entities.GameFile());

        _mockGameFileService.Setup(
            service => service.SaveFileAsync(It.IsAny<Context.File.Entities.GameFile>(), fileModel.Content!, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _mockGameVersionFileRepository.Setup(repo => repo.SaveFileAsync(versionFile, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

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
        ValidationResult validationResult = new(
            new List<ValidationFailure> { new("Name", "Name is required.") });

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
            GameVersion = new GameVersionModel() { Guid = Guid.NewGuid() }
        };
        GameVersion gameVersion = new(VersionDate: DateTime.UtcNow.AddDays(1), Guid: Guid.NewGuid());
        GameVersionFile versionFile = new() { GuidVersion = gameVersion.Guid };

        _mockMapper.Setup(mapper => mapper.Map<GameVersionFile>(fileModel)).Returns(versionFile);

        _mockValidator.Setup(validator => validator.ValidateAsync(fileModel, default))
            .ReturnsAsync(new ValidationResult());

        _mockGameVersionService.Setup(service => service.VerifyIfVersionExistAsync(gameVersion.Guid, It.IsAny<CancellationToken>()))
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
            GameVersion = new GameVersionModel() { Guid = testGuid, Released = true }
        };
        GameVersion gameVersion = new(VersionDate: DateTime.UtcNow.AddDays(1), Guid: testGuid) { Released = true };
        GameVersionFile versionFile = new() { GuidVersion = gameVersion.Guid };

        _mockMapper.Setup(mapper => mapper.Map<GameVersionFile>(fileModel)).Returns(versionFile);

        _mockValidator.Setup(validator => validator.ValidateAsync(fileModel, default))
            .ReturnsAsync(new ValidationResult());

        _mockGameVersionService.Setup(service => service.VerifyIfVersionExistAsync(gameVersion.Guid, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _mockGameVersionService.Setup(service => service.GetCurrentVersionAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new VersionServiceApi.DefaultResponse() { ObjectResponse = gameVersion.ToJson() });

        // Act
        DefaultResponse result = await _service.NewFileAsync(fileModel, CancellationToken.None).ConfigureAwait(true);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(HttpStatusCode.BadRequest, result.HttpStatus);
        Assert.Equal(
            "File Upload Failed: This version has already been released. You cannot upload new files for a released version.",
            result.Message);
    }
}
