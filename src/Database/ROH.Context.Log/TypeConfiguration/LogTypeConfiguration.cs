//-----------------------------------------------------------------------
// <copyright file="LogTypeConfiguration.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ROH.Context.Log.TypeConfiguration;

public class LogTypeConfiguration : IEntityTypeConfiguration<Entities.Log>
{
    public void Configure(EntityTypeBuilder<Entities.Log> builder) => builder.HasKey(x => x.Id);
}
