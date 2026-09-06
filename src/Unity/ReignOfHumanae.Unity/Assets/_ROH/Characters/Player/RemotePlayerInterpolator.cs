using System.Collections.Generic;

using UnityEngine;

namespace Assets.Scripts.Player
{

    public class RemotePlayerInterpolator : MonoBehaviour
    {
        private struct Snapshot
        {
            public Vector3 Position;
            public Quaternion Rotation;
            public float Time;
        }

        private const float INTERPOLATION_DELAY = 0.12f; // 120ms
        private const int MAX_BUFFER_SIZE = 20;
        private const float TELEPORT_DISTANCE = 5f;

        private readonly List<Snapshot> _buffer = new();

        private bool _initialized;

        public void Initialize(Vector3 pos, Quaternion rot)
        {
            _buffer.Clear();
            _buffer.Add(new Snapshot
            {
                Position = pos,
                Rotation = rot,
                Time = Time.time
            });

            transform.SetPositionAndRotation(pos, rot);
            _initialized = true;
        }

        public void AddSnapshot(Vector3 pos, Quaternion rot)
        {
            if (!_initialized)
            {
                Initialize(pos, rot);
                return;
            }

            var snap = new Snapshot
            {
                Position = pos,
                Rotation = rot,
                Time = Time.time
            };

            // Garantir ordem temporal
            if (_buffer.Count > 0 && snap.Time <= _buffer[^1].Time)
                snap.Time = _buffer[^1].Time + 0.0001f;

            _buffer.Add(snap);

            if (_buffer.Count > MAX_BUFFER_SIZE)
                _buffer.RemoveAt(0);
        }

        private void Update()
        {
            if (!_initialized || _buffer.Count < 2)
                return;

            float renderTime = Time.time - INTERPOLATION_DELAY;

            Snapshot from = _buffer[0];
            Snapshot to = _buffer[1];

            bool found = false;

            for (int i = 0; i < _buffer.Count - 1; i++)
            {
                if (_buffer[i].Time <= renderTime &&
                    _buffer[i + 1].Time >= renderTime)
                {
                    from = _buffer[i];
                    to = _buffer[i + 1];
                    found = true;
                    break;
                }
            }

            // Se não achou intervalo, segura no último conhecido
            if (!found)
            {
                from = _buffer[^2];
                to = _buffer[^1];
            }

            float t = Mathf.InverseLerp(from.Time, to.Time, renderTime);

            // Anti-teleport legítimo
            if (Vector3.Distance(transform.position, to.Position) > TELEPORT_DISTANCE)
            {
                transform.SetPositionAndRotation(to.Position, to.Rotation);
                _buffer.Clear();
                _buffer.Add(to);
                return;
            }

            transform.position = Vector3.Lerp(from.Position, to.Position, t);
            transform.rotation = Quaternion.Slerp(from.Rotation, to.Rotation, t);
        }
    }

}