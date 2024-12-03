using Microsoft.EntityFrameworkCore;

using ROH.Context.Version;
using ROH.Context.Version.Entities;
using ROH.Context.Version.Repository;

namespace ROH.Test.Version
{
    public class GameVersionRepositoryTests
    {
        private static DbContextOptions<VersionContext> GetInMemoryDbContextOptions(string dbName)
        {
            return new DbContextOptionsBuilder<VersionContext>()
                .UseInMemoryDatabase(databaseName: dbName).EnableSensitiveDataLogging()
                .Options;
        }

        [Fact]
        public async Task GetAllReleasedVersionsAsync_ReturnsCorrectData()
        {
            // Arrange
            var options = GetInMemoryDbContextOptions("GetAllReleasedVersionsAsync_Db");
            using var context = new VersionContext(options);
            var repository = new GameVersionRepository(context);

            context.GameVersions.AddRange(new List<GameVersion>
                 {
                    new (new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc), 1, Guid.NewGuid(), 1, 0, 5)
                    {
                        Released = false,
                        ReleaseDate = null
                    },
                    new (new DateTime(2023, 2, 1, 0, 0, 0, DateTimeKind.Utc), 2, Guid.NewGuid(), 1, 0, 5)
                    {
                        Released = true,
                        ReleaseDate = new DateTime(2023, 2, 1, 0, 0, 0, DateTimeKind.Utc)
                    },
                    new (new DateTime(2023, 3, 1, 0, 0, 0, DateTimeKind.Utc), 3, Guid.NewGuid(), 1, 0, 5)
                    {
                        Released = true,
                        ReleaseDate = new DateTime(2023, 3, 1, 0, 0, 0, DateTimeKind.Utc)
                    },
                    new (new DateTime(2023, 4, 1, 0, 0, 0, DateTimeKind.Utc), 4, Guid.NewGuid(), 1, 0, 5)
                    {
                        Released = false,
                        ReleaseDate = null
                    }
                });
            await context.SaveChangesAsync();

            // Act
            var result = await repository.GetAllReleasedVersionsAsync();
            List<GameVersion>? versions = result.ObjectResponse.Cast<GameVersion>().ToList();

            // Assert
            Assert.Equal(2, result.Total);
            Assert.Equal(2, result.ObjectResponse.Count);
            Assert.Equal(new DateTime(2023, 2, 1, 0, 0, 0, DateTimeKind.Utc), versions[0].ReleaseDate);
            Assert.Equal(new DateTime(2023, 3, 1, 0, 0, 0, DateTimeKind.Utc), versions[1].ReleaseDate);
        }

        [Fact]
        public async Task GetAllVersionsAsync_ReturnsCorrectData()
        {
            // Arrange
            var options = GetInMemoryDbContextOptions("GetAllVersionsAsync_Db");
            using var context = new VersionContext(options);
            var repository = new GameVersionRepository(context);

            context.GameVersions.AddRange(new List<GameVersion>
            {
                new (new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc), 1, Guid.NewGuid(), 1, 0, 5)
                {
                    Released = false,
                    ReleaseDate = null
                },
                new (new DateTime(2023, 2, 1, 0, 0, 0, DateTimeKind.Utc), 2, Guid.NewGuid(), 1, 0, 5)
                {
                    Released = true,
                    ReleaseDate = new DateTime(2023, 2, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new (new DateTime(2023, 3, 1, 0, 0, 0, DateTimeKind.Utc), 3, Guid.NewGuid(), 1, 0, 5)
                {
                    Released = true,
                    ReleaseDate = new DateTime(2023, 3, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new (new DateTime(2023, 4, 1, 0, 0, 0, DateTimeKind.Utc), 4, Guid.NewGuid(), 1, 0, 5)
                {
                    Released = false,
                    ReleaseDate = null
                }
            });
            await context.SaveChangesAsync();

            // Act
            var result = await repository.GetAllVersionsAsync();
            List<GameVersion>? versions = result.ObjectResponse.Cast<GameVersion>().ToList();

            // Assert
            Assert.Equal(4, result.Total);
            Assert.Equal(4, versions.Count);
        }
    }
}