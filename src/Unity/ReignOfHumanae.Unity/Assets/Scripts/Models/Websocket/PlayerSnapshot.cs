using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

namespace Assets.Scripts.Models.Websocket
{
    public struct PlayerSnapshot
    {
        public Vector3 Position;
        public Quaternion Rotation;
        public float Timestamp;
    }

}
