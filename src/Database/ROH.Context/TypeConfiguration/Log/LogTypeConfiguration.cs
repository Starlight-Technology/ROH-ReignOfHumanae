//-----------------------------------------------------------------------
// <copyright file="LogTypeConfiguration.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ROH.Context.TypeConfiguration.Log;

public class LogTypeConfiguration : IEntityTypeConfiguration<Domain.Logging.Log>
{
    public void Configure(EntityTypeBuilder<Domain.Logging.Log> builder) => builder.HasKey(x => x.Id);
}
