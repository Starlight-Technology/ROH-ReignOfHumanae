using Microsoft.EntityFrameworkCore;

using ROH.Context.Version;
using ROH.Context.Version.Entities;
using ROH.Context.Version.Repository;

namespace ROH.Test.Version;

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
        List<GameVersion>? versions = [.. result.ObjectResponse.Cast<GameVersion>()];

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
        List<GameVersion>? versions = [.. result.ObjectResponse.Cast<GameVersion>()];

        // Assert
        Assert.Equal(4, result.Total);
        Assert.Equal(4, versions.Count);
    }

    [Fact]
    public async Task GetCurrentGameVersionAsync_ReturnsLatestReleasedVersion()
    {
        // Arrange
        var options = GetInMemoryDbContextOptions("GetCurrentGameVersionAsync_Db");
        using var context = new VersionContext(options);
        var repository = new GameVersionRepository(context);

        context.GameVersions.AddRange(new List<GameVersion>
        {
            new (new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc), 1, Guid.NewGuid(), 1, 0, 5) { Released = true, ReleaseDate = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new (new DateTime(2023, 2, 1, 0, 0, 0, DateTimeKind.Utc), 2, Guid.NewGuid(), 1, 0, 5) { Released = true, ReleaseDate = new DateTime(2023, 2, 1, 0, 0, 0, DateTimeKind.Utc) },
            new (new DateTime(2023, 3, 1, 0, 0, 0, DateTimeKind.Utc), 3, Guid.NewGuid(), 1, 0, 5) { Released = false, ReleaseDate = null }
        });
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetCurrentGameVersionAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(new DateTime(2023, 2, 1, 0, 0, 0, DateTimeKind.Utc), result.ReleaseDate);
    }

    [Fact]
    public async Task GetVersionByGuidAsync_ReturnsCorrectVersion()
    {
        // Arrange
        var options = GetInMemoryDbContextOptions("GetVersionByGuidAsync_Db");
        using var context = new VersionContext(options);
        var repository = new GameVersionRepository(context);

        var guid = Guid.NewGuid();
        context.GameVersions.Add(new(new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc), 1, guid, 1, 0, 5) { Released = true });
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetVersionByGuidAsync(guid);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(guid, result.Guid);
    }

    [Fact]
    public async Task SetNewGameVersionAsync_AddsVersionCorrectly()
    {
        // Arrange
        var options = GetInMemoryDbContextOptions("SetNewGameVersionAsync_Db");
        using var context = new VersionContext(options);
        var repository = new GameVersionRepository(context);

        var version = new GameVersion(new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc), 1, Guid.NewGuid(), 1, 0, 5);

        // Act
        var result = await repository.SetNewGameVersionAsync(version);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(DateTime.UtcNow.Date, result.VersionDate?.Date);
        Assert.Single(context.GameVersions);
    }

    [Fact]
    public async Task UpdateGameVersionAsync_UpdatesVersionCorrectly()
    {
        // Arrange
        var options = GetInMemoryDbContextOptions("UpdateGameVersionAsync_Db");
        using var context = new VersionContext(options);
        var repository = new GameVersionRepository(context);

        var version = new GameVersion(new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc), 1, Guid.NewGuid(), 1, 0, 5);
        context.GameVersions.Add(version);
        await context.SaveChangesAsync();

        // Act
        context.GameVersions.Entry(version).State = EntityState.Detached;
        version = version with { Review = 10 }; // Update property
        var result = await repository.UpdateGameVersionAsync(version);

        // Assert
        Assert.Equal(10, result.Review);
        Assert.Single(context.GameVersions);
    }

    [Fact]
    public async Task VerifyIfExistAsync_ReturnsTrueIfExists()
    {
        // Arrange
        var options = GetInMemoryDbContextOptions("VerifyIfExistAsync_Db");
        using var context = new VersionContext(options);
        var repository = new GameVersionRepository(context);

        var version = new GameVersion(new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc), 1, Guid.NewGuid(), 1, 0, 5);
        context.GameVersions.Add(version);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.VerifyIfExistAsync(version);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task VerifyIfExistAsyncWithGuid_ReturnsTrueIfExists()
    {
        // Arrange
        var options = GetInMemoryDbContextOptions("VerifyIfExistAsyncWithGuid_Db");
        using var context = new VersionContext(options);
        var repository = new GameVersionRepository(context);
        var guid = Guid.NewGuid();

        var version = new GameVersion(new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc), 1, guid, 1, 0, 5);
        context.GameVersions.Add(version);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.VerifyIfExistAsync(guid);

        // Assert
        Assert.True(result);
    }
}