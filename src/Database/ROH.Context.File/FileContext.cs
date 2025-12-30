//-----------------------------------------------------------------------
// <copyright file="FileContext.cs" company="Starlight-Technology">
//     Author:  
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using Microsoft.EntityFrameworkCore;

using ROH.Context.File.Entities;
using ROH.Context.File.Interface;
using ROH.Context.File.TypeConfiguration;

namespace ROH.Context.File;

public class FileContext : DbContext, IFileContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string? connectionString = Environment.GetEnvironmentVariable("ROH_DATABASE_CONNECTION_STRING_FILE");
        _ = optionsBuilder.UseNpgsql(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.ApplyConfiguration(new GameVersionFileTypeConfiguration());
        _ = modelBuilder.ApplyConfiguration(new GameFileTypeConfiguration());

        base.OnModelCreating(modelBuilder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) => base.SaveChangesAsync(
        cancellationToken);

    public DbSet<GameFile> GameFiles { get; set; }

    public DbSet<GameVersionFile> GameVersionFiles { get; set; }
}