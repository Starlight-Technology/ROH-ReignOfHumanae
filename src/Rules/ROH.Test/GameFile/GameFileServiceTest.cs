using Moq;
using ROH.Interfaces.Repository.GameFile;
using ROH.Interfaces.Services.ExceptionService;
using ROH.Services.GameFile;
using ROH.StandardModels.File;
using ROH.StandardModels.Response;
using ROH.StandardModels.Version;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace ROH.Test.GameFile
{
    public class GameFileServiceTest
    {
        private readonly Mock<IGameFileRepository> _mockRepository;
        private readonly Mock<IExceptionHandler> _mockExceptionHandler;
        private readonly GameFileService _service;

        private readonly Guid _testGuid = Guid.NewGuid();
        private readonly Domain.GameFiles.GameFile _testFile = new(
            Name: "testFile.txt",
            Format: "txt",
            Path: Path.GetTempPath(),
            Guid: Guid.NewGuid()
        );

        public GameFileServiceTest()
        {
            _mockRepository = new Mock<IGameFileRepository>();
            _mockExceptionHandler = new Mock<IExceptionHandler>();
            _service = new GameFileService(_mockRepository.Object, _mockExceptionHandler.Object);
        }

        [Fact]
        public async Task DownloadFile_ByGuid_ShouldReturn_FileNotFound_WhenFileDoesNotExist()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetFile(It.IsAny<Guid>()))
                .ReturnsAsync((Domain.GameFiles.GameFile?)null);

            // Act
            DefaultResponse result = await _service.DownloadFile(_testGuid);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, result.HttpStatus);
            Assert.Equal("File Not Found.", result.Message);
        }

        [Fact]
        public async Task DownloadFile_ByGuid_ShouldReturn_FileContent_WhenFileExists()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetFile(It.IsAny<Guid>()))
                .ReturnsAsync(_testFile);

            string filePath = Path.Combine(_testFile.Path, _testFile.Name);
            await File.WriteAllTextAsync(filePath, "Test content");

            // Act
            DefaultResponse result = await _service.DownloadFile(_testFile.Guid);

            // Assert
            Assert.Equal(HttpStatusCode.OK, result.HttpStatus);
            Assert.NotNull(result.ObjectResponse);
            var fileModel = Assert.IsType<GameFileModel>(result.ObjectResponse);
            Assert.Equal("testFile.txt", fileModel.Name);
            Assert.Equal("txt", fileModel.Format);
        }

        [Fact]
        public async Task DownloadFile_ByGuid_ShouldHandle_Exceptions()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetFile(It.IsAny<Guid>()))
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
        public async Task DownloadFile_ById_ShouldReturn_FileNotFound_WhenFileDoesNotExist()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetFile(It.IsAny<long>()))
                .ReturnsAsync((Domain.GameFiles.GameFile?)null);

            // Act
            DefaultResponse result = await _service.DownloadFile(1);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, result.HttpStatus);
            Assert.Equal("File Not Found.", result.Message);
        }

        [Fact]
        public async Task DownloadFile_ById_ShouldReturn_FileContent_WhenFileExists()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetFile(It.IsAny<long>()))
                .ReturnsAsync(_testFile);

            string filePath = Path.Combine(_testFile.Path, _testFile.Name);
            await File.WriteAllTextAsync(filePath, "Test content");

            // Act
            DefaultResponse result = await _service.DownloadFile(1);

            // Assert
            Assert.Equal(HttpStatusCode.OK, result.HttpStatus);
            Assert.NotNull(result.ObjectResponse);
            var fileModel = Assert.IsType<GameFileModel>(result.ObjectResponse);
            Assert.Equal("testFile.txt", fileModel.Name);
            Assert.Equal("txt", fileModel.Format);
        }

        [Fact]
        public async Task SaveFileAsync_ShouldSaveFile_WhenCalled()
        {
            // Arrange
            byte[] content = System.Text.Encoding.UTF8.GetBytes("Test content");
            string filePath = Path.Combine(_testFile.Path, _testFile.Name);

            // Act
            await _service.SaveFileAsync(_testFile, content);

            // Assert
            Assert.True(File.Exists(filePath));
            Assert.Equal("Test content", await File.ReadAllTextAsync(filePath));
            _mockRepository.Verify(repo => repo.SaveFile(It.IsAny<Domain.GameFiles.GameFile>()), Times.Once);

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
            _mockRepository.Setup(repo => repo.SaveFile(It.IsAny<Domain.GameFiles.GameFile>()))
                .ThrowsAsync(new Exception("Repository exception"));

            _mockExceptionHandler.Setup(handler => handler.HandleException(It.IsAny<Exception>()))
                .Returns(new DefaultResponse(null, HttpStatusCode.InternalServerError, "Internal server error"));

            byte[] content = System.Text.Encoding.UTF8.GetBytes("Test content");

            // Act
            await _service.SaveFileAsync(_testFile, content);

            // Assert
            _mockExceptionHandler.Verify(handler => handler.HandleException(It.IsAny<Exception>()), Times.Once);
        }
    }
}
