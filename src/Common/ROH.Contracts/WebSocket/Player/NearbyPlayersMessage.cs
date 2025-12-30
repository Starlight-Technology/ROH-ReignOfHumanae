//-----------------------------------------------------------------------
// <copyright file="NearbyPlayersMessage.cs" company="Starlight-Technology">
//     Author:  
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using MessagePack;

namespace ROH.Contracts.WebSocket.Player;

[MessagePackObject]
public class NearbyPlayersMessage
{
    [Key(0)] public List<NearbyPlayerMessage> Players = new List<NearbyPlayerMessage>();
}