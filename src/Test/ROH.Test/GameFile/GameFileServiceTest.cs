//-----------------------------------------------------------------------
// <copyright file="GameFileServiceTest.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using Moq;

using ROH.Context.File.Interface;
using ROH.Service.Exception.Interface;
using ROH.Service.File;
using ROH.StandardModels.File;
using ROH.StandardModels.Response;

using System.Net;
using System.Text;

namespace ROH.Test.GameFile;

public class GameFileServiceTest
{
    readonly Mock<IExceptionHandler> _mockExceptionHandler;
    readonly Mock<IGameFileRepository> _mockRepository;
    readonly GameFileService _service;
    readonly Context.File.Entities.GameFile _testFile = new(
        Name: "testFile.txt",
        Format: "txt",
        Path: Path.GetTempPath(),
        Guid: Guid.NewGuid());
    readonly Guid _testGuid = Guid.NewGuid();

    public GameFileServiceTest()
    {
        _mockRepository = new Mock<IGameFileRepository>();
        _mockExceptionHandler = new Mock<IExceptionHandler>();
        _service = new GameFileService(_mockRepository.Object, _mockExceptionHandler.Object);
    }

    [Fact]
    public async Task DownloadFileByGuidShouldHandleExceptions()
    {
        // Arrange
        _mockRepository.Setup(repo => repo.GetFileAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Repository exception"));

        _mockExceptionHandler.Setup(handler => handler.HandleException(It.IsAny<Exception>()))
            .Returns(new DefaultResponse(null, HttpStatusCode.InternalServerError, "Internal server error"));

        // Act
        DefaultResponse result = await _service.DownloadFileAsync(_testGuid, CancellationToken.None)
            .ConfigureAwait(true);

        // Assert
        Assert.Equal(HttpStatusCode.InternalServerError, result.HttpStatus);
        Assert.Equal("Internal server error", result.Message);
    }

    [Fact]
    public async Task DownloadFileByGuidShouldReturnFileContentWhenFileExists()
    {
        // Arrange
        _mockRepository.Setup(repo => repo.GetFileAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_testFile);

        string filePath = Path.Combine(_testFile.Path, _testFile.Name);
        await File.WriteAllTextAsync(filePath, "Test content", cancellationToken: CancellationToken.None)
            .ConfigureAwait(true);

        // Act
        DefaultResponse result = await _service.DownloadFileAsync(_testFile.Guid, CancellationToken.None)
            .ConfigureAwait(true);

        // Assert
        Assert.Equal(HttpStatusCode.OK, result.HttpStatus);
        Assert.NotNull(result.ObjectResponse);
        GameFileModel fileModel = Assert.IsType<GameFileModel>(result.ObjectResponse);
        Assert.Equal("testFile.txt", fileModel.Name);
        Assert.Equal("txt", fileModel.Format);
        Assert.Equal(
            await File.ReadAllBytesAsync(filePath, cancellationToken: CancellationToken.None).ConfigureAwait(true),
            fileModel.Content);

        // Cleanup
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }

    [Fact]
    public async Task DownloadFileByGuidShouldReturnFileNotFoundWhenFileDoesNotExist()
    {
        // Arrange
        _mockRepository.Setup(repo => repo.GetFileAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Context.File.Entities.GameFile?)null);

        // Act
        DefaultResponse result = await _service.DownloadFileAsync(_testGuid, CancellationToken.None)
            .ConfigureAwait(true);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, result.HttpStatus);
        Assert.Equal("File Not Found.", result.Message);
    }

    [Fact]
    public async Task DownloadFileByIdShouldReturnFileContentWhenFileExists()
    {
        // Arrange
        _mockRepository.Setup(repo => repo.GetFileAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_testFile);

        string filePath = Path.Combine(_testFile.Path, _testFile.Name);
        await File.WriteAllTextAsync(filePath, "Test content", cancellationToken: CancellationToken.None)
            .ConfigureAwait(true);

        // Act
        DefaultResponse result = await _service.DownloadFileAsync(1, CancellationToken.None).ConfigureAwait(true);

        // Assert
        Assert.Equal(HttpStatusCode.OK, result.HttpStatus);
        Assert.NotNull(result.ObjectResponse);
        GameFileModel fileModel = Assert.IsType<GameFileModel>(result.ObjectResponse);
        Assert.Equal("testFile.txt", fileModel.Name);
        Assert.Equal("txt", fileModel.Format);
        Assert.Equal(
            await File.ReadAllBytesAsync(filePath, cancellationToken: CancellationToken.None).ConfigureAwait(true),
            fileModel.Content);

        // Cleanup
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }

    [Fact]
    public async Task DownloadFileByIdShouldReturnFileNotFoundWhenFileDoesNotExist()
    {
        // Arrange
        _mockRepository.Setup(repo => repo.GetFileAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Context.File.Entities.GameFile?)null);

        // Act
        DefaultResponse result = await _service.DownloadFileAsync(1, CancellationToken.None).ConfigureAwait(true);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, result.HttpStatus);
        Assert.Equal("File Not Found.", result.Message);
    }

    [Fact]
    public async Task DownloadFileShouldReturnErrorResponseWhenExceptionIsThrown()
    {
        // Arrange
        long fileId = 1;
        Exception testException = new("Test exception");

        _mockRepository.Setup(repo => repo.GetFileAsync(fileId, It.IsAny<CancellationToken>()))
            .ThrowsAsync(testException);

        // Mock the exception handler to return a specific response
        DefaultResponse expectedResponse = new(
            httpStatus: HttpStatusCode.InternalServerError,
            message: "An error has occurred.");
        _mockExceptionHandler.Setup(handler => handler.HandleException(testException)).Returns(expectedResponse);

        // Act
        DefaultResponse result = await _service.DownloadFileAsync(fileId, CancellationToken.None).ConfigureAwait(true);

        // Assert
        Assert.Equal(HttpStatusCode.InternalServerError, result.HttpStatus);
        Assert.Equal("An error has occurred.", result.Message);

        // Verify that the exception handler was called with the test exception
        _mockExceptionHandler.Verify(handler => handler.HandleException(testException), Times.Once);
    }

    [Fact]
    public async Task GetGameFileShouldHandleExceptionWhenReadingFileFails()
    {
        // Arrange
        Context.File.Entities.GameFile testFile = _testFile with
        {
            Path = string.Empty,
            Name = string.Empty,
            Format = string.Empty
        };

        _mockRepository.Setup(repo => repo.GetFileAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(testFile);

        _mockExceptionHandler.Setup(handler => handler.HandleException(It.IsAny<Exception>()))
            .Returns(new DefaultResponse(null, HttpStatusCode.InternalServerError, "File read error"));

        // Act
        DefaultResponse result = await _service.DownloadFileAsync(_testGuid, CancellationToken.None)
            .ConfigureAwait(true);

        // Assert
        Assert.Equal(HttpStatusCode.InternalServerError, result.HttpStatus);
        Assert.Equal("File read error", result.Message);
    }

    [Fact]
    public async Task GetGameFileShouldReturnNotFoundWhenFileDoesNotExist()
    {
        // Arrange
        _mockRepository.Setup(repo => repo.GetFileAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_testFile with { Path = string.Empty });

        // Act
        DefaultResponse result = await _service.DownloadFileAsync(1, CancellationToken.None).ConfigureAwait(true);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, result.HttpStatus);
        Assert.Equal("File not found.", result.Message);
    }

    [Fact]
    public async Task MakeFileHasDeprecatedAsyncShouldHandleExceptionWhenExceptionOccurs()
    {
        // Arrange
        _mockRepository.Setup(repo => repo.GetFileAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Error message"));

        _mockExceptionHandler.Setup(handler => handler.HandleException(It.IsAny<Exception>()))
            .Returns(new DefaultResponse(null, HttpStatusCode.InternalServerError, "Error message"));

        // Act
        DefaultResponse result = await _service.MakeFileHasDeprecatedAsync(_testGuid, CancellationToken.None)
            .ConfigureAwait(true);

        // Assert
        Assert.Equal(HttpStatusCode.InternalServerError, result.HttpStatus);
        Assert.Equal("Error message", result.Message);
    }

    [Fact]
    public async Task MakeFileHasDeprecatedAsyncShouldReturnNotFoundWhenFileDoesNotExist()
    {
        //  Arrange
        _mockRepository.Setup(repo => repo.GetFileAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Context.File.Entities.GameFile?)null);

        // Act
        DefaultResponse result = await _service.MakeFileHasDeprecatedAsync(_testGuid, CancellationToken.None)
            .ConfigureAwait(true);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, result.HttpStatus);
        Assert.Equal("File not found.", result.Message);
    }

    [Fact]
    public async Task MakeFileHasDeprecatedAsyncShouldReturnSuccessMessageWhenFileHasMarkedAsDeprecated()
    {
        // Arrange
        _mockRepository.Setup(repo => repo.GetFileAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_testFile with { Active = true });

        _mockRepository.Setup(
            repo => repo.UpdateFileAsync(It.IsAny<Context.File.Entities.GameFile>(), It.IsAny<CancellationToken>()));

        // Act
        DefaultResponse result = await _service.MakeFileHasDeprecatedAsync(_testGuid, CancellationToken.None)
            .ConfigureAwait(true);

        // Assert
        Assert.Equal(HttpStatusCode.OK, result.HttpStatus);
        Assert.Equal($"The file \"{_testFile.Name}\" of version has marked as deprecated.", result.Message);
    }

    [Fact]
    public async Task SaveFileAsyncShouldDeleteFileWhenItAlreadyExists()
    {
        // Arrange
        byte[] content = Encoding.UTF8.GetBytes("New content");
        string filePath = Path.Combine(_testFile.Path, _testFile.Name);

        // Pre-create the file to test deletion
        await File.WriteAllTextAsync(filePath, "Old content", cancellationToken: CancellationToken.None)
            .ConfigureAwait(true);

        // Act
        await _service.SaveFileAsync(_testFile, content, CancellationToken.None).ConfigureAwait(true);

        // Assert
        Assert.True(File.Exists(filePath));
        Assert.Equal(
            "New content",
            await File.ReadAllTextAsync(filePath, cancellationToken: CancellationToken.None).ConfigureAwait(true));
        _mockRepository.Verify(
            repo => repo.SaveFileAsync(It.IsAny<Context.File.Entities.GameFile>(), It.IsAny<CancellationToken>()),
            Times.Once);

        // Cleanup
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }

    [Fact]
    public async Task SaveFileAsyncShouldHandleExceptions()
    {
        // Arrange
        _mockRepository.Setup(
            repo => repo.SaveFileAsync(It.IsAny<Context.File.Entities.GameFile>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Repository exception"));

        _mockExceptionHandler.Setup(handler => handler.HandleException(It.IsAny<Exception>()))
            .Returns(new DefaultResponse(null, HttpStatusCode.InternalServerError, "Internal server error"));

        byte[] content = Encoding.UTF8.GetBytes("Test content");
        string filePath = Path.Combine(_testFile.Path, _testFile.Name);

        // Act
        await _service.SaveFileAsync(_testFile, content, CancellationToken.None).ConfigureAwait(true);

        // Assert
        _mockExceptionHandler.Verify(handler => handler.HandleException(It.IsAny<Exception>()), Times.Once);

        // Cleanup
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }

    [Fact]
    public async Task SaveFileAsyncShouldSaveFileWhenCalled()
    {
        // Arrange
        byte[] content = Encoding.UTF8.GetBytes("Test content");
        string filePath = Path.Combine(_testFile.Path, _testFile.Name);

        File.Delete(filePath);

        // Act
        await _service.SaveFileAsync(_testFile, content, CancellationToken.None).ConfigureAwait(true);

        // Assert
        Assert.True(File.Exists(filePath));
        Assert.Equal(
            "Test content",
            await File.ReadAllTextAsync(filePath, cancellationToken: CancellationToken.None).ConfigureAwait(true));
        _mockRepository.Verify(
            repo => repo.SaveFileAsync(It.IsAny<Context.File.Entities.GameFile>(), It.IsAny<CancellationToken>()),
            Times.Once);

        // Cleanup
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }
}