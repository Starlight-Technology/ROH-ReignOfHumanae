//-----------------------------------------------------------------------
// <copyright file="RealtimeEventTypes.cs" company="Starlight-Technology">
//     Author:  
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace ROH.Contracts.WebSocket;

public class RealtimeEventTypes
{
    public const string ChatError = "ChatError";
    public const string ChatHistoryRequest = "ChatHistoryRequest";
    public const string ChatHistoryResponse = "ChatHistoryResponse";
    public const string ChatMessage = "ChatMessage";
    public const string ChatSend = "ChatSend";
    public const string DisconnectNotice = "DisconnectNotice";
    public const string GetNearbyPlayers = "GetNearbyPlayers";
    public const string InitialPlayerState = "InitialPlayerState";
    public const string SavePlayerPosition = "SavePlayerPosition";
    public const string SavePlayerPositionResponse = "SavePlayerPositionResponse";
    public const string SystemMessage = "SystemMessage";
}
