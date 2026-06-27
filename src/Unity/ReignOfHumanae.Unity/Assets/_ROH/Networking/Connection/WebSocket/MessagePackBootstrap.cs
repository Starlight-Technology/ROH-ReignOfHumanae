using Assets.Scripts.Models.Character;

using MessagePack;
using MessagePack.Resolvers;

using System;

using UnityEngine;
namespace Assets.Scripts.Connection.WebSocket
{
    public static class MessagePackBootstrap
    {
        private static bool _initialized = false;
        private static MessagePackSerializerOptions _options;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Initialize()
        {
            if (_initialized) return;

            Debug.Log("[MessagePack] Initializing for IL2CPP build...");

            try
            {
                // Configuração explícita para IL2CPP
                StaticCompositeResolver.Instance.Register(
                    MessagePack.Resolvers.StandardResolver.Instance
                );

                var resolver = StaticCompositeResolver.Instance;
                _options = MessagePackSerializerOptions.Standard
                    .WithResolver(resolver)
                    .WithCompression(MessagePackCompression.Lz4BlockArray);

                // Testa a serialização de tipos usados
                TestSerialization();

                _initialized = true;
                Debug.Log("[MessagePack] Initialized successfully for IL2CPP");
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"[MessagePack] Initialization failed: {ex.Message}");
                Debug.LogError(ex.StackTrace);
            }
        }

        private static void TestSerialization()
        {
            // Testa serialização de RealtimeEnvelope
            var testEnvelope = new RealtimeEnvelope
            {
                Type = "test",
                Payload = System.Text.Encoding.UTF8.GetBytes("test")
            };

            var bytes = MessagePackSerializer.Serialize(testEnvelope, _options);
            var deserialized = MessagePackSerializer.Deserialize<RealtimeEnvelope>(bytes, _options);

            Debug.Log($"[MessagePack] Test serialization successful: {deserialized.Type}");
        }

        public static MessagePackSerializerOptions Options => _options;
        public static bool IsInitialized => _initialized;

    }
}
