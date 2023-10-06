// Ignore Spelling: Shouldnt Havent

using AutoMapper;

using FluentValidation;

using Moq;

using ROH.Domain.Paginator;
using ROH.Domain.Version;
using ROH.Interfaces.Repository.Version;
using ROH.Interfaces.Services.Version;
using ROH.Services.Version;
using ROH.StandardModels.Paginator;
using ROH.StandardModels.Response;
using ROH.StandardModels.Version;

namespace ROH.Test.Version
{
    public class GameVersionServiceTests
    {
        private static readonly DateTime _utcNow = DateTime.UtcNow;

        private readonly GameVersionModel _versionModel = new() { Version = 1, Release = 1, Review = 1, Released = false, ReleaseDate = null, VersionDate = _utcNow };

        private readonly GameVersion _version = new(null, _utcNow, 1, 1, 1, 1, false);


        [Fact]
        public async Task GetVersionById_ShouldReturnVersion_WhenVersionExists()
        {
            // Arrange
            MapperConfiguration config = new(cfg =>
            {
                // Configure your mappings here
                _ = cfg.CreateMap<GameVersion, GameVersionModel>().ReverseMap();
            });

            Mapper mapper = new(config);

            Mock<IGameVersionRepository> mockRepository = new();
            _ = mockRepository.Setup(x => x.GetVersionById(It.IsAny<long>())).ReturnsAsync(_version);

            GameVersionService service = new(mockRepository.Object, mapper);

            // Act
            DefaultResponse? result = await service.GetVersionById(1);

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

            List<dynamic> listOfVersions = new();

            for (int i = 0; i < 10; i++)
            {
                GameVersion version = _version with { Id = i + 1 };

                listOfVersions.Add(version as dynamic);
            }

            Paginated paginatedVersions = new(listOfVersions.Count, listOfVersions);

            Mock<IGameVersionRepository> mockRepository = new();
            _ = mockRepository.Setup(x => x.GetAllVersions(10, 0)).ReturnsAsync(paginatedVersions);

            GameVersionService service = new(mockRepository.Object, mapper);

            // Act
            DefaultResponse? result = await service.GetAllVersions();
            var versions = ((PaginatedModel?)result.ObjectResponse)?.ObjectResponse?.Cast<GameVersionModel>().ToList();

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

            List<dynamic> listOfVersions = new();

            for (int i = 0; i < 10; i++)
            {
                GameVersion version = _version with { Id = i + 1 };

                listOfVersions.Add(version as dynamic);
            }

            Paginated paginatedVersions = new(5, listOfVersions.Take(5).ToList());

            Mock<IGameVersionRepository> mockRepository = new();
            _ = mockRepository.Setup(x => x.GetAllVersions(5, 0)).ReturnsAsync(paginatedVersions);

            GameVersionService service = new(mockRepository.Object, mapper);

            // Act
            DefaultResponse? result = await service.GetAllVersions(5, 1);
            var versions = ((PaginatedModel?)result.ObjectResponse)?.ObjectResponse?.Cast<GameVersionModel>().ToList();

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

            List<dynamic> listOfVersions = new();

            for (int i = 0; i < 10; i++)
            {
                GameVersion version = _version with { Id = i + 1, Version = i + 1 };

                listOfVersions.Add(version as dynamic);
            }

            Paginated paginatedVersions = new(10, listOfVersions.Skip(5).Take(5).ToList());

            Mock<IGameVersionRepository> mockRepository = new();
            _ = mockRepository.Setup(x => x.GetAllVersions(5, 5)).ReturnsAsync(paginatedVersions);

            GameVersionService service = new(mockRepository.Object, mapper);

            // Act
            DefaultResponse? result = await service.GetAllVersions(5, 2);
            var versions = ((PaginatedModel?)result.ObjectResponse)?.ObjectResponse?.Cast<GameVersionModel>().ToList();

            // Assert
            Assert.True(versions?.Count == 5);
            Assert.True(versions.First().Version == 6);
            Assert.True(versions.Last().Version == 10);
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

            List<dynamic> listOfVersions = new();

            for (int i = 0; i < 10; i++)
            {
                GameVersion version = _version with { Id = i + 1 , Released = true};

                listOfVersions.Add(version as dynamic);
            }

            Paginated paginatedVersions = new(listOfVersions.Count, listOfVersions);

            Mock<IGameVersionRepository> mockRepository = new();
            _ = mockRepository.Setup(x => x.GetAllReleasedVersions(10, 0)).ReturnsAsync(paginatedVersions);

            GameVersionService service = new(mockRepository.Object, mapper);

            // Act
            DefaultResponse? result = await service.GetAllReleasedVersions();
            var versions = ((PaginatedModel?)result.ObjectResponse)?.ObjectResponse?.Cast<GameVersionModel>().ToList();

            // Assert
            Assert.True(versions?.Any());
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

            List<dynamic> listOfVersions = new();
             

            Paginated paginatedVersions = new(listOfVersions.Count, listOfVersions);

            Mock<IGameVersionRepository> mockRepository = new();
            _ = mockRepository.Setup(x => x.GetAllReleasedVersions(10, 0)).ReturnsAsync(paginatedVersions);

            GameVersionService service = new(mockRepository.Object, mapper);

            // Act
            DefaultResponse? result = await service.GetAllReleasedVersions();
            var versions = ((PaginatedModel?)result.ObjectResponse)?.ObjectResponse?.Cast<GameVersionModel>().ToList();

            // Assert
            Assert.False(versions?.Any());
        }

    }
}