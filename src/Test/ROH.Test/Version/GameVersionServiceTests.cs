//-----------------------------------------------------------------------
// <copyright file="GameVersionServiceTests.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using AutoMapper;

using Moq;

using ROH.Context.Version.Entities;
using ROH.Context.Version.Interface;
using ROH.Context.Version.Paginator;
using ROH.Service.Exception.Interface;
using ROH.Service.Version;
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
    public async Task GetAllReleasedVersionsShouldHandleException()
    {
        // Arrange
        _mockVersionRepository.Setup(repo => repo.GetAllReleasedVersionsAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception());
        _mockExceptionHandler.Setup(handler => handler.HandleException(It.IsAny<Exception>()))
            .Returns(new DefaultResponse(null, HttpStatusCode.InternalServerError, "Exception occurred"));

        // Act
        DefaultResponse result = await _service.GetAllReleasedVersionsAsync(cancellationToken: CancellationToken.None).ConfigureAwait(true);

        // Assert
        Assert.Equal(HttpStatusCode.InternalServerError, result.HttpStatus);
        Assert.Equal("Exception occurred", result.Message);
    }

    [Fact]
    public async Task GetAllReleasedVersionsShouldReturnReleasedVersions()
    {
        // Arrange
        _mockVersionRepository.Setup(repo => repo.GetAllReleasedVersionsAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_paginatedResult);

        // Act
        DefaultResponse result = await _service.GetAllReleasedVersionsAsync(cancellationToken: CancellationToken.None).ConfigureAwait(true);

        // Assert
        Assert.NotNull(result.ObjectResponse);
        Assert.IsType<PaginatedModel>(result.ObjectResponse);
        Assert.Equal("That are all released versions", result.Message);
    }

    [Fact]
    public async Task GetAllVersionsShouldHandleException()
    {
        // Arrange
        _mockVersionRepository.Setup(repo => repo.GetAllVersionsAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception());
        _mockExceptionHandler.Setup(handler => handler.HandleException(It.IsAny<Exception>()))
            .Returns(new DefaultResponse(null, HttpStatusCode.InternalServerError, "Exception occurred"));

        // Act
        DefaultResponse result = await _service.GetAllVersionsAsync(cancellationToken: CancellationToken.None).ConfigureAwait(true);

        // Assert
        Assert.Equal(HttpStatusCode.InternalServerError, result.HttpStatus);
        Assert.Equal("Exception occurred", result.Message);
    }

    [Fact]
    public async Task GetAllVersionsShouldReturnVersions()
    {
        // Arrange
        _mockVersionRepository.Setup(repo => repo.GetAllVersionsAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_paginatedResult);

        // Act
        DefaultResponse result = await _service.GetAllVersionsAsync(cancellationToken: CancellationToken.None).ConfigureAwait(true);

        // Assert
        Assert.NotNull(result.ObjectResponse);
        Assert.IsType<PaginatedModel>(result.ObjectResponse);
    }

    [Fact]
    public async Task GetCurrentVersionShouldHandleException()
    {
        // Arrange
        _mockVersionRepository.Setup(repo => repo.GetCurrentGameVersionAsync(It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());
        _mockExceptionHandler.Setup(handler => handler.HandleException(It.IsAny<Exception>()))
            .Returns(new DefaultResponse(null, HttpStatusCode.InternalServerError, "Exception occurred"));

        // Act
        DefaultResponse result = await _service.GetCurrentVersionAsync(cancellationToken: CancellationToken.None).ConfigureAwait(true);

        // Assert
        Assert.Equal(HttpStatusCode.InternalServerError, result.HttpStatus);
        Assert.Equal("Exception occurred", result.Message);
    }

    [Fact]
    public async Task GetCurrentVersionShouldReturnCurrentVersion()
    {
        // Arrange
        _mockVersionRepository.Setup(repo => repo.GetCurrentGameVersionAsync(It.IsAny<CancellationToken>())).ReturnsAsync(_gameVersion);

        // Act
        DefaultResponse result = await _service.GetCurrentVersionAsync(cancellationToken: CancellationToken.None).ConfigureAwait(true);

        // Assert
        Assert.NotNull(result.ObjectResponse);
        Assert.IsType<GameVersion>(result.ObjectResponse);
    }

    [Fact]
    public async Task GetVersionByGuidShouldHandleException()
    {
        // Arrange
        _mockVersionRepository.Setup(repo => repo.GetVersionByGuidAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());
        _mockExceptionHandler.Setup(handler => handler.HandleException(It.IsAny<Exception>()))
            .Returns(new DefaultResponse(null, HttpStatusCode.InternalServerError, "Exception occurred"));

        // Act
        DefaultResponse result = await _service.GetVersionByGuidAsync(Guid.NewGuid().ToString(), cancellationToken: CancellationToken.None).ConfigureAwait(true);

        // Assert
        Assert.Equal(HttpStatusCode.InternalServerError, result.HttpStatus);
        Assert.Equal("Exception occurred", result.Message);
    }

    [Fact]
    public async Task GetVersionByGuidShouldReturnExpectationFailedWhenVersionGuidIsInvalid()
    {
        // Act
        DefaultResponse result = await _service.GetVersionByGuidAsync("invalid-guid", cancellationToken: CancellationToken.None).ConfigureAwait(true);

        // Assert
        Assert.Equal(HttpStatusCode.ExpectationFailed, result.HttpStatus);
        Assert.Equal("The Guid is invalid!", result.Message);
    }

    [Fact]
    public async Task GetVersionByGuidShouldReturnVersionWhenFound()
    {
        // Arrange
        _mockVersionRepository.Setup(repo => repo.GetVersionByGuidAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(_gameVersion);

        // Act
        DefaultResponse result = await _service.GetVersionByGuidAsync(_gameVersion.Guid.ToString(), cancellationToken: CancellationToken.None).ConfigureAwait(true);

        // Assert
        Assert.NotNull(result.ObjectResponse);
        Assert.IsType<GameVersion>(result.ObjectResponse);
    }

    [Fact]
    public async Task NewVersionShouldHandleException()
    {
        // Arrange
        _mockVersionRepository.Setup(repo => repo.SetNewGameVersionAsync(It.IsAny<GameVersion>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception());
        _mockExceptionHandler.Setup(handler => handler.HandleException(It.IsAny<Exception>()))
            .Returns(new DefaultResponse(null, HttpStatusCode.InternalServerError, "Exception occurred"));

        // Act
        DefaultResponse result = await _service.NewVersionAsync(_gameVersionModel, cancellationToken: CancellationToken.None).ConfigureAwait(true);

        // Assert
        Assert.Equal(HttpStatusCode.InternalServerError, result.HttpStatus);
        Assert.Equal("Exception occurred", result.Message);
    }

    [Fact]
    public async Task NewVersionShouldReturnConflictWhenVersionAlreadyExists()
    {
        // Arrange
        _mockVersionRepository.Setup(repo => repo.VerifyIfExistAsync(It.IsAny<GameVersion>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);

        // Act
        DefaultResponse result = await _service.NewVersionAsync(_gameVersionModel, cancellationToken: CancellationToken.None).ConfigureAwait(true);

        // Assert
        Assert.Equal(HttpStatusCode.Conflict, result.HttpStatus);
        Assert.Equal("This version already exist.", result.Message);
    }

    [Fact]
    public async Task NewVersionShouldReturnCreatedWhenVersionIsNew()
    {
        // Arrange
        _mockVersionRepository.Setup(repo => repo.VerifyIfExistAsync(It.IsAny<GameVersion>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);

        // Act
        DefaultResponse result = await _service.NewVersionAsync(_gameVersionModel, cancellationToken: CancellationToken.None).ConfigureAwait(true);

        // Assert
        Assert.Equal(HttpStatusCode.Created, result.HttpStatus);
        Assert.Equal("New game version created.", result.Message);
    }

    [Fact]
    public async Task ReleaseVersionShouldHandleException()
    {
        // Arrange
        _mockVersionRepository.Setup(repo => repo.GetVersionByGuidAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(_gameVersion);
        _mockVersionRepository.Setup(repo => repo.UpdateGameVersionAsync(It.IsAny<GameVersion>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception());

        _mockExceptionHandler.Setup(handler => handler.HandleException(It.IsAny<Exception>()))
            .Returns(new DefaultResponse(null, HttpStatusCode.InternalServerError, "Exception occurred"));

        // Act
        DefaultResponse result = await _service.SetReleasedAsync(_gameVersion.Guid.ToString(), cancellationToken: CancellationToken.None).ConfigureAwait(true);

        // Assert
        Assert.Equal(HttpStatusCode.InternalServerError, result.HttpStatus);
        Assert.Equal("Exception occurred", result.Message);
    }

    [Fact]
    public async Task ReleaseVersionShouldReturnNotFoundWhenObjectIsNull()
    {
        // Arrange
        _mockVersionRepository.Setup(repo => repo.GetVersionByGuidAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(() => null);

        // Act
        DefaultResponse result = await _service.SetReleasedAsync(_gameVersion.Guid.ToString(), cancellationToken: CancellationToken.None).ConfigureAwait(true);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, result.HttpStatus);
        Assert.Equal("The version has not found!", result.Message);
    }

    [Fact]
    public async Task SetReleasedShouldReleaseVersionWhenValidGuid()
    {
        // Arrange
        _mockVersionRepository.Setup(repo => repo.GetVersionByGuidAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(_gameVersion);
        _mockVersionRepository.Setup(repo => repo.UpdateGameVersionAsync(It.IsAny<GameVersion>(), It.IsAny<CancellationToken>())).ReturnsAsync(_gameVersion);

        // Act
        DefaultResponse result = await _service.SetReleasedAsync(_gameVersion.Guid.ToString(), cancellationToken: CancellationToken.None).ConfigureAwait(true);

        // Assert
        Assert.Equal(HttpStatusCode.OK, result.HttpStatus);
        Assert.Equal("The version has been set as release.", result.Message);
    }

    [Fact]
    public async Task SetReleasedShouldReturnNotFoundWhenInvalidGuid()
    {
        // Act
        DefaultResponse result = await _service.SetReleasedAsync("invalid-guid", cancellationToken: CancellationToken.None).ConfigureAwait(true);

        // Assert
        Assert.Equal(HttpStatusCode.ExpectationFailed, result.HttpStatus);
        Assert.Equal("The Guid is invalid!", result.Message);
    }

    [Fact]
    public async Task VerifyIfVersionExistShouldReturnFalseWhenVersionDoesNotExist()
    {
        // Arrange
        _mockVersionRepository.Setup(repo => repo.VerifyIfExistAsync(It.IsAny<GameVersion>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);

        // Act
        bool result = await _service.VerifyIfVersionExistAsync(_gameVersionModel, cancellationToken: CancellationToken.None).ConfigureAwait(true);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task VerifyIfVersionExistShouldReturnFalseWhenVersionGuidDoesNotExist()
    {
        // Arrange
        _mockVersionRepository.Setup(repo => repo.VerifyIfExistAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);

        // Act
        bool result = await _service.VerifyIfVersionExistAsync(_gameVersion.Guid.ToString(), cancellationToken: CancellationToken.None).ConfigureAwait(true);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task VerifyIfVersionExistShouldReturnTrueWhenVersionExists()
    {
        // Arrange
        _mockVersionRepository.Setup(repo => repo.VerifyIfExistAsync(It.IsAny<GameVersion>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);

        // Act
        bool result = await _service.VerifyIfVersionExistAsync(_gameVersionModel, cancellationToken: CancellationToken.None).ConfigureAwait(true);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task VerifyIfVersionExistShouldReturnTrueWhenVersionGuidExists()
    {
        // Arrange
        _mockVersionRepository.Setup(repo => repo.VerifyIfExistAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);

        // Act
        bool result = await _service.VerifyIfVersionExistAsync(_gameVersion.Guid.ToString(), cancellationToken: CancellationToken.None).ConfigureAwait(true);

        // Assert
        Assert.True(result);
    }
}
