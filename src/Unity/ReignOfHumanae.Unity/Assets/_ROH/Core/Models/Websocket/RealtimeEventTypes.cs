using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Models.Websocket
{
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
}
