using MessagePack;

using System;
using System.Collections.Generic;
using System.Text;

namespace ROH.StandardModels.WebSocket
{
    [MessagePackObject]
    public class RealtimeEnvelope
    {
        [Key(0)] public string Type;
        [Key(1)] public byte[] Payload;
    }
}
