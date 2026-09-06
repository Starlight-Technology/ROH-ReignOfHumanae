using Assets.Scripts.Models.Websocket;

using MessagePack;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Models.Character
{
    [MessagePackObject]
    public class RealtimeEnvelope
    {
        [Key(0)] public string Type;
        [Key(1)] public byte[] Payload;
    }
}
