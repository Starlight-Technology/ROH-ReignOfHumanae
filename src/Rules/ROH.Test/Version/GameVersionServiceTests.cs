//-----------------------------------------------------------------------
// <copyright file="GameVersionServiceTests.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using AutoMapper;

using Moq;

using ROH.Domain.Paginator;
using ROH.Domain.Version;
using ROH.Interfaces.Repository.Version;
using ROH.Interfaces.Services.ExceptionService;
using ROH.Services.Version;
using ROH.StandardModels.Paginator;
using ROH.StandardModels.Response;
using ROH.StandardModels.Version;

using System.Net;

namespace ROH.Test.Version;

public class GameVersionServiceTests
{
    private readonly GameVersion _gameVersion;
    private readonly GameVersionModel _gameVersionModel;
    private readonly Mock<IExceptionHandler> _mockExceptionHandler;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IGameVersionRepository> _mockVersionRepository;
    private readonly Paginated _paginatedResult;
    private readonly GameVersionService _service;

    public GameVersionServiceTests()
    {
        _mockVersionRepository = new Mock<IGameVersionRepository>();
        _mockMapper = new Mock<IMapper>();
        _mockExceptionHandler = new Mock<IExceptionHandler>();
        _service = new GameVersionService(
            _mockVersionRepository.Object,
            _mockMapper.Object,
            _mockExceptionHandler.Object);

        _gameVersion = new GameVersion(VersionDate: DateTime.Today, Guid: Guid.NewGuid());
        _gameVersionModel = new GameVersionModel { Guid = _gameVersion.Guid };

        _paginatedResult = new Paginated(1, new List<object> { _gameVersion });

        _mockMapper.Setup(m => m.Map<IList<GameVersionModel>>(It.IsAny<IList<GameVersion>>()))
            .Returns([_gameVersionModel]);
    }

    [Fact]
    public async Task GetAllReleasedVersions_ShouldHandle_Exception()
    {
        // Arrange
        _mockVersionRepository.Setup(repo => repo.GetAllReleasedVersionsAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception());
        _mockExceptionHandler.Setup(handler => handler.HandleException(It.IsAny<Exception>()))
            .Returns(new DefaultResponse(null, HttpStatusCode.InternalServerError, "Exception occurred"));

        // Act
        DefaultResponse result = await _service.GetAllReleasedVersions();

        // Assert
        Assert.Equal(HttpStatusCode.InternalServerError, result.HttpStatus);
        Assert.Equal("Exception occurred", result.Message);
    }

    [Fact]
    public async Task GetAllReleasedVersions_ShouldReturn_ReleasedVersions()
    {
        // Arrange
        _mockVersionRepository.Setup(repo => repo.GetAllReleasedVersionsAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_paginatedResult);

        // Act
        DefaultResponse result = await _service.GetAllReleasedVersions();

        // Assert
        Assert.NotNull(result.ObjectResponse);
        Assert.IsType<PaginatedModel>(result.ObjectResponse);
        Assert.Equal("That are all released versions", result.Message);
    }

    [Fact]
    public async Task GetAllVersions_ShouldHandle_Exception()
    {
        // Arrange
        _mockVersionRepository.Setup(repo => repo.GetAllVersionsAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception());
        _mockExceptionHandler.Setup(handler => handler.HandleException(It.IsAny<Exception>()))
            .Returns(new DefaultResponse(null, HttpStatusCode.InternalServerError, "Exception occurred"));

        // Act
        DefaultResponse result = await _service.GetAllVersions();

        // Assert
        Assert.Equal(HttpStatusCode.InternalServerError, result.HttpStatus);
        Assert.Equal("Exception occurred", result.Message);
    }

    [Fact]
    public async Task GetAllVersions_ShouldReturn_Versions()
    {
        // Arrange
        _mockVersionRepository.Setup(repo => repo.GetAllVersionsAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_paginatedResult);

        // Act
        DefaultResponse result = await _service.GetAllVersions();

        // Assert
        Assert.NotNull(result.ObjectResponse);
        Assert.IsType<PaginatedModel>(result.ObjectResponse);
    }

