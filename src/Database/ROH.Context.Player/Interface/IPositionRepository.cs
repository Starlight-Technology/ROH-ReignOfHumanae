﻿using ROH.Context.Player.Entities.Characters;

namespace ROH.Context.Player.Interface;
public interface IPositionRepository
{
    Task<PlayerPosition?> GetPosition(long idPlayer, CancellationToken token);
    Task SavePosition(PlayerPosition position, CancellationToken token);
    Task UpdatePosition(PlayerPosition position, CancellationToken token);
}

