using MessagePack;

using System;

namespace Assets.Scripts.Models.Websocket
{
    [MessagePackObject]
    public class ChatMessageModel
    {
        [Key(0)] public string Id;
        [Key(1)] public ChatChannel Channel;
        [Key(2)] public string SenderCharacterId;
        [Key(3)] public string SenderDisplayName;
        [Key(4)] public string TargetCharacterId;
        [Key(5)] public string[] Tags;
        [Key(6)] public string Message;
        [Key(7)] public DateTime CreatedAtUtc;
    }
}
