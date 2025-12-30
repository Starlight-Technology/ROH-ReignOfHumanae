using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Models.Websocket
{
    public class RealtimeEventTypes
    {
        public const string GetNearbyPlayers = "GetNearbyPlayers";

        public const string SavePlayerPosition = "SavePlayerPosition";
        public const string SavePlayerPositionResponse = "SavePlayerPositionResponse";
    }
}
