using MessagePack;

using System;
using System.Collections.Generic;
using System.Text;

namespace ROH.Contracts.WebSocket.Player;

[MessagePackObject]
public sealed class PlayerSnapshot
{
    [Key(0)] public string PlayerId;
    [Key(1)] public float X;
    [Key(2)] public float Y;
    [Key(3)] public float Z;
    [Key(4)] public float RotY;
    [Key(5)] public string Animation;
}