    [Fact]
    public async Task GetCurrentVersion_ShouldHandle_Exception()
    {
        // Arrange
        _mockVersionRepository.Setup(repo => repo.GetCurrentGameVersionAsync(It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());
        _mockExceptionHandler.Setup(handler => handler.HandleException(It.IsAny<Exception>()))
            .Returns(new DefaultResponse(null, HttpStatusCode.InternalServerError, "Exception occurred"));

        // Act
        DefaultResponse result = await _service.GetCurrentVersion();

        // Assert
        Assert.Equal(HttpStatusCode.InternalServerError, result.HttpStatus);
        Assert.Equal("Exception occurred", result.Message);
    }

    [Fact]
    public async Task GetCurrentVersion_ShouldReturn_CurrentVersion()
    {
        // Arrange
        _mockVersionRepository.Setup(repo => repo.GetCurrentGameVersionAsync(It.IsAny<CancellationToken>())).ReturnsAsync(_gameVersion);

        // Act
        DefaultResponse result = await _service.GetCurrentVersion();

        // Assert
        Assert.NotNull(result.ObjectResponse);
        Assert.IsType<GameVersion>(result.ObjectResponse);
    }

    [Fact]
    public async Task GetVersionByGuid_ShouldHandle_Exception()
    {
        // Arrange
        _mockVersionRepository.Setup(repo => repo.GetVersionByGuidAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());
        _mockExceptionHandler.Setup(handler => handler.HandleException(It.IsAny<Exception>()))
            .Returns(new DefaultResponse(null, HttpStatusCode.InternalServerError, "Exception occurred"));

        // Act
        DefaultResponse result = await _service.GetVersionByGuid(Guid.NewGuid().ToString());

        // Assert
        Assert.Equal(HttpStatusCode.InternalServerError, result.HttpStatus);
        Assert.Equal("Exception occurred", result.Message);
    }

    [Fact]
    public async Task GetVersionByGuid_ShouldReturn_ExpectationFailed_WhenVersionGuidIsInvalid()
    {
        // Act
        DefaultResponse result = await _service.GetVersionByGuid("invalid-guid");

        // Assert
        Assert.Equal(HttpStatusCode.ExpectationFailed, result.HttpStatus);
        Assert.Equal("The Guid is invalid!", result.Message);
    }

    [Fact]
    public async Task GetVersionByGuid_ShouldReturn_Version_WhenFound()
    {
        // Arrange
        _mockVersionRepository.Setup(repo => repo.GetVersionByGuidAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(_gameVersion);

        // Act
        DefaultResponse result = await _service.GetVersionByGuid(_gameVersion.Guid.ToString());

        // Assert
        Assert.NotNull(result.ObjectResponse);
        Assert.IsType<GameVersion>(result.ObjectResponse);
    }

    [Fact]
    public async Task NewVersion_ShouldHandle_Exception()
    {
        // Arrange
        _mockVersionRepository.Setup(repo => repo.SetNewGameVersionAsync(It.IsAny<GameVersion>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception());
        _mockExceptionHandler.Setup(handler => handler.HandleException(It.IsAny<Exception>()))
            .Returns(new DefaultResponse(null, HttpStatusCode.InternalServerError, "Exception occurred"));

        // Act
        DefaultResponse result = await _service.NewVersion(_gameVersionModel);

        // Assert
        Assert.Equal(HttpStatusCode.InternalServerError, result.HttpStatus);
        Assert.Equal("Exception occurred", result.Message);
    }

    [Fact]
    public async Task NewVersion_ShouldReturn_Conflict_WhenVersionAlreadyExists()
    {
        // Arrange
        _mockVersionRepository.Setup(repo => repo.VerifyIfExistAsync(It.IsAny<GameVersion>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);

        // Act
        DefaultResponse result = await _service.NewVersion(_gameVersionModel);

        // Assert
        Assert.Equal(HttpStatusCode.Conflict, result.HttpStatus);
        Assert.Equal("This version already exist.", result.Message);
    }

    [Fact]
    public async Task NewVersion_ShouldReturn_Created_WhenVersionIsNew()
    {
        // Arrange
        _mockVersionRepository.Setup(repo => repo.VerifyIfExistAsync(It.IsAny<GameVersion>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);

        // Act
        DefaultResponse result = await _service.NewVersion(_gameVersionModel);

        // Assert
        Assert.Equal(HttpStatusCode.Created, result.HttpStatus);
        Assert.Equal("New game version created.", result.Message);
    }

    [Fact]
    public async Task ReleaseVersion_ShouldHandle_Exception()
    {
        // Arrange
        _mockVersionRepository.Setup(repo => repo.GetVersionByGuidAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(_gameVersion);
        _mockVersionRepository.Setup(repo => repo.UpdateGameVersionAsync(It.IsAny<GameVersion>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception());

        _mockExceptionHandler.Setup(handler => handler.HandleException(It.IsAny<Exception>()))
            .Returns(new DefaultResponse(null, HttpStatusCode.InternalServerError, "Exception occurred"));

        // Act
        DefaultResponse result = await _service.SetReleased(_gameVersion.Guid.ToString());

        // Assert
        Assert.Equal(HttpStatusCode.InternalServerError, result.HttpStatus);
        Assert.Equal("Exception occurred", result.Message);
    }

    [Fact]
    public async Task ReleaseVersion_ShouldReturn_NotFound_WhenObjectIsNull()
    {
        // Arrange
        _mockVersionRepository.Setup(repo => repo.GetVersionByGuidAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(() => null);

        // Act
        DefaultResponse result = await _service.SetReleased(_gameVersion.Guid.ToString());

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, result.HttpStatus);
        Assert.Equal("The version has not found!", result.Message);
    }

    [Fact]
    public async Task SetReleased_ShouldRelease_Version_WhenValidGuid()
    {
        // Arrange
        _mockVersionRepository.Setup(repo => repo.GetVersionByGuidAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(_gameVersion);
        _mockVersionRepository.Setup(repo => repo.UpdateGameVersionAsync(It.IsAny<GameVersion>(), It.IsAny<CancellationToken>())).ReturnsAsync(_gameVersion);

        // Act
        DefaultResponse result = await _service.SetReleased(_gameVersion.Guid.ToString());

        // Assert
        Assert.Equal(HttpStatusCode.OK, result.HttpStatus);
        Assert.Equal("The version has been set as release.", result.Message);
    }

    [Fact]
    public async Task SetReleased_ShouldReturn_NotFound_WhenInvalidGuid()
    {
        // Act
        DefaultResponse result = await _service.SetReleased("invalid-guid");

        // Assert
        Assert.Equal(HttpStatusCode.ExpectationFailed, result.HttpStatus);
        Assert.Equal("The Guid is invalid!", result.Message);
    }

    [Fact]
    public async Task VerifyIfVersionExist_ShouldReturn_False_WhenVersionDoesNotExist()
    {
        // Arrange
        _mockVersionRepository.Setup(repo => repo.VerifyIfExistAsync(It.IsAny<GameVersion>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);

        // Act
        bool result = await _service.VerifyIfVersionExist(_gameVersionModel);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task VerifyIfVersionExist_ShouldReturn_False_WhenVersionGuidDoesNotExist()
    {
        // Arrange
        _mockVersionRepository.Setup(repo => repo.VerifyIfExistAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);

        // Act
        bool result = await _service.VerifyIfVersionExist(_gameVersion.Guid.ToString());

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task VerifyIfVersionExist_ShouldReturn_True_WhenVersionExists()
    {
        // Arrange
        _mockVersionRepository.Setup(repo => repo.VerifyIfExistAsync(It.IsAny<GameVersion>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);

        // Act
        bool result = await _service.VerifyIfVersionExist(_gameVersionModel);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task VerifyIfVersionExist_ShouldReturn_True_WhenVersionGuidExists()
    {
        // Arrange
        _mockVersionRepository.Setup(repo => repo.VerifyIfExistAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);

        // Act
        bool result = await _service.VerifyIfVersionExist(_gameVersion.Guid.ToString());

        // Assert
        Assert.True(result);
    }
}
