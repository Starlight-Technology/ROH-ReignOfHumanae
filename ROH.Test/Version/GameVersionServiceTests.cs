// Ignore Spelling: Shouldnt Havent

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

namespace ROH.Test.Version
{
    public class GameVersionServiceTests
    {
        private static readonly DateTime _utcNow = DateTime.UtcNow;

        private static readonly Guid _guidGenerated = Guid.NewGuid();

        private readonly GameVersionModel _versionModel = new() { Guid = _guidGenerated, Version = 1, Release = 1, Review = 1, Released = false, ReleaseDate = null, VersionDate = _utcNow };

        private readonly GameVersion _version = new( _utcNow, 1, _guidGenerated, 1, 1, 1);

        [Fact]
        public async Task GetVersionByGuid_ShouldReturnVersion_WhenVersionExists()
        {
            // Arrange
            MapperConfiguration config = new(cfg =>
            {
                // Configure your mappings here
                _ = cfg.CreateMap<GameVersion, GameVersionModel>().ReverseMap();
            });

            Mapper mapper = new(config);

            Mock<IGameVersionRepository> mockRepository = new();
            _ = mockRepository.Setup(x => x.GetVersionByGuid(It.IsAny<Guid>())).ReturnsAsync(_version);

            Mock<IExceptionHandler> mockExceptionHandler = new();

            GameVersionService service = new(mockRepository.Object, mapper, mockExceptionHandler.Object);

            // Act
            DefaultResponse? result = await service.GetVersionByGuid(_guidGenerated.ToString());

            // Assert
            Assert.Equivalent(_versionModel, result?.ObjectResponse);
        }

        [Fact]
        public async Task GetAllVersions_ShouldReturnListWithVersions_WhenHaveAny()
        {
            // Arrange
            MapperConfiguration config = new(cfg =>
            {
                // Configure your mappings here
                _ = cfg.CreateMap<GameVersion, GameVersionModel>().ReverseMap();
            });

            Mapper mapper = new(config);

            List<dynamic> listOfVersions = [];

            for (int i = 0; i < 10; i++)
            {
                GameVersion version = _version with { Id = i + 1 };

                listOfVersions.Add(version);
            }

            Paginated paginatedVersions = new(listOfVersions.Count, listOfVersions);

            Mock<IGameVersionRepository> mockRepository = new();
            _ = mockRepository.Setup(x => x.GetAllVersions(10, 0)).ReturnsAsync(paginatedVersions);

            Mock<IExceptionHandler> mockExceptionHandler = new();

            GameVersionService service = new(mockRepository.Object, mapper, mockExceptionHandler.Object);

            // Act
            DefaultResponse? result = await service.GetAllVersions();
            List<GameVersionModel>? versions = ((PaginatedModel?)result.ObjectResponse)?.ObjectResponse?.Cast<GameVersionModel>().ToList();

            // Assert
            Assert.True(versions?.Count > 1);
        }

        [Fact]
        public async Task GetAllVersions_ShouldReturnListWithMaximum5Versions_WhenHaveMoreThan5Versions()
        {
            // Arrange
            MapperConfiguration config = new(cfg =>
            {
                // Configure your mappings here
                _ = cfg.CreateMap<GameVersion, GameVersionModel>().ReverseMap();
            });

            Mapper mapper = new(config);

            List<dynamic> listOfVersions = [];

            for (int i = 0; i < 10; i++)
            {
                GameVersion version = _version with { Id = i + 1 };

                listOfVersions.Add(version);
            }

            Paginated paginatedVersions = new(5, listOfVersions.Take(5).ToList());

            Mock<IGameVersionRepository> mockRepository = new();
            _ = mockRepository.Setup(x => x.GetAllVersions(5, 0)).ReturnsAsync(paginatedVersions);

            Mock<IExceptionHandler> mockExceptionHandler = new();

            GameVersionService service = new(mockRepository.Object, mapper, mockExceptionHandler.Object);

            // Act
            DefaultResponse? result = await service.GetAllVersions(5, 1);
            List<GameVersionModel>? versions = ((PaginatedModel?)result.ObjectResponse)?.ObjectResponse?.Cast<GameVersionModel>().ToList();

            // Assert
            Assert.True(versions?.Count == 5);
        }

