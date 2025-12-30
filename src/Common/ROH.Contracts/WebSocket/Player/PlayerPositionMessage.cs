//-----------------------------------------------------------------------
// <copyright file="PlayerPositionMessage.cs" company="Starlight-Technology">
//     Author:  
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using MessagePack;

namespace ROH.Contracts.WebSocket.Player;

[MessagePackObject]
public class PlayerPositionMessage
{
    [Key(10)] public int AnimationState;
    [Key(8)] public string ModelName;
    [Key(0)] public string PlayerId;
    [Key(9)] public float Radius;
    [Key(7)] public float RotW;
    [Key(4)] public float RotX;
    [Key(5)] public float RotY;
    [Key(6)] public float RotZ;
    [Key(1)] public float X;
    [Key(2)] public float Y;
    [Key(3)] public float Z;
}