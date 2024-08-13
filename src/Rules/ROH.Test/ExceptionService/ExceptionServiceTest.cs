using Microsoft.Extensions.Configuration;

using Moq;

using ROH.Domain.Logging;
using ROH.Interfaces.Repository.Log;
using ROH.Services.ExceptionService;

using System.Net;

namespace ROH.Test.ExceptionService;

public class ExceptionHandlerTests
{
    [Fact]
    public void HandleException_ShouldLogException()
    {
        // Arrange
        var logRepositoryMock = new Mock<ILogRepository>();
        var configurationMock = new Mock<IConfiguration>();
        var configurationSectionMock = new Mock<IConfigurationSection>();

        configurationSectionMock.Setup(x => x.Value).Returns("true");
        configurationMock.Setup(x => x.GetSection("IsDebugMode")).Returns(configurationSectionMock.Object);

        var exceptionHandler = new ExceptionHandler(logRepositoryMock.Object, configurationMock.Object);
        var exception = new Exception("Test exception");

        // Act
        exceptionHandler.HandleException(exception);

        // Assert
        var expectedError = $@"Source: {exception.Source};Message: {exception.Message}; StackTrace: {exception.StackTrace}";
        logRepositoryMock.Verify(logRepo => logRepo.SaveLog(It.Is<Log>(log => log.Message == expectedError && log.Severity == Severity.Error)), Times.Once);
    }

    [Fact]
    public void HandleException_ShouldReturnErrorResponseInDebugMode()
    {
        // Arrange
        var logRepositoryMock = new Mock<ILogRepository>();
        var configurationMock = new Mock<IConfiguration>();
        var configurationSectionMock = new Mock<IConfigurationSection>();

        configurationSectionMock.Setup(x => x.Value).Returns("true");
        configurationMock.Setup(x => x.GetSection("IsDebugMode")).Returns(configurationSectionMock.Object);

        var exceptionHandler = new ExceptionHandler(logRepositoryMock.Object, configurationMock.Object);
        var exception = new Exception("Test exception");

        // Act
        var response = exceptionHandler.HandleException(exception);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.HttpStatus);
        Assert.Contains("Source:", response.Message);
    }

    [Fact]
    public void HandleException_ShouldReturnFriendlyResponseInReleaseMode()
    {
        // Arrange
        var logRepositoryMock = new Mock<ILogRepository>();
        var configurationMock = new Mock<IConfiguration>();
        var configurationSectionMock = new Mock<IConfigurationSection>();

        configurationSectionMock.Setup(x => x.Value).Returns("false");
        configurationMock.Setup(x => x.GetSection("IsDebugMode")).Returns(configurationSectionMock.Object);

        var exceptionHandler = new ExceptionHandler(logRepositoryMock.Object, configurationMock.Object);
        var exception = new Exception("Test exception");

        // Act
        var response = exceptionHandler.HandleException(exception);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.HttpStatus);
        Assert.Equal("An error has occurred. Don't be afraid! An email with the error details has been sent to your developers.", response.Message);
    }
}