        [Fact]
        public async Task GetAllVersions_ShouldSkip5VersionsAndReturnListWithMaximum5Versions_WhenHaveMoreThan5Versions()
        {
            // Arrange
            MapperConfiguration config = new(cfg =>
            {
                // Configure your mappings here
                _ = cfg.CreateMap<GameVersion, GameVersionModel>().ReverseMap();
            });

            Mapper mapper = new(config);

            List<dynamic> listOfVersions = [];

            for (int i = 0; i < 10; i++)
            {
                GameVersion version = _version with { Id = i + 1, Version = i + 1 };

                listOfVersions.Add(version);
            }

            Paginated paginatedVersions = new(10, listOfVersions.Skip(5).Take(5).ToList());

            Mock<IGameVersionRepository> mockRepository = new();
            _ = mockRepository.Setup(x => x.GetAllVersions(5, 5)).ReturnsAsync(paginatedVersions);

            Mock<IExceptionHandler> mockExceptionHandler = new();

            GameVersionService service = new(mockRepository.Object, mapper, mockExceptionHandler.Object);

            // Act
            DefaultResponse? result = await service.GetAllVersions(5, 2);
            List<GameVersionModel>? versions = ((PaginatedModel?)result.ObjectResponse)?.ObjectResponse?.Cast<GameVersionModel>().ToList();

            // Assert
            Assert.True(versions?.Count == 5);
            Assert.True(versions[0].Version == 6);
            Assert.True(versions[^1].Version == 10);
        }

        [Fact]
        public async Task GetAllReleasedVersions_ShouldReturnListWithReleasedVersions_WhenHaveAny()
        {
            // Arrange
            MapperConfiguration config = new(cfg =>
            {
                // Configure your mappings here
                _ = cfg.CreateMap<GameVersion, GameVersionModel>().ReverseMap();
            });

            Mapper mapper = new(config);

            List<dynamic> listOfVersions = [];

            for (int i = 0; i < 10; i++)
            {
                GameVersion version = _version with { Id = i + 1, Released = true };

                listOfVersions.Add(version);
            }

            Paginated paginatedVersions = new(listOfVersions.Count, listOfVersions);

            Mock<IGameVersionRepository> mockRepository = new();
            _ = mockRepository.Setup(x => x.GetAllReleasedVersions(10, 0)).ReturnsAsync(paginatedVersions);

            Mock<IExceptionHandler> mockExceptionHandler = new();

            GameVersionService service = new(mockRepository.Object, mapper, mockExceptionHandler.Object);

            // Act
            DefaultResponse? result = await service.GetAllReleasedVersions();
            List<GameVersionModel>? versions = ((PaginatedModel?)result.ObjectResponse)?.ObjectResponse?.Cast<GameVersionModel>().ToList();

            // Assert
            Assert.True(versions?.Count > 0);
        }

        [Fact]
        public async Task GetAllReleasedVersions_ShouldntReturnListWithReleasedVersions_WhenHaventAny()
        {
            // Arrange
            MapperConfiguration config = new(cfg =>
            {
                // Configure your mappings here
                _ = cfg.CreateMap<GameVersion, GameVersionModel>().ReverseMap();
            });

            Mapper mapper = new(config);

            List<dynamic> listOfVersions = [];

            Paginated paginatedVersions = new(listOfVersions.Count, listOfVersions);

            Mock<IGameVersionRepository> mockRepository = new();
            _ = mockRepository.Setup(x => x.GetAllReleasedVersions(10, 0)).ReturnsAsync(paginatedVersions);

            Mock<IExceptionHandler> mockExceptionHandler = new();

            GameVersionService service = new(mockRepository.Object, mapper, mockExceptionHandler.Object);

            // Act
            DefaultResponse? result = await service.GetAllReleasedVersions();
            List<GameVersionModel>? versions = ((PaginatedModel?)result.ObjectResponse)?.ObjectResponse?.Cast<GameVersionModel>().ToList();

            // Assert
            Assert.False(versions?.Count > 0);
        }

