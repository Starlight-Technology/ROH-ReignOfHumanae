
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
        private readonly GameVersionModel _versionModel = new () { Version = 1, Release = 1, Review = 1, Released = false, ReleaseDate = null, VersionDate = DateTime.Today };
        private readonly GameVersionFileModel _fileModel = new() { Name= "testFile", Size =26354178, Path="~/testFolder", Format="format", Content="wertfgby834ht348ghrfowefj234fh32urf3fh23rfhfh83" };

        private readonly GameVersion _version = new (1,1,1,1,false);
        private readonly GameVersionFile _file = new (1,1,"testFile", 26354178, "~/testFolder", "format");
    

        [Fact]
        public async Task GetFiles_Returns_Files()
        {
            // Arrange
            var config = new MapperConfiguration(cfg =>
            {
                // Configure your mappings here
                cfg.CreateMap<GameVersion, GameVersionModel>().ReverseMap();
                cfg.CreateMap<GameVersionFile, GameVersionFileModel>().ReverseMap();
            });

            var mapper = new Mapper(config);

            var files = new List<GameVersionFile> { _file };

            var mockRepository = new Mock<IGameVersionFileRepository>();
            mockRepository.Setup(x => x.GetFiles(It.IsAny<GameVersion>())).ReturnsAsync(files);

            var mockVersionService = new Mock<IGameVersionService>();

            var mockValidator = new Mock<IValidator<GameVersionFileModel>>();
            var service = new GameVersionFileService(mockRepository.Object, mockValidator.Object, mockVersionService.Object, mapper);

            // Act
            var result = await service.GetFiles(_versionModel);

            // Assert
            Assert.Equal(files, result.ObjectResponse);
        }

        [Fact]
        public async Task NewFile_With_Valid_File_Creates_File_And_Saves()
        {
            // Arrange
            var config = new MapperConfiguration(cfg =>
            {
                // Configure your mappings here
                cfg.CreateMap<GameVersion, GameVersionModel>().ReverseMap();
                cfg.CreateMap<GameVersionFile, GameVersionFileModel>().ReverseMap();
            });

            var mapper = new Mapper(config);

            _fileModel.GameVersion = _versionModel;
            var mockValidator = new Mock<IValidator<GameVersionFileModel>>();
            mockValidator.Setup(x => x.ValidateAsync(_fileModel, CancellationToken.None)).ReturnsAsync(new ValidationResult());

            var mockRepository = new Mock<IGameVersionFileRepository>();
            mockRepository.Setup(x => x.SaveFile(It.IsAny<GameVersionFile>())).Returns(Task.CompletedTask);

            var mockVersionService = new Mock<IGameVersionService>();
            mockVersionService.Setup(x => x.VerifyIfVersionExist(It.IsAny<GameVersionModel>())).ReturnsAsync(true);

            var service = new GameVersionFileService(mockRepository.Object, mockValidator.Object, mockVersionService.Object, mapper);

            // Act
            var result = service.NewFile(_fileModel).IsCompletedSuccessfully;

            // Assert
            Assert.True(result);

            await Task.CompletedTask;
        }

        [Fact]
        public async Task DownloadFile_Returns_File()
        {
            // Arrange
            var config = new MapperConfiguration(cfg =>
            {
                // Configure your mappings here
                cfg.CreateMap<GameVersion, GameVersionModel>().ReverseMap();
                cfg.CreateMap<GameVersionFile, GameVersionFileModel>().ReverseMap();
            });

            var mapper = new Mapper(config);

            _fileModel.GameVersion = _versionModel;

            var mockRepository = new Mock<IGameVersionFileRepository>();
            mockRepository.Setup(x => x.GetFile(_file.Id)).ReturnsAsync(_file);

            var mockVersionService = new Mock<IGameVersionService>();
            mockVersionService.Setup(x => x.VerifyIfVersionExist(It.IsAny<GameVersionModel>())).ReturnsAsync(true);
            mockVersionService.Setup(x => x.GetVersionById(It.IsAny<long>())).Returns(Task.FromResult<DefaultResponse?>(new DefaultResponse(objectResponse: _version)));

            var mockValidator = new Mock<IValidator<GameVersionFileModel>>();
            mockValidator.Setup(x => x.ValidateAsync(It.IsAny<GameVersionFileModel>(), CancellationToken.None)).ReturnsAsync(new ValidationResult());

            var service = new GameVersionFileService(mockRepository.Object, mockValidator.Object, mockVersionService.Object, mapper);

            // Act
            var result = await service.DownloadFile(_file.Id);

            // Assert
            Assert.Equal("wertfgby834ht348ghrfowefj234fh32urf3fh23rfhfh83", result.ObjectResponse);

        }
    }
}
