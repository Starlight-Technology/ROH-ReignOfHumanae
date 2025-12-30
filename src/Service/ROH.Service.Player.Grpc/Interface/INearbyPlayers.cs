//-----------------------------------------------------------------------
// <copyright file="INearbyPlayers.cs" company="Starlight-Technology">
//     Author:  
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using Grpc.Core;

using ROH.Contracts.GRPC.Player.NearbyPlayer;

namespace ROH.Service.Player.Grpc.Interface;

public interface INearbyPlayers
{
    Task<NearbyPlayersResponse> GetNearbyPlayers(NearbyPlayersRequest request, ServerCallContext context);
}