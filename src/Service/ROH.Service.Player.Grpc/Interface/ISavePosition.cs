using Grpc.Core;

using ROH.Contracts.GRPC.Player.PlayerPosition;

namespace ROH.Service.Player.Grpc.Interface;

public interface ISavePosition
{
    Task<SaveResponse> SavePlayerData(PlayerRequest request, ServerCallContext context);
}