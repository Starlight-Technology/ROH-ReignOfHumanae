using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace ROH.StandardModels.Character.Position
{
    public class PlayerPositionInput
    {
        public PlayerPositionInput(Guid playerId, Vector3 lastServerPosition, Vector3 clientReportedPosition, DateTime lastServerTimestamp, DateTime serverTimestamp)
        {
            PlayerId = playerId;
            LastServerPosition = lastServerPosition;
            ClientReportedPosition = clientReportedPosition;
            LastServerTimestamp = lastServerTimestamp;
            ServerTimestamp = serverTimestamp;
        }

        public Guid PlayerId { get; set; }
        public Vector3 LastServerPosition { get; set; }
        public Vector3 ClientReportedPosition { get; set; }
        public DateTime LastServerTimestamp { get; set; }
        public DateTime ServerTimestamp { get; set; }
    }
}
