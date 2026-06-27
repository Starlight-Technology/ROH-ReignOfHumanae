using Assets.Scripts.Models.Character;

using MessagePack;
using MessagePack.Formatters;
using MessagePack.Resolvers;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static MessagePack.GeneratedMessagePackResolver.Assets.Scripts.Models.Character;

namespace Assets.Scripts.Connection.WebSocket
{
    public class GameMessagePackResolver : IFormatterResolver
    {
        public static readonly GameMessagePackResolver Instance = new();

        private static readonly IFormatterResolver[] Resolvers = new IFormatterResolver[]
        {
            BuiltinResolver.Instance,
            AttributeFormatterResolver.Instance,
            DynamicGenericResolver.Instance,
            DynamicUnionResolver.Instance,
            DynamicObjectResolver.Instance,
            PrimitiveObjectResolver.Instance,
            StandardResolver.Instance
        };

        public IMessagePackFormatter<T> GetFormatter<T>()
        {
            return Cache<T>.Formatter;
        }

        private static class Cache<T>
        {
            public static readonly IMessagePackFormatter<T> Formatter;

            static Cache()
            {
                // Primeiro, tente encontrar um formulador específico para RealtimeEnvelope
                if (typeof(T) == typeof(RealtimeEnvelope))
                {
                    Formatter = (IMessagePackFormatter<T>)new RealtimeEnvelopeFormatter();
                    return;
                }

                // Caso contrário, use os resolvers padrão
                foreach (var resolver in Resolvers)
                {
                    var formatter = resolver.GetFormatter<T>();
                    if (formatter != null)
                    {
                        Formatter = formatter;
                        return;
                    }
                }

                Formatter = null;
            }
        }
    }
}
