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

using ROH.Domain.Version;
using ROH.Interfaces.Repository.Version;
using ROH.Interfaces.Services.ExceptionService;
using ROH.Interfaces.Services.GameFile;
using ROH.Interfaces.Services.Version;
using ROH.Services.Version;
using ROH.StandardModels.Response;
using ROH.StandardModels.Version;

using System.Net;

namespace ROH.Test.Version;

public class GameVersionFileServiceTests
{
    private readonly Mock<IExceptionHandler> _mockExceptionHandler;
    private readonly Mock<IGameFileService> _mockGameFileService;
    private readonly Mock<IGameVersionFileRepository> _mockGameVersionFileRepository;
    private readonly Mock<IGameVersionService> _mockGameVersionService;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IValidator<GameVersionFileModel>> _mockValidator;
    private readonly GameVersionFileService _service;

    public GameVersionFileServiceTests()
    {
        _mockGameVersionFileRepository = new Mock<IGameVersionFileRepository>();
        _mockGameFileService = new Mock<IGameFileService>();
        _mockValidator = new Mock<IValidator<GameVersionFileModel>>();
        _mockGameVersionService = new Mock<IGameVersionService>();
        _mockMapper = new Mock<IMapper>();
        _mockExceptionHandler = new Mock<IExceptionHandler>();

        _service = new GameVersionFileService(
            _mockGameVersionFileRepository.Object,
            _mockGameFileService.Object,
            _mockValidator.Object,
            _mockGameVersionService.Object,
            _mockMapper.Object,
            _mockExceptionHandler.Object);
    }

    [Fact]
    public async Task DownloadFile_ByGuid_FileNotFound()
    {
        // Arrange
        Guid fileGuid = Guid.NewGuid();

        _mockGameVersionFileRepository.Setup(repo => repo.GetFile(fileGuid, It.IsAny<CancellationToken>())).ReturnsAsync((GameVersionFile?)null);

        // Act
        DefaultResponse result = await _service.DownloadFile(fileGuid);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(HttpStatusCode.NotFound, result.HttpStatus);
        Assert.Equal("Game Version File Not Found.", result.Message);
    }

    [Fact]
    public async Task DownloadFile_ByGuid_ReturnsFileSuccessfully()
    {
        // Arrange
        Guid fileGuid = Guid.NewGuid();
        Domain.GameFiles.GameFile gameFile = new() { Guid = fileGuid };
        GameVersionFile versionFile = new() { GameFile = gameFile };

        _mockGameVersionFileRepository.Setup(repo => repo.GetFile(fileGuid, It.IsAny<CancellationToken>())).ReturnsAsync(versionFile);

        _mockGameFileService.Setup(service => service.DownloadFile(fileGuid))
            .ReturnsAsync(new DefaultResponse(HttpStatusCode.OK));

        // Act
        DefaultResponse result = await _service.DownloadFile(fileGuid);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(HttpStatusCode.OK, result.HttpStatus);
    }