        [Fact]
        public async Task NewVersion_ShouldReturnStatusCreated_WhenVersionCreatedWithSuccess()
        {
            // Arrange
            MapperConfiguration config = new(cfg =>
            {
                // Configure your mappings here
                _ = cfg.CreateMap<GameVersion, GameVersionModel>().ReverseMap();
            });

            Mapper mapper = new(config);

            Mock<IGameVersionRepository> mockRepository = new();
            _ = mockRepository.Setup(x => x.SetNewGameVersion(_version)).ReturnsAsync(_version);

            Mock<IExceptionHandler> mockExceptionHandler = new();

            GameVersionService service = new(mockRepository.Object, mapper, mockExceptionHandler.Object);

            // Act
            DefaultResponse result = await service.NewVersion(_versionModel);

            // Assert
            Assert.Equal(HttpStatusCode.Created, result.HttpStatus);
        }

        [Fact]
        public async Task NewVersion_ShouldReturnStatusConflict_WhenVersionNotCreated()
        {
            // Arrange
            MapperConfiguration config = new(cfg =>
            {
                // Configure your mappings here
                _ = cfg.CreateMap<GameVersion, GameVersionModel>().ReverseMap();
            });

            Mapper mapper = new(config);

            Mock<IGameVersionRepository> mockRepository = new();
            _ = mockRepository.Setup(x => x.SetNewGameVersion(It.IsAny<GameVersion>())).ReturnsAsync(_version);
            _ = mockRepository.Setup(x => x.VerifyIfExist(It.IsAny<GameVersion>())).ReturnsAsync(true);

            Mock<IExceptionHandler> mockExceptionHandler = new();

            GameVersionService service = new(mockRepository.Object, mapper, mockExceptionHandler.Object);

            // Act
            DefaultResponse result = await service.NewVersion(_versionModel);

            // Assert
            Assert.Equal(HttpStatusCode.Conflict, result.HttpStatus);
        }

        [Fact]
        public async Task SetReleased_ShouldReturnStatusOkWithSuccessfullMesage_WhenVersionSetHasReleased()
        {
            // Arrange
            MapperConfiguration config = new(cfg =>
            {
                // Configure your mappings here
                _ = cfg.CreateMap<GameVersion, GameVersionModel>().ReverseMap();
            });
            Mapper mapper = new(config);

            Mock<IGameVersionRepository> mockRepository = new();
            _ = mockRepository.Setup(x => x.GetVersionByGuid(It.IsAny<Guid>())).ReturnsAsync(_version);

            Mock<IExceptionHandler> mockExceptionHandler = new();

            GameVersionService service = new(mockRepository.Object, mapper, mockExceptionHandler.Object);

            DefaultResponse expected = new(message: "The version has been set as release.");

            // Act
            DefaultResponse result = await service.SetReleased(_guidGenerated.ToString());

            // Assert
            Assert.Equivalent(result, expected);
        }

        [Fact]
        public async Task SetReleased_ShouldReturnError_WhenGuidIsInvalid()
        {
            // Arrange
            MapperConfiguration config = new(cfg =>
            {
                // Configure your mappings here
                _ = cfg.CreateMap<GameVersion, GameVersionModel>().ReverseMap();
            });
            Mapper mapper = new(config);

            Mock<IGameVersionRepository> mockRepository = new();
            _ = mockRepository.Setup(x => x.GetVersionByGuid(It.IsAny<Guid>())).ReturnsAsync((GameVersion?)null);

            Mock<IExceptionHandler> mockExceptionHandler = new();

            GameVersionService service = new(mockRepository.Object, mapper, mockExceptionHandler.Object);

            DefaultResponse expected = new() { HttpStatus = System.Net.HttpStatusCode.ExpectationFailed, Message = "The Guid is invalid!" };

            // Act
            DefaultResponse result = await service.SetReleased(_guidGenerated.ToString());

            // Assert
            Assert.Equivalent(result, expected);
        }
    }
}