﻿using System.Net;
using System.Runtime.InteropServices.JavaScript;
using AutoMapper;

using FluentValidation;
using FluentValidation.Results;

using Moq;

using ROH.Domain.Version;
using ROH.Interfaces.Repository.Version;
using ROH.Interfaces.Services.Version;
using ROH.Services.Version;
using ROH.StandardModels.Response;
using ROH.StandardModels.Version;

namespace ROH.Test.Version
{
    public class GameVersionFileServiceTest
    {
        private readonly GameVersionModel _versionModel = new() { Version = 1, Release = 1, Review = 1, Released = false, ReleaseDate = null, VersionDate = DateTime.UtcNow };
        private readonly GameVersionFileModel _fileModel = new() { Name = "testFile", Size = 26354178, Path = "~/testFolder", Format = "format", Content = "wertfgby834ht348ghrfowefj234fh32urf3fh23rfhfh83" };

        private readonly GameVersion _version = new(null, null,  1, Guid.NewGuid(), 1, 1, 1, false);
        private readonly GameVersionFile _file = new(1, 1, Guid.NewGuid(),  26354178, "testFile", "~/testFolder", "format");

        [Fact]
        public async Task GetFiles_Returns_Files()
        {
            // Arrange
            MapperConfiguration config = new(cfg =>
            {
                // Configure your mappings here
                _ = cfg.CreateMap<GameVersion, GameVersionModel>().ReverseMap();
                _ = cfg.CreateMap<GameVersionFile, GameVersionFileModel>().ReverseMap();
            });

            Mapper mapper = new(config);

            List<GameVersionFile> files = new() { _file };

            Mock<IGameVersionFileRepository> mockRepository = new();
            _ = mockRepository.Setup(x => x.GetFiles(It.IsAny<GameVersion>())).ReturnsAsync(files);

            Mock<IGameVersionService> mockVersionService = new();

            Mock<IValidator<GameVersionFileModel>> mockValidator = new();
            GameVersionFileService service = new(mockRepository.Object, mockValidator.Object, mockVersionService.Object, mapper);

            // Act
            DefaultResponse result = await service.GetFiles(_versionModel);

            // Assert
            Assert.Equivalent(files, result.ObjectResponse);
        }

        [Fact]
        public async Task NewFile_With_Valid_File_Creates_File_And_Saves()
        {
            // Arrange
            MapperConfiguration config = new(cfg =>
            {
                // Configure your mappings here
                _ = cfg.CreateMap<GameVersion, GameVersionModel>().ReverseMap();
                _ = cfg.CreateMap<GameVersionFile, GameVersionFileModel>().ReverseMap();
            });

            Mapper mapper = new(config);

            _fileModel.GameVersion = _versionModel;
            Mock<IValidator<GameVersionFileModel>> mockValidator = new();
            _ = mockValidator.Setup(x => x.ValidateAsync(_fileModel, CancellationToken.None)).ReturnsAsync(new ValidationResult());

            Mock<IGameVersionFileRepository> mockRepository = new();
            _ = mockRepository.Setup(x => x.SaveFile(It.IsAny<GameVersionFile>())).Returns(Task.CompletedTask);

            Mock<IGameVersionService> mockVersionService = new();
            _ = mockVersionService.Setup(x => x.VerifyIfVersionExist(It.IsAny<GameVersionModel>())).ReturnsAsync(true);

            GameVersionFileService service = new(mockRepository.Object, mockValidator.Object, mockVersionService.Object, mapper);

            // Act
            bool result = service.NewFile(_fileModel).IsCompletedSuccessfully;

            // Assert
            Assert.True(result);

            await Task.CompletedTask;
        }
        
        [Fact]
        public async Task NewFile_With_Invalid_File_Returns_Error()
        {
            // Arrange
            MapperConfiguration config = new(cfg =>
            {
                // Configure your mappings here
                _ = cfg.CreateMap<GameVersion, GameVersionModel>().ReverseMap();
                _ = cfg.CreateMap<GameVersionFile, GameVersionFileModel>().ReverseMap();
            });

            Mapper mapper = new(config);

            _fileModel.GameVersion = _versionModel;
            Mock<IValidator<GameVersionFileModel>> mockValidator = new();
            _ = mockValidator.Setup(x => x.ValidateAsync(_fileModel, CancellationToken.None))
                .ReturnsAsync(new ValidationResult(
                    new List<ValidationFailure>
                        {
                        new("Name", "Name cannot be empty.")
                        })
                    );

            Mock<IGameVersionFileRepository> mockRepository = new();
            _ = mockRepository.Setup(x => x.SaveFile(It.IsAny<GameVersionFile>())).Returns(Task.CompletedTask);

            Mock<IGameVersionService> mockVersionService = new();
            _ = mockVersionService.Setup(x => x.VerifyIfVersionExist(It.IsAny<GameVersionModel>())).ReturnsAsync(true);

            GameVersionFileService service = new(mockRepository.Object, mockValidator.Object, mockVersionService.Object, mapper);

            // Act
            _fileModel.Name = "";
            var result = await service.NewFile(_fileModel);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, result.HttpStatus);
            Assert.False(string.IsNullOrWhiteSpace(result.Message));

        }

