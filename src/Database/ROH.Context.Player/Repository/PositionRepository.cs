//-----------------------------------------------------------------------
// <copyright file="PositionRepository.cs" company="Starlight-Technology">
//     Author:  
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using Microsoft.EntityFrameworkCore;

using ROH.Context.Player.Entities.Characters;
using ROH.Context.Player.Interface;

namespace ROH.Context.Player.Repository;

public class PositionRepository(IPlayerContext context) : IPositionRepository
{
    public async Task<PlayerPosition?> GetPosition(long idPlayer, CancellationToken token) => await context.PlayersPosition
        .Include(p => p.Position)
        .Include(p => p.Rotation)
        .FirstOrDefaultAsync(p => p.IdPlayer == idPlayer, token)
        .ConfigureAwait(true);

    public async Task SavePosition(PlayerPosition position, CancellationToken token)
    {
        await context.PlayersPosition.AddAsync(position).ConfigureAwait(true);
        await context.SaveChangesAsync(token).ConfigureAwait(true);
    }

    public async Task UpdatePosition(PlayerPosition position, CancellationToken token)
    {
        context.PlayersPosition.Update(position);
        await context.SaveChangesAsync(token).ConfigureAwait(true);
    }
}