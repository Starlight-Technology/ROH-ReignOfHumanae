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
    private readonly Mock<IGameVersionFileRepository> _mockGameVersionFileRepository;
    private readonly Mock<IGameFileService> _mockGameFileService;
    private readonly Mock<IValidator<GameVersionFileModel>> _mockValidator;
    private readonly Mock<IGameVersionService> _mockGameVersionService;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IExceptionHandler> _mockExceptionHandler;
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
            _mockExceptionHandler.Object
        );
    }

    [Fact]
    public async Task DownloadFile_ByGuid_ReturnsFileSuccessfully()
    {
        // Arrange
        var fileGuid = Guid.NewGuid();
        var gameFile = new Domain.GameFiles.GameFile { Guid = fileGuid };
        var versionFile = new GameVersionFile { GameFile = gameFile };

        _mockGameVersionFileRepository.Setup(repo => repo.GetFile(fileGuid))
            .ReturnsAsync(versionFile);

        _mockGameFileService.Setup(service => service.DownloadFile(fileGuid))
            .ReturnsAsync(new DefaultResponse(HttpStatusCode.OK));

        // Act
        var result = await _service.DownloadFile(fileGuid);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(HttpStatusCode.OK, result.HttpStatus);
    }

    [Fact]
    public async Task DownloadFile_ByGuid_FileNotFound()
    {
        // Arrange
        var fileGuid = Guid.NewGuid();

        _mockGameVersionFileRepository.Setup(repo => repo.GetFile(fileGuid))
            .ReturnsAsync((GameVersionFile?)null);

        // Act
        var result = await _service.DownloadFile(fileGuid);

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
        var gameFile = new Domain.GameFiles.GameFile { Id = fileId };
        var versionFile = new GameVersionFile { GameFile = gameFile };

        _mockGameVersionFileRepository.Setup(repo => repo.GetFile(fileId))
            .ReturnsAsync(versionFile);

        _mockGameFileService.Setup(service => service.DownloadFile(fileId))
            .ReturnsAsync(new DefaultResponse(HttpStatusCode.OK));

        // Act
        var result = await _service.DownloadFile(fileId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(HttpStatusCode.OK, result.HttpStatus);
    }

    [Fact]
    public async Task DownloadFile_ById_FileNotFound()
    {
        // Arrange
        long fileId = 1;

        _mockGameVersionFileRepository.Setup(repo => repo.GetFile(fileId))
            .ReturnsAsync((GameVersionFile?)null);

        // Act
        var result = await _service.DownloadFile(fileId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(HttpStatusCode.NotFound, result.HttpStatus);
        Assert.Equal("Game Version File Not Found.", result.Message);
    }

    [Fact]
    public async Task GetFiles_VersionExists_ReturnsFiles()
    {
        // Arrange
        var versionGuid = Guid.NewGuid().ToString();
        var files = new List<GameVersionFile> { new() };

        _mockGameVersionService.Setup(service => service.VerifyIfVersionExist(versionGuid))
            .ReturnsAsync(true);

        _mockGameVersionFileRepository.Setup(repo => repo.GetFiles(It.IsAny<Guid>()))
            .ReturnsAsync(files);

        // Act
        var result = await _service.GetFiles(versionGuid);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(HttpStatusCode.OK, result.HttpStatus);
        Assert.NotNull(result.ObjectResponse);
    }

    [Fact]
    public async Task GetFiles_VersionNotExists_ReturnsNotFound()
    {
        // Arrange
        var versionGuid = Guid.NewGuid().ToString();

        _mockGameVersionService.Setup(service => service.VerifyIfVersionExist(versionGuid))
            .ReturnsAsync(false);

        // Act
        var result = await _service.GetFiles(versionGuid);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(HttpStatusCode.NotFound, result.HttpStatus);
    }

    [Fact]
    public async Task NewFile_FileContentIsNull_ReturnsBadRequest()
    {
        // Arrange
        var fileModel = new GameVersionFileModel { Content = null };

        // Act
        var result = await _service.NewFile(fileModel);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(HttpStatusCode.BadRequest, result.HttpStatus);
        Assert.Equal("File content can't be empty.", result.Message);
    }

    [Fact]
    public async Task NewFile_ValidationFails_ReturnsBadRequest()
    {
        // Arrange
        var fileModel = new GameVersionFileModel { Content = [1, 2, 3] };
        var validationResult = new ValidationResult(new List<ValidationFailure> { new("Name", "Name is required.") });

        _mockValidator.Setup(validator => validator.ValidateAsync(fileModel, default))
            .ReturnsAsync(validationResult);

        // Act
        var result = await _service.NewFile(fileModel);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(HttpStatusCode.BadRequest, result.HttpStatus);
        Assert.Equal(validationResult.Errors.ToString(), result.Message);
    }

    [Fact]
    public async Task NewFile_VersionNotFound_ReturnsBadRequest()
    {
        // Arrange
        var fileModel = new GameVersionFileModel { Content = [1, 2, 3], GameVersion = null };
        var gameVersion = new GameVersion(VersionDate: DateTime.Now.AddDays(1));
        var versionFile = new GameVersionFile { GameVersion = gameVersion };

        _mockMapper.Setup(mapper => mapper.Map<GameVersionFile>(fileModel))
                .Returns(versionFile);

        _mockValidator.Setup(validator => validator.ValidateAsync(fileModel, default))
            .ReturnsAsync(new ValidationResult());

        _mockGameVersionService.Setup(service => service.VerifyIfVersionExist(fileModel.GameVersion!))
            .ReturnsAsync(false);

        // Act
        var result = await _service.NewFile(fileModel);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(HttpStatusCode.BadRequest, result.HttpStatus);
        Assert.Equal("Game Version Not Found.", result.Message);
    }

    [Fact]
    public async Task NewFile_SuccessfullySavesFile()
    {
        // Arrange
        var fileModel = new GameVersionFileModel { Content = new byte[] { 1, 2, 3 }, GameVersion = new GameVersionModel() { } };
        var gameVersion = new GameVersion(VersionDate: DateTime.Now.AddDays(1));
        var versionFile = new GameVersionFile { GameVersion = gameVersion };

        _mockValidator.Setup(validator => validator.ValidateAsync(fileModel, default))
            .ReturnsAsync(new ValidationResult());

        _mockMapper.Setup(mapper => mapper.Map<GameVersionFile>(fileModel))
            .Returns(versionFile);

        _mockGameVersionService.Setup(service => service.VerifyIfVersionExist(fileModel.GameVersion))
            .ReturnsAsync(true);

        _mockGameVersionService.Setup(service => service.GetCurrentVersion())
            .ReturnsAsync(new DefaultResponse(objectResponse: gameVersion));

        _mockMapper.Setup(mapper => mapper.Map<Domain.GameFiles.GameFile>(fileModel))
            .Returns(new Domain.GameFiles.GameFile());

        _mockGameFileService.Setup(service => service.SaveFileAsync(It.IsAny<Domain.GameFiles.GameFile>(), fileModel.Content))
            .Returns(Task.CompletedTask);

        _mockGameVersionFileRepository.Setup(repo => repo.SaveFile(versionFile))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _service.NewFile(fileModel);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(HttpStatusCode.OK, result.HttpStatus);
    }
}
