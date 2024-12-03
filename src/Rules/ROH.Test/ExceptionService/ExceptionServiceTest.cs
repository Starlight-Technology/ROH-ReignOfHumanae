////-----------------------------------------------------------------------
//// <copyright file="ExceptionServiceTest.cs" company="Starlight-Technology">
////     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
////     Copyright (c) Starlight-Technology. All rights reserved.
//// </copyright>
////-----------------------------------------------------------------------
//using Microsoft.Extensions.Configuration;

//using Moq;

//using ROH.Domain.Logging;
//using ROH.Interfaces.Repository.Log;
//using ROH.Services.ExceptionService;
//using ROH.StandardModels.Response;

//using System.Net;

//namespace ROH.Test.ExceptionService;

//public class ExceptionServiceTest
//{
//    [Fact]
//    public void HandleExceptionShouldLogException()
//    {
//        // Arrange
//        Mock<ILogRepository> logRepositoryMock = new();
//        Mock<IConfiguration> configurationMock = new();
//        Mock<IConfigurationSection> configurationSectionMock = new();

//        configurationSectionMock.Setup(x => x.Value).Returns("true");
//        configurationMock.Setup(x => x.GetSection("IsDebugMode")).Returns(configurationSectionMock.Object);

//        ExceptionHandler exceptionHandler = new(logRepositoryMock.Object, configurationMock.Object);
//        Exception exception = new("Test exception");

//        // Act
//        exceptionHandler.HandleException(exception);

//        // Assert
//        string expectedError = $@"Source: {exception.Source};Message: {exception.Message}; StackTrace: {exception.StackTrace}";
//        logRepositoryMock.Verify(
//            logRepo => logRepo.SaveLogAsync(
//                It.Is<Log>(log => (string.Compare(log.Message, expectedError, StringComparison.Ordinal) == 0) && (log.Severity == Severity.Error)), It.IsAny<CancellationToken>()),
//            Times.Once);
//    }

//    [Fact]
//    public void HandleExceptionShouldReturnErrorResponseInDebugMode()
//    {
//        // Arrange
//        Mock<ILogRepository> logRepositoryMock = new();
//        Mock<IConfiguration> configurationMock = new();
//        Mock<IConfigurationSection> configurationSectionMock = new();

//        configurationSectionMock.Setup(x => x.Value).Returns("true");
//        configurationMock.Setup(x => x.GetSection("IsDebugMode")).Returns(configurationSectionMock.Object);

//        ExceptionHandler exceptionHandler = new(logRepositoryMock.Object, configurationMock.Object);
//        Exception exception = new("Test exception");

//        // Act
//        DefaultResponse response = exceptionHandler.HandleException(exception);

//        // Assert
//        Assert.Equal(HttpStatusCode.InternalServerError, response.HttpStatus);
//        Assert.Contains("Source:", response.Message);
//    }

//    [Fact]
//    public void HandleExceptionShouldReturnFriendlyResponseInReleaseMode()
//    {
//        // Arrange
//        Mock<ILogRepository> logRepositoryMock = new();
//        Mock<IConfiguration> configurationMock = new();
//        Mock<IConfigurationSection> configurationSectionMock = new();

//        configurationSectionMock.Setup(x => x.Value).Returns("false");
//        configurationMock.Setup(x => x.GetSection("IsDebugMode")).Returns(configurationSectionMock.Object);

//        ExceptionHandler exceptionHandler = new(logRepositoryMock.Object, configurationMock.Object);
//        Exception exception = new("Test exception");

//        // Act
//        DefaultResponse response = exceptionHandler.HandleException(exception);

//        // Assert
//        Assert.Equal(HttpStatusCode.InternalServerError, response.HttpStatus);
//        Assert.Equal(
//            "An error has occurred. Don't be afraid! An email with the error details has been sent to your developers.",
//            response.Message);
//    }
//}
