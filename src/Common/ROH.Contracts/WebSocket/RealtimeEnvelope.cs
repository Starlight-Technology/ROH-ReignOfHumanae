//-----------------------------------------------------------------------
// <copyright file="RealtimeEnvelope.cs" company="Starlight-Technology">
//     Author:  
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using MessagePack;

namespace ROH.Contracts.WebSocket;

[MessagePackObject]
public class RealtimeEnvelope
{
    [Key(1)] public required byte[] Payload;
    [Key(0)] public required string Type;
}