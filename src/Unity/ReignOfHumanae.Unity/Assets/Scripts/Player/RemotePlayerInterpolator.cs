using Assets.Scripts.Models.Websocket;

using System.Collections.Generic;

using UnityEngine;

namespace Assets.Scripts.Player
{
    public class RemotePlayerInterpolator : MonoBehaviour
    {
        private const int BUFFER_SIZE = 5;
        private const float INTERPOLATION_BACK_TIME = 0.1f;

        private readonly List<PlayerSnapshot> _buffer = new();

        public void AddSnapshot(Vector3 pos, Quaternion rot)
        {

            if (_buffer.Count > 0 &&
                Vector3.Distance(_buffer[^1].Position, pos) > 10f)
            {
                _buffer.Clear();
                transform.position = pos;
                transform.rotation = rot;
            }

            var snapshot = new PlayerSnapshot
            {
                Position = pos,
                Rotation = rot,
                Timestamp = Time.time
            };

            _buffer.Add(snapshot);

            if (_buffer.Count > BUFFER_SIZE)
                _buffer.RemoveAt(0);
        }

        void Update()
        {
            if (_buffer.Count < 2)
                return;

            float renderTime = Time.time - INTERPOLATION_BACK_TIME;

            PlayerSnapshot older = default;
            PlayerSnapshot newer = default;
            bool found = false;

            for (int i = 0; i < _buffer.Count - 1; i++)
            {
                if (_buffer[i].Timestamp <= renderTime &&
                    _buffer[i + 1].Timestamp >= renderTime)
                {
                    older = _buffer[i];
                    newer = _buffer[i + 1];
                    found = true;
                    break;
                }
            }

            if (!found)
                return;

            float t = Mathf.InverseLerp(
                older.Timestamp,
                newer.Timestamp,
                renderTime
            );

            transform.position = Vector3.Lerp(
                older.Position,
                newer.Position,
                t
            );

            transform.rotation = Quaternion.Slerp(
                older.Rotation,
                newer.Rotation,
                t
            );
        }
    }
}