        [Fact]
        public async Task NewFile_With_Invalid_Version_Returns_Error()
        {
            // Arrange
            MapperConfiguration config = new(cfg =>
            {
                // Configure your mappings here
                _ = cfg.CreateMap<GameVersion, GameVersionModel>().ReverseMap();
                _ = cfg.CreateMap<GameVersionFile, GameVersionFileModel>().ReverseMap();
            });

            Mapper mapper = new(config);

            _fileModel.GameVersion = _versionModel;
            Mock<IValidator<GameVersionFileModel>> mockValidator = new();
            _ = mockValidator.Setup(x => x.ValidateAsync(_fileModel, CancellationToken.None))
                .ReturnsAsync(new ValidationResult());

            Mock<IGameVersionFileRepository> mockRepository = new();
            _ = mockRepository.Setup(x => x.SaveFile(It.IsAny<GameVersionFile>())).Returns(Task.CompletedTask);

            Mock<IGameVersionService> mockVersionService = new();
            _ = mockVersionService.Setup(x => x.VerifyIfVersionExist(It.IsAny<GameVersionModel>())).ReturnsAsync(false);

            GameVersionFileService service = new(mockRepository.Object, mockValidator.Object, mockVersionService.Object, mapper);

            // Act
            var result = await service.NewFile(_fileModel);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, result.HttpStatus);
            Assert.False(string.IsNullOrWhiteSpace(result.Message));

        }

        [Fact]
        public async Task DownloadFile_Returns_File_WhenFileExist()
        {
            // Arrange
            MapperConfiguration config = new(cfg =>
            {
                // Configure your mappings here
                _ = cfg.CreateMap<GameVersion, GameVersionModel>().ReverseMap();
                _ = cfg.CreateMap<GameVersionFile, GameVersionFileModel>().ReverseMap();
            });

            Mapper mapper = new(config);

            _fileModel.GameVersion = _versionModel;

            Mock<IGameVersionFileRepository> mockRepository = new();
            _ = mockRepository.Setup(x => x.GetFile(_file.Id)).ReturnsAsync(_file);

            Mock<IGameVersionService> mockVersionService = new();
            _ = mockVersionService.Setup(x => x.VerifyIfVersionExist(It.IsAny<GameVersionModel>())).ReturnsAsync(true);
            _ = mockVersionService.Setup(x => x.GetVersionByGuid(It.IsAny<Guid>())).Returns(Task.FromResult<DefaultResponse?>(new DefaultResponse(objectResponse: _version)));

            Mock<IValidator<GameVersionFileModel>> mockValidator = new();
            _ = mockValidator.Setup(x => x.ValidateAsync(It.IsAny<GameVersionFileModel>(), CancellationToken.None)).ReturnsAsync(new ValidationResult());

            GameVersionFileService service = new(mockRepository.Object, mockValidator.Object, mockVersionService.Object, mapper);

            // Act
            DefaultResponse result = await service.DownloadFile(_file.Id);

            // Assert
            Assert.Equal("wertfgby834ht348ghrfowefj234fh32urf3fh23rfhfh83", result.ObjectResponse);
        }

        [Fact]
        public async Task DownloadFile_Returns_NotFound_WhenFileNotExist()
        {
            // Arrange
            MapperConfiguration config = new(cfg =>
            {
                // Configure your mappings here
                _ = cfg.CreateMap<GameVersion, GameVersionModel>().ReverseMap();
                _ = cfg.CreateMap<GameVersionFile, GameVersionFileModel>().ReverseMap();
            });

            Mapper mapper = new(config);

            _fileModel.GameVersion = _versionModel;

            Mock<IGameVersionFileRepository> mockRepository = new();
            _ = mockRepository.Setup(x => x.GetFile(It.IsAny<long>())).ReturnsAsync(value: null);

            Mock<IGameVersionService> mockVersionService = new();
            _ = mockVersionService.Setup(x => x.VerifyIfVersionExist(It.IsAny<GameVersionModel>())).ReturnsAsync(true);
            _ = mockVersionService.Setup(x => x.GetVersionByGuid(It.IsAny<Guid>())).Returns(Task.FromResult<DefaultResponse?>(new DefaultResponse(objectResponse: _version)));

            Mock<IValidator<GameVersionFileModel>> mockValidator = new();
            _ = mockValidator.Setup(x => x.ValidateAsync(It.IsAny<GameVersionFileModel>(), CancellationToken.None)).ReturnsAsync(new ValidationResult());

            GameVersionFileService service = new(mockRepository.Object, mockValidator.Object, mockVersionService.Object, mapper);

            // Act
            DefaultResponse result = await service.DownloadFile(_file.Id);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, result.HttpStatus);
        }

        [Fact]
        public async Task DownloadFile_Returns_Error_WhenException()
        {
            // Arrange
            MapperConfiguration config = new(cfg =>
            {
                // Configure your mappings here
                _ = cfg.CreateMap<GameVersion, GameVersionModel>().ReverseMap();
                _ = cfg.CreateMap<GameVersionFile, GameVersionFileModel>().ReverseMap();
            });

            Mapper mapper = new(config);

            _fileModel.GameVersion = _versionModel;

            Mock<IGameVersionFileRepository> mockRepository = new();
            _ = mockRepository.Setup(x => x.GetFile(It.IsAny<long>())).ReturnsAsync(_file with { Name = "" });

            Mock<IGameVersionService> mockVersionService = new();
            _ = mockVersionService.Setup(x => x.VerifyIfVersionExist(It.IsAny<GameVersionModel>())).ReturnsAsync(true);
            _ = mockVersionService.Setup(x => x.GetVersionByGuid(It.IsAny<Guid>())).Returns(Task.FromResult<DefaultResponse?>(new DefaultResponse(objectResponse: _version)));

            Mock<IValidator<GameVersionFileModel>> mockValidator = new();
            _ = mockValidator.Setup(x => x.ValidateAsync(It.IsAny<GameVersionFileModel>(), CancellationToken.None)).ReturnsAsync(new ValidationResult());

            GameVersionFileService service = new(mockRepository.Object, mockValidator.Object, mockVersionService.Object, mapper);

            // Act
            DefaultResponse result = await service.DownloadFile(_file.Id);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, result.HttpStatus);
            Assert.False(string.IsNullOrWhiteSpace(result.Message));
        }

        [Fact]
        public async Task DownloadFile_Returns_Error_WhenFileIsNotValid()
        {
            // Arrange
            MapperConfiguration config = new(cfg =>
            {
                // Configure your mappings here
                _ = cfg.CreateMap<GameVersion, GameVersionModel>().ReverseMap();
                _ = cfg.CreateMap<GameVersionFile, GameVersionFileModel>().ReverseMap();
            });

            Mapper mapper = new(config);

            _fileModel.GameVersion = _versionModel;

            Mock<IGameVersionFileRepository> mockRepository = new();
            _ = mockRepository.Setup(x => x.GetFile(It.IsAny<long>())).ReturnsAsync(_file with { Name = "" });

            Mock<IGameVersionService> mockVersionService = new();
            _ = mockVersionService.Setup(x => x.VerifyIfVersionExist(It.IsAny<GameVersionModel>())).ReturnsAsync(true);
            _ = mockVersionService.Setup(x => x.GetVersionByGuid(It.IsAny<Guid>())).Returns(Task.FromResult<DefaultResponse?>(new DefaultResponse(objectResponse: _version)));

            Mock<IValidator<GameVersionFileModel>> mockValidator = new();
            _ = mockValidator.Setup(x => x.ValidateAsync(It.IsAny<GameVersionFileModel>(), CancellationToken.None)).ReturnsAsync(new ValidationResult(new List<ValidationFailure>
                    {new ValidationFailure("Name", "Name cannot be empty")}));

            GameVersionFileService service = new(mockRepository.Object, mockValidator.Object, mockVersionService.Object, mapper);

            // Act
            DefaultResponse result = await service.DownloadFile(_file.Id);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, result.HttpStatus);
            Assert.False(string.IsNullOrWhiteSpace(result.Message));
        }
    }
}