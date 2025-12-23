using MessagePack;
using MessagePack.Resolvers;

using UnityEngine;
namespace Assets.Scripts.Connection.WebSocket
{
    public static class MessagePackBootstrap
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Init()
        {
            MessagePackSerializer.DefaultOptions =
                MessagePackSerializerOptions.Standard
                    .WithResolver(StandardResolver.Instance);

#if UNITY_EDITOR
            UnityEngine.Debug.Log("[MessagePack] Bootstrap initialized");
#endif
        }
    }

}
