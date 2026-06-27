using Assets.Scripts.Models.Character;

using MessagePack;
using MessagePack.Formatters;

using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Connection.WebSocket
{
    public class RealtimeEnvelopeFormatter : IMessagePackFormatter<RealtimeEnvelope>
    {
        public void Serialize(ref MessagePackWriter writer, RealtimeEnvelope value, MessagePackSerializerOptions options)
        {
            writer.WriteArrayHeader(2);

            // Fix: Use writer.Write(string) overload for string values
            writer.Write(value.Type);

            if (value.Payload is byte[] bytes)
            {
                writer.Write(bytes);
            }
            else
            {
                // Serializa o objeto usando o resolver padrão
                var payloadBytes = MessagePackSerializer.Serialize(value.Payload, MessagePackSerializerOptions.Standard);
                writer.Write(payloadBytes);
            }
        }

        public RealtimeEnvelope Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
        {
            var envelope = new RealtimeEnvelope();

            var arrayLength = reader.ReadArrayHeader();
            if (arrayLength != 2) throw new MessagePackSerializationException("Invalid RealtimeEnvelope format");

            envelope.Type = reader.ReadString();

            // Lê o payload como bytes
            var payloadBytes = reader.ReadBytes();
            if (payloadBytes.HasValue)
            {
                envelope.Payload = payloadBytes.Value.ToArray();
            }

            return envelope;
        }
    }
}
