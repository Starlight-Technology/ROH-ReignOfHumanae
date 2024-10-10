//-----------------------------------------------------------------------
// <copyright file="GameFileServiceTest.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using Moq;

using ROH.Interfaces.Repository.GameFile;
using ROH.Interfaces.Services.ExceptionService;
using ROH.Services.GameFile;
using ROH.StandardModels.File;
using ROH.StandardModels.Response;

using System.Net;
using System.Text;

namespace ROH.Test.GameFile;

public class GameFileServiceTest
{
    private readonly Mock<IExceptionHandler> _mockExceptionHandler;
    private readonly Mock<IGameFileRepository> _mockRepository;
    private readonly GameFileService _service;

    private readonly Domain.GameFiles.GameFile _testFile = new(
        Name: "testFile.txt",
        Format: "txt",
        Path: Path.GetTempPath(),
        Guid: Guid.NewGuid());

    private readonly Guid _testGuid = Guid.NewGuid();

    public GameFileServiceTest()
    {
        _mockRepository = new Mock<IGameFileRepository>();
        _mockExceptionHandler = new Mock<IExceptionHandler>();
        _service = new GameFileService(_mockRepository.Object, _mockExceptionHandler.Object);
    }

    [Fact]
    public async Task DownloadFile_ByGuid_ShouldHandle_Exceptions()
    {
        // Arrange
        _mockRepository.Setup(repo => repo.GetFileAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Repository exception"));

        _mockExceptionHandler.Setup(handler => handler.HandleException(It.IsAny<Exception>()))
            .Returns(new DefaultResponse(null, HttpStatusCode.InternalServerError, "Internal server error"));

        // Act
        DefaultResponse result = await _service.DownloadFile(_testGuid);

        // Assert
        Assert.Equal(HttpStatusCode.InternalServerError, result.HttpStatus);
        Assert.Equal("Internal server error", result.Message);
    }

    [Fact]
    public async Task DownloadFile_ByGuid_ShouldReturn_FileContent_WhenFileExists()
    {
        // Arrange
        _mockRepository.Setup(repo => repo.GetFileAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(_testFile);

        string filePath = Path.Combine(_testFile.Path, _testFile.Name);
        await File.WriteAllTextAsync(filePath, "Test content");

        // Act
        DefaultResponse result = await _service.DownloadFile(_testFile.Guid);

        // Assert
        Assert.Equal(HttpStatusCode.OK, result.HttpStatus);
        Assert.NotNull(result.ObjectResponse);
        GameFileModel fileModel = Assert.IsType<GameFileModel>(result.ObjectResponse);
        Assert.Equal("testFile.txt", fileModel.Name);
        Assert.Equal("txt", fileModel.Format);
        Assert.Equal(await File.ReadAllBytesAsync(filePath), fileModel.Content);

        // Cleanup
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }

    [Fact]
    public async Task DownloadFile_ByGuid_ShouldReturn_FileNotFound_WhenFileDoesNotExist()
    {
        // Arrange
        _mockRepository.Setup(repo => repo.GetFileAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Domain.GameFiles.GameFile?)null);

        // Act
        DefaultResponse result = await _service.DownloadFile(_testGuid);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, result.HttpStatus);
        Assert.Equal("File Not Found.", result.Message);
    }

    [Fact]
    public async Task DownloadFile_ById_ShouldReturn_FileContent_WhenFileExists()
    {
        // Arrange
        _mockRepository.Setup(repo => repo.GetFileAsync(It.IsAny<long>(), It.IsAny<CancellationToken>())).ReturnsAsync(_testFile);

        string filePath = Path.Combine(_testFile.Path, _testFile.Name);
        await File.WriteAllTextAsync(filePath, "Test content");

        // Act
        DefaultResponse result = await _service.DownloadFile(1);

        // Assert
        Assert.Equal(HttpStatusCode.OK, result.HttpStatus);
        Assert.NotNull(result.ObjectResponse);
        GameFileModel fileModel = Assert.IsType<GameFileModel>(result.ObjectResponse);
        Assert.Equal("testFile.txt", fileModel.Name);
        Assert.Equal("txt", fileModel.Format);
        Assert.Equal(await File.ReadAllBytesAsync(filePath), fileModel.Content);

        // Cleanup
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }

    [Fact]
    public async Task DownloadFile_ById_ShouldReturn_FileNotFound_WhenFileDoesNotExist()
    {
        // Arrange
        _mockRepository.Setup(repo => repo.GetFileAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Domain.GameFiles.GameFile?)null);