    [Fact]
    public async Task DownloadFile_ByGuid_ShouldHandleException()
    {
        // Arrange
        Guid fileGuid = Guid.NewGuid();

        _mockGameVersionFileRepository.Setup(repo => repo.GetFile(fileGuid, It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());
        _mockExceptionHandler.Setup(x => x.HandleException(It.IsAny<Exception>()))
            .Returns(
                new DefaultResponse(httpStatus: HttpStatusCode.InternalServerError, message: "Internal Server Error"));

        // Act
        DefaultResponse result = await _service.DownloadFile(fileGuid);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(HttpStatusCode.InternalServerError, result.HttpStatus);
        Assert.Equal("Internal Server Error", result.Message);
    }

    [Fact]
    public async Task DownloadFile_ById_FileNotFound()
    {
        // Arrange
        long fileId = 1;

        _mockGameVersionFileRepository.Setup(repo => repo.GetFile(fileId, It.IsAny<CancellationToken>())).ReturnsAsync((GameVersionFile?)null);

        // Act
        DefaultResponse result = await _service.DownloadFile(fileId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(HttpStatusCode.NotFound, result.HttpStatus);
        Assert.Equal("Game Version File Not Found.", result.Message);
    }

    [Fact]
    public async Task DownloadFile_ById_ReturnsFileSuccessfully()
    {
        // Arrange
        long fileId = 1;
        Domain.GameFiles.GameFile gameFile = new() { Id = fileId };
        GameVersionFile versionFile = new() { GameFile = gameFile };

        _mockGameVersionFileRepository.Setup(repo => repo.GetFile(fileId, It.IsAny<CancellationToken>())).ReturnsAsync(versionFile);

        _mockGameFileService.Setup(service => service.DownloadFile(fileId))
            .ReturnsAsync(new DefaultResponse(HttpStatusCode.OK));

        // Act
        DefaultResponse result = await _service.DownloadFile(fileId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(HttpStatusCode.OK, result.HttpStatus);
    }

    [Fact]
    public async Task DownloadFile_ById_ShouldHandleException()
    {
        // Arrange
        long fileId = 1;

        _mockGameVersionFileRepository.Setup(repo => repo.GetFile(fileId, It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());
        _mockExceptionHandler.Setup(x => x.HandleException(It.IsAny<Exception>()))
            .Returns(
                new DefaultResponse(httpStatus: HttpStatusCode.InternalServerError, message: "Internal Server Error"));

        // Act
        DefaultResponse result = await _service.DownloadFile(fileId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(HttpStatusCode.InternalServerError, result.HttpStatus);
        Assert.Equal("Internal Server Error", result.Message);
    }

    [Fact]
    public async Task GetFiles_ShouldHandleException()
    {
        // Arrange
        string versionGuid = Guid.NewGuid().ToString();

        _mockGameVersionService.Setup(service => service.VerifyIfVersionExist(versionGuid)).ThrowsAsync(new Exception());

        _mockExceptionHandler.Setup(x => x.HandleException(It.IsAny<Exception>()))
            .Returns(
                new DefaultResponse(httpStatus: HttpStatusCode.InternalServerError, message: "Internal Server Error"));

        // Act
        DefaultResponse result = await _service.GetFiles(versionGuid);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(HttpStatusCode.InternalServerError, result.HttpStatus);
        Assert.Equal("Internal Server Error", result.Message);
    }

    [Fact]
    public async Task GetFiles_VersionExists_ReturnsFiles()
    {
        // Arrange
        string versionGuid = Guid.NewGuid().ToString();
        List<GameVersionFile> files = [new()];
        List<GameVersionFileModel> filesModel = [new()];

        _mockMapper.Setup(mapper => mapper.Map<List<GameVersionFileModel>>(files)).Returns(filesModel);

        _mockGameVersionService.Setup(service => service.VerifyIfVersionExist(versionGuid)).ReturnsAsync(true);

        _mockGameVersionFileRepository.Setup(repo => repo.GetFiles(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(files);

        // Act
        DefaultResponse result = await _service.GetFiles(versionGuid);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(HttpStatusCode.OK, result.HttpStatus);
        Assert.NotNull(result.ObjectResponse);
    }

    [Fact]
    public async Task GetFiles_VersionNotExists_ReturnsNotFound()
    {
        // Arrange
        string versionGuid = Guid.NewGuid().ToString();

        _mockGameVersionService.Setup(service => service.VerifyIfVersionExist(versionGuid)).ReturnsAsync(false);

        // Act
        DefaultResponse result = await _service.GetFiles(versionGuid);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(HttpStatusCode.NotFound, result.HttpStatus);
    }

    [Fact]
    public async Task NewFile_FileContentIsNull_ReturnsBadRequest()
    {
        // Arrange
        GameVersionFileModel fileModel = new();

        // Act
        DefaultResponse result = await _service.NewFile(fileModel);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(HttpStatusCode.BadRequest, result.HttpStatus);
        Assert.Equal("File content can't be empty.", result.Message);
    }

    [Fact]
    public async Task NewFile_OldVersion_ReturnsBadRequest()
    {
        // Arrange
        GameVersionFileModel fileModel = new()
        {
            Content = [1, 2, 3],
            GameVersion = null
        };
        GameVersion gameVersion = new(VersionDate: DateTime.Now.AddDays(1));
        GameVersionFile versionFile = new() { GameVersion = gameVersion };

        _mockMapper.Setup(mapper => mapper.Map<GameVersionFile>(fileModel)).Returns(versionFile);

        _mockValidator.Setup(validator => validator.ValidateAsync(fileModel, default))
            .ReturnsAsync(new ValidationResult());

        _mockGameVersionService.Setup(service => service.VerifyIfVersionExist(fileModel.GameVersion!))
            .ReturnsAsync(true);
        _mockGameVersionService.Setup(service => service.GetCurrentVersion())
            .ReturnsAsync(
                new DefaultResponse(
                    objectResponse: new GameVersion(VersionDate: DateTime.Now.AddDays(2)) { Released = true }));

        // Act
        DefaultResponse result = await _service.NewFile(fileModel);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(HttpStatusCode.BadRequest, result.HttpStatus);
        Assert.Equal(
            "File Upload Failed: This version has already been released with a yearly schedule. Uploading new files is not allowed for past versions.",
            result.Message);
    }

    [Fact]
    public async Task NewFile_ShouldHandleException()
    {
        // Arrange
        GameVersionFileModel fileModel = new()
        {
            Content = [1, 2, 3],
            GameVersion = null
        };
        GameVersion gameVersion = new(VersionDate: DateTime.Now.AddDays(1));
        GameVersionFile versionFile = new() { GameVersion = gameVersion };

        _mockMapper.Setup(mapper => mapper.Map<GameVersionFile>(fileModel)).Returns(versionFile);

        _mockValidator.Setup(validator => validator.ValidateAsync(fileModel, default))
            .ReturnsAsync(new ValidationResult());

        _mockGameVersionService.Setup(service => service.VerifyIfVersionExist(fileModel.GameVersion!))
            .ReturnsAsync(true);
        _mockGameVersionService.Setup(service => service.GetCurrentVersion()).ThrowsAsync(new Exception());

        _mockExceptionHandler.Setup(x => x.HandleException(It.IsAny<Exception>()))
            .Returns(
                new DefaultResponse(httpStatus: HttpStatusCode.InternalServerError, message: "Internal Server Error"));

        // Act
        DefaultResponse result = await _service.NewFile(fileModel);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(HttpStatusCode.InternalServerError, result.HttpStatus);
        Assert.Equal("Internal Server Error", result.Message);
    }

    [Fact]
    public async Task NewFile_SuccessfullySavesFile()
    {
        // Arrange
        GameVersionFileModel fileModel = new()
        {
            Content = [1, 2, 3],
            GameVersion = new GameVersionModel { }
        };
        GameVersion gameVersion = new(VersionDate: DateTime.Now.AddDays(1));
        GameVersionFile versionFile = new() { GameVersion = gameVersion };

        _mockValidator.Setup(validator => validator.ValidateAsync(fileModel, default))
            .ReturnsAsync(new ValidationResult());

        _mockMapper.Setup(mapper => mapper.Map<GameVersionFile>(fileModel)).Returns(versionFile);

        _mockGameVersionService.Setup(service => service.VerifyIfVersionExist(fileModel.GameVersion!)).ReturnsAsync(true);

        _mockGameVersionService.Setup(service => service.GetCurrentVersion())
            .ReturnsAsync(new DefaultResponse(objectResponse: gameVersion));

        _mockMapper.Setup(mapper => mapper.Map<Domain.GameFiles.GameFile>(fileModel))
            .Returns(new Domain.GameFiles.GameFile());

        _mockGameFileService.Setup(
            service => service.SaveFileAsync(It.IsAny<Domain.GameFiles.GameFile>(), fileModel.Content!))
            .Returns(Task.CompletedTask);

        _mockGameVersionFileRepository.Setup(repo => repo.SaveFile(versionFile, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        // Act
        DefaultResponse result = await _service.NewFile(fileModel);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(HttpStatusCode.OK, result.HttpStatus);
    }

    [Fact]
    public async Task NewFile_ValidationFails_ReturnsBadRequest()
    {
        // Arrange
        GameVersionFileModel fileModel = new() { Content = [1, 2, 3] };
        ValidationResult validationResult = new(
            new List<ValidationFailure> { new("Name", "Name is required.") });

        _mockValidator.Setup(validator => validator.ValidateAsync(fileModel, default)).ReturnsAsync(validationResult);

        // Act
        DefaultResponse result = await _service.NewFile(fileModel);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(HttpStatusCode.BadRequest, result.HttpStatus);
        Assert.Equal(validationResult.Errors.ToString(), result.Message);
    }

    [Fact]
    public async Task NewFile_VersionNotFound_ReturnsBadRequest()
    {
        // Arrange
        GameVersionFileModel fileModel = new()
        {
            Content = [1, 2, 3],
            GameVersion = null
        };
        GameVersion gameVersion = new(VersionDate: DateTime.Now.AddDays(1));
        GameVersionFile versionFile = new() { GameVersion = gameVersion };

        _mockMapper.Setup(mapper => mapper.Map<GameVersionFile>(fileModel)).Returns(versionFile);

        _mockValidator.Setup(validator => validator.ValidateAsync(fileModel, default))
            .ReturnsAsync(new ValidationResult());

        _mockGameVersionService.Setup(service => service.VerifyIfVersionExist(fileModel.GameVersion!))
            .ReturnsAsync(false);

        // Act
        DefaultResponse result = await _service.NewFile(fileModel);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(HttpStatusCode.BadRequest, result.HttpStatus);
        Assert.Equal("Game Version Not Found.", result.Message);
    }

    [Fact]
    public async Task NewFile_VersionReleased_ReturnsBadRequest()
    {
        // Arrange
        GameVersionFileModel fileModel = new()
        {
            Content = [1, 2, 3],
            GameVersion = null
        };
        GameVersion gameVersion = new(VersionDate: DateTime.Now.AddDays(1)) { Released = true };
        GameVersionFile versionFile = new() { GameVersion = gameVersion };

        _mockMapper.Setup(mapper => mapper.Map<GameVersionFile>(fileModel)).Returns(versionFile);

        _mockValidator.Setup(validator => validator.ValidateAsync(fileModel, default))
            .ReturnsAsync(new ValidationResult());

        _mockGameVersionService.Setup(service => service.VerifyIfVersionExist(fileModel.GameVersion!))
            .ReturnsAsync(true);

        // Act
        DefaultResponse result = await _service.NewFile(fileModel);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(HttpStatusCode.BadRequest, result.HttpStatus);
        Assert.Equal(
            "File Upload Failed: This version has already been released. You cannot upload new files for a released version.",
            result.Message);
    }
}