        // Act
        DefaultResponse result = await _service.DownloadFile(1);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, result.HttpStatus);
        Assert.Equal("File Not Found.", result.Message);
    }

    [Fact]
    public async Task DownloadFile_ShouldReturnErrorResponse_WhenExceptionIsThrown()
    {
        // Arrange
        long fileId = 1;
        Exception testException = new("Test exception");

        _mockRepository.Setup(repo => repo.GetFileAsync(fileId, It.IsAny<CancellationToken>())).ThrowsAsync(testException);

        // Mock the exception handler to return a specific response
        DefaultResponse expectedResponse = new(
            httpStatus: HttpStatusCode.InternalServerError,
            message: "An error has occurred.");
        _mockExceptionHandler.Setup(handler => handler.HandleException(testException)).Returns(expectedResponse);

        // Act
        DefaultResponse result = await _service.DownloadFile(fileId);

        // Assert
        Assert.Equal(HttpStatusCode.InternalServerError, result.HttpStatus);
        Assert.Equal("An error has occurred.", result.Message);

        // Verify that the exception handler was called with the test exception
        _mockExceptionHandler.Verify(handler => handler.HandleException(testException), Times.Once);
    }

    [Fact]
    public async Task GetGameFile_ShouldHandleException_WhenReadingFileFails()
    {
        // Arrange
        Domain.GameFiles.GameFile testFile = _testFile with
        {
            Path = string.Empty,
            Name = string.Empty,
            Format =
                                                                                                            string.Empty
        };

        _mockRepository.Setup(repo => repo.GetFileAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(testFile);

        _mockExceptionHandler.Setup(handler => handler.HandleException(It.IsAny<Exception>()))
            .Returns(new DefaultResponse(null, HttpStatusCode.InternalServerError, "File read error"));

        // Act
        DefaultResponse result = await _service.DownloadFile(_testGuid);

        // Assert
        Assert.Equal(HttpStatusCode.InternalServerError, result.HttpStatus);
        Assert.Equal("File read error", result.Message);
    }

    [Fact]
    public async Task GetGameFile_ShouldReturnNotFound_WhenFileDoesNotExist()
    {
        // Arrange
        _mockRepository.Setup(repo => repo.GetFileAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_testFile with { Path = string.Empty });

        // Act
        DefaultResponse result = await _service.DownloadFile(1);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, result.HttpStatus);
        Assert.Equal("File not found.", result.Message);
    }

    [Fact]
    public async Task MakeFileHasDeprecatedAsync_ShouldHandleException_WhenExceptionOccurs()
    {
        // Arrange
        _mockRepository.Setup(repo => repo.GetFileAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception("Error message"));

        _mockExceptionHandler.Setup(handler => handler.HandleException(It.IsAny<Exception>()))
            .Returns(new DefaultResponse(null, HttpStatusCode.InternalServerError, "Error message"));

        // Act
        DefaultResponse result = await _service.MakeFileHasDeprecatedAsync(_testGuid);

        // Assert
        Assert.Equal(HttpStatusCode.InternalServerError, result.HttpStatus);
        Assert.Equal("Error message", result.Message);
    }

    [Fact]
    public async Task MakeFileHasDeprecatedAsync_ShouldReturnNotFound_WhenFileDoesNotExist()
    {
        //  Arrange
        _mockRepository.Setup(repo => repo.GetFileAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Domain.GameFiles.GameFile?)null);

        // Act
        DefaultResponse result = await _service.MakeFileHasDeprecatedAsync(_testGuid);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, result.HttpStatus);
        Assert.Equal("File not found.", result.Message);
    }

    [Fact]
    public async Task MakeFileHasDeprecatedAsync_ShouldReturnSuccessMessage_WhenFileHasMarkedAsDeprecated()
    {
        // Arrange
        _mockRepository.Setup(repo => repo.GetFileAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_testFile with { Active = true });

        _mockRepository.Setup(repo => repo.UpdateFileAsync(It.IsAny<Domain.GameFiles.GameFile>(), It.IsAny<CancellationToken>()));

        // Act
        DefaultResponse result = await _service.MakeFileHasDeprecatedAsync(_testGuid);

        // Assert
        Assert.Equal(HttpStatusCode.OK, result.HttpStatus);
        Assert.Equal($"The file \"{_testFile.Name}\" of version has marked as deprecated.", result.Message);
    }

    [Fact]
    public async Task SaveFileAsync_ShouldDeleteFile_WhenItAlreadyExists()
    {
        // Arrange
        byte[] content = Encoding.UTF8.GetBytes("New content");
        string filePath = Path.Combine(_testFile.Path, _testFile.Name);

        // Pre-create the file to test deletion
        await File.WriteAllTextAsync(filePath, "Old content");

        // Act
        await _service.SaveFileAsync(_testFile, content);

        // Assert
        Assert.True(File.Exists(filePath));
        Assert.Equal("New content", await File.ReadAllTextAsync(filePath));
        _mockRepository.Verify(repo => repo.SaveFileAsync(It.IsAny<Domain.GameFiles.GameFile>(), It.IsAny<CancellationToken>()), Times.Once);

        // Cleanup
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }

    [Fact]
    public async Task SaveFileAsync_ShouldHandle_Exceptions()
    {
        // Arrange
        _mockRepository.Setup(repo => repo.SaveFileAsync(It.IsAny<Domain.GameFiles.GameFile>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Repository exception"));

        _mockExceptionHandler.Setup(handler => handler.HandleException(It.IsAny<Exception>()))
            .Returns(new DefaultResponse(null, HttpStatusCode.InternalServerError, "Internal server error"));

        byte[] content = Encoding.UTF8.GetBytes("Test content");
        string filePath = Path.Combine(_testFile.Path, _testFile.Name);

        // Act
        await _service.SaveFileAsync(_testFile, content);

        // Assert
        _mockExceptionHandler.Verify(handler => handler.HandleException(It.IsAny<Exception>()), Times.Once);

        // Cleanup
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }

    [Fact]
    public async Task SaveFileAsync_ShouldSaveFile_WhenCalled()
    {
        // Arrange
        byte[] content = Encoding.UTF8.GetBytes("Test content");
        string filePath = Path.Combine(_testFile.Path, _testFile.Name);

        File.Delete(filePath);

        // Act
        await _service.SaveFileAsync(_testFile, content);

        // Assert
        Assert.True(File.Exists(filePath));
        Assert.Equal("Test content", await File.ReadAllTextAsync(filePath));
        _mockRepository.Verify(repo => repo.SaveFileAsync(It.IsAny<Domain.GameFiles.GameFile>(), It.IsAny<CancellationToken>()), Times.Once);

        // Cleanup
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }
}