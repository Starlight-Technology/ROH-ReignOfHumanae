//-----------------------------------------------------------------------
// <copyright file="PlayerService.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using Assets.Scripts.Configurations;
using Assets.Scripts.Connection;
using Assets.Scripts.Connection.Api;
using Assets.Scripts.Connection.WebSocket;
using Assets.Scripts.Helpers;
using Assets.Scripts.Models.Character;
using Assets.Scripts.Models.Configuration;
using Assets.Scripts.Models.Websocket;
using Assets.Scripts.UI.Chat;

using MessagePack;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.SceneManagement;

using static UnityEngine.AdaptivePerformance.Provider.AdaptivePerformanceSubsystemDescriptor;

namespace Assets.Scripts.Player
{
    public class PlayerService : MonoBehaviour
    {
        private GameObject apiCharacter;
        private GameObject playerInstance;
        private CharacterModel player;

        private WebSocketService _socket;
        private GlobalChatController _chatController;

        public GameObject PlayerObj;

        private PlayerStatusHUDController _status_HUD;

        private Vector3 _lastSentPosition;
        private Quaternion _lastSentRotation;
        private bool _hasLastPosition;
        private bool _hasReceivedInitialState;

        private const float POSITION_THRESHOLD = 0.01f;
        private const float ROTATION_THRESHOLD = 0.5f;

        private readonly Dictionary<string, GameObject> _nearbyPlayers = new();
        private readonly Dictionary<string, DateTime> _nearbyPlayersLastUpdate = new();

        public static PlayerAnimationState playerAnimationState;

        #region Unity Lifecycle

        public void Start()
        {
            apiCharacter = new GameObject("apiCharacter");
            var apiService = apiCharacter.AddComponent<ApiCharacterService>();
            apiService.Start();
            MessagePackBootstrap.Initialize();

            if (Application.isEditor && GameState.CharacterGuid == Guid.Empty)
            {
                player = new CharacterModel(0, GameState.UserGuid, 0, 0, GameState.CharacterGuid, "GOD TEST", Race.God);
                player.PlayerPosition = new PlayerPositionModel();
                player.PlayerPosition.Position = new PositionModel()
                {
                    X = -320,
                    Y = 30,
                    Z = 240
                };
                player.PlayerPosition.Rotation = new RotationModel();
                UnityMainThreadDispatcher.Instance()
                    .Enqueue(
                        () =>
                        {
                            InitializePlayer();
                        });
            }
            else
            {
                try
                {
                    apiService.GetCharacterAsync(GameState.CharacterGuid)
                        .ContinueWith(
                            task =>
                            {
                                if (!task.IsCompletedSuccessfully || task.Result == null)
                                {
                                    Debug.LogError("Failed to load character.");
                                    SceneManager.LoadScene("Login");
                                    return;
                                }

                                player = task.Result;

                                UnityMainThreadDispatcher.Instance()
                                    .Enqueue(
                                        () =>
                                        {
                                            InitializePlayer();
                                            InitializeChat();
                                            InitializeWebSocket();
                                            StartCoroutine(SendPositionCoroutine());
                                            StartCoroutine(CleanupNearbyPlayerCoroutine());
                                        });
                            });
                }
                catch (Exception ex)
                {
                    Debug.LogError($"PlayerService start error: {ex}");

                    SceneManager.LoadScene("Login");
                }
            }

            _status_HUD = GameObject.FindFirstObjectByType<PlayerStatusHUDController>();
        }

        private void Update()
        {
            _socket?.Dispatch();

            if (_status_HUD == null)
                return;

            if (_status_HUD._hp > 10)
            {
                _status_HUD._hp -= 0.1f;
            }
            else
                _status_HUD._hp = 100f;

            if (_status_HUD._mana > 10)
            {
                _status_HUD._mana -= 0.1f;
            }
            else
                _status_HUD._mana = 100f;

            if (_status_HUD._stamina > 10)
            {
                _status_HUD._stamina -= 0.1f;
            }
            else
                _status_HUD._stamina = 100f;
        }

        #endregion

        #region Initialization

        private void InitializePlayer()
        {
            player.PlayerPosition ??= new PlayerPositionModel();
            player.PlayerPosition.Position ??= new PositionModel();
            player.PlayerPosition.Rotation ??= new RotationModel();

            Vector3 position = new(
                player.PlayerPosition.Position.X,
                player.PlayerPosition.Position.Y,
                player.PlayerPosition.Position.Z);

            Quaternion rotation = Quaternion.Euler(
                player.PlayerPosition.Rotation.X,
                player.PlayerPosition.Rotation.Y,
                player.PlayerPosition.Rotation.Z);

            playerInstance = Instantiate(PlayerObj, position, rotation);

            if (playerInstance == null)
            {
                Debug.LogError("Player instantiation failed.");
                SceneManager.LoadScene("Login");
                return;
            }

            InstantiateCharacter("DefaultMaleHumanoid", playerInstance.transform);

            var mainCamera = Camera.main;
            if (mainCamera != null)
            {
                var cam = mainCamera.GetComponent<CameraMovements>();
                if (cam != null)
                    cam.player = playerInstance.transform;
            }

            var movement = playerInstance.GetComponent<PlayerMovements>();
            if (movement != null && mainCamera != null)
            {
                movement.cameraTransform = mainCamera.transform;
                movement.mainCamera = mainCamera;
            }

            _lastSentPosition = playerInstance.transform.position;
            _lastSentRotation = playerInstance.transform.rotation;
            _hasLastPosition = true;
        }

        private void InitializeWebSocket()
        {
            ConfigurationModel config = InitialConfiguration.GetInitialConfiguration();

            string baseUrl = config.ServerUrlWebSocket.TrimEnd('/');
            string jwt = config.JwToken;
            string endpoint = baseUrl.EndsWith("/ws", StringComparison.OrdinalIgnoreCase)
                ? baseUrl
                : $"{baseUrl}/ws";
            string worldId = SceneManager.GetActiveScene().name;

            string wsUrl =
                $"{endpoint}?access_token={Uri.EscapeDataString(jwt)}"
                + $"&character_id={Uri.EscapeDataString(player.Guid.ToString())}"
                + $"&world_id={Uri.EscapeDataString(worldId)}";

            _socket = new WebSocketService();

            _socket.OnConnected += () =>
            {
                Debug.Log("[WS] Connected");
                _ = SendChatHistoryRequestAsync();
            };

            _socket.OnDisconnected += () =>
            {
                Debug.LogWarning("[WS] Disconnected");
                UnityMainThreadDispatcher.Instance().Enqueue(
                    () => _chatController?.ShowSystemMessage("Conexão realtime encerrada."));
            };

            _socket.OnError += error =>
            {
                Debug.LogError($"[WS] Error: {error}");
            };

            _socket.OnMessage += HandleRealtimeMessage;

            _ = ConnectWebSocketAsync(wsUrl);
        }

        private void InitializeChat()
        {
            _chatController = FindFirstObjectByType<GlobalChatController>();
            if (_chatController == null)
            {
                GameObject chatObject = new("GlobalChat");
                _chatController = chatObject.AddComponent<GlobalChatController>();
            }

            _chatController.Bind(SendGlobalChatMessage);
        }

        private async Task ConnectWebSocketAsync(string wsUrl)
        {
            try
            {
                await _socket.ConnectAsync(wsUrl);
            }
            catch (Exception ex)
            {
                Debug.LogError($"WebSocket connection failed: {ex.Message}");
            }
        }

        #endregion

        #region Position Sync (WebSocket)

        private IEnumerator SendPositionCoroutine()
        {
            while (true)
            {
                SendPlayerPositionWs();
                yield return new WaitForSeconds(0.05f); // 20 TPS
            }
        }

        private IEnumerator CleanupNearbyPlayerCoroutine()
        {
            while (true)
            {
                CleanupNearbyPlayer();
                yield return new WaitForSeconds(5f);
            }
        }
        void CleanupNearbyPlayer()
        {
            var toRemove = new List<string>();

            foreach (var kv in _nearbyPlayersLastUpdate)
            {
                if (kv.Value.AddSeconds(5) < DateTime.UtcNow)
                    toRemove.Add(kv.Key);
            }

            foreach (var id in toRemove)
            {
                if (_nearbyPlayers.TryGetValue(id, out var go))
                    Destroy(go);

                _nearbyPlayers.Remove(id);
                _nearbyPlayersLastUpdate.Remove(id);
            }
        }

        private async void SendPlayerPositionWs()
        {
            if (!_hasLastPosition || !_hasReceivedInitialState || playerInstance == null)
                return;

            Vector3 pos = playerInstance.transform.position;
            Quaternion rot = playerInstance.transform.rotation;

            var msg = new PlayerPositionMessage
            {
                PlayerId = player.Guid.ToString(),
                X = pos.x,
                Y = pos.y,
                Z = pos.z,
                RotX = rot.eulerAngles.x,
                RotY = rot.eulerAngles.y,
                RotZ = rot.eulerAngles.z,
                RotW = rot.w,
                AnimationState = (int)playerInstance.GetComponent<PlayerMovements>().currentAnimState
            };

            var envelope = new RealtimeEnvelope
            {
                Type = RealtimeEventTypes.SavePlayerPosition,
                Payload = MessagePackSerializer.Serialize<PlayerPositionMessage>(msg)
            };

            await _socket.SendAsync(envelope);

            _lastSentPosition = pos;
            _lastSentRotation = rot;
        }

        #endregion

        #region Realtime Receive

        private void HandleRealtimeMessage(object data)
        {
            if (data is not RealtimeEnvelope env)
            {
                Debug.LogWarning($"[WS] Unexpected message type: {data?.GetType()}");
                return;
            }

            switch (env.Type)
            {
                case RealtimeEventTypes.InitialPlayerState:
                    {
                        var msg = MessagePackSerializer
                            .Deserialize<InitialPlayerStateMessage>(env.Payload);

                        UnityMainThreadDispatcher.Instance().Enqueue(() => ApplyInitialPlayerState(msg));
                        break;
                    }
                case RealtimeEventTypes.GetNearbyPlayers:
                    {
                        var msg = MessagePackSerializer
                            .Deserialize<NearbyPlayersMessage>(env.Payload);

                        UnityMainThreadDispatcher.Instance().Enqueue(() =>
                        {
                            foreach (var playerInfo in msg.Players)
                                UpdateNearbyPlayer(playerInfo);
                        });
                        break;
                    }
                case RealtimeEventTypes.ChatMessage:
                    {
                        var msg = MessagePackSerializer.Deserialize<ChatMessageModel>(env.Payload);
                        UnityMainThreadDispatcher.Instance().Enqueue(
                            () => _chatController?.AppendMessage(msg));
                        break;
                    }
                case RealtimeEventTypes.ChatHistoryResponse:
                    {
                        var msg = MessagePackSerializer.Deserialize<ChatHistoryResponse>(env.Payload);
                        UnityMainThreadDispatcher.Instance().Enqueue(
                            () => _chatController?.LoadHistory(msg.Messages));
                        break;
                    }
                case RealtimeEventTypes.ChatError:
                    {
                        var msg = MessagePackSerializer.Deserialize<ChatErrorMessage>(env.Payload);
                        UnityMainThreadDispatcher.Instance().Enqueue(
                            () => _chatController?.ShowSystemMessage(msg.Message));
                        break;
                    }
                case RealtimeEventTypes.DisconnectNotice:
                    {
                        var msg = MessagePackSerializer.Deserialize<DisconnectNoticeMessage>(env.Payload);
                        UnityMainThreadDispatcher.Instance().Enqueue(
                            () => _chatController?.ShowSystemMessage(msg.Message));
                        break;
                    }
            }
        }

        private void ApplyInitialPlayerState(InitialPlayerStateMessage state)
        {
            if (playerInstance == null)
                return;

            Vector3 position = new(state.X, state.Y, state.Z);
            Quaternion rotation = Quaternion.Euler(state.RotX, state.RotY, state.RotZ);
            playerInstance.transform.SetPositionAndRotation(position, rotation);

            player.PlayerPosition.Position.X = state.X;
            player.PlayerPosition.Position.Y = state.Y;
            player.PlayerPosition.Position.Z = state.Z;
            player.PlayerPosition.Rotation.X = state.RotX;
            player.PlayerPosition.Rotation.Y = state.RotY;
            player.PlayerPosition.Rotation.Z = state.RotZ;
            player.PlayerPosition.Rotation.W = state.RotW;

            _lastSentPosition = position;
            _lastSentRotation = rotation;
            _hasReceivedInitialState = true;
        }

        private async void SendGlobalChatMessage(string message)
        {
            if (_socket == null)
                return;

            var request = new ChatSendMessage
            {
                Channel = ChatChannel.Global,
                Message = message
            };

            await _socket.SendAsync(
                new RealtimeEnvelope
                {
                    Type = RealtimeEventTypes.ChatSend,
                    Payload = MessagePackSerializer.Serialize(request)
                });
        }

        private async Task SendChatHistoryRequestAsync()
        {
            if (_socket == null)
                return;

            var request = new ChatHistoryRequest
            {
                Channel = ChatChannel.Global,
                Limit = 50
            };

            await _socket.SendAsync(
                new RealtimeEnvelope
                {
                    Type = RealtimeEventTypes.ChatHistoryRequest,
                    Payload = MessagePackSerializer.Serialize(request)
                });
        }

        private void UpdateNearbyPlayer(NearbyPlayerMessage info)
        {
            if (info.PlayerId == player.Guid.ToString())
                return;

            if (_nearbyPlayers.TryGetValue(info.PlayerId, out var instance))
            {
                _nearbyPlayersLastUpdate[info.PlayerId] = DateTime.UtcNow;

                var interpolator =
                    instance.GetComponent<RemotePlayerInterpolator>();

                interpolator.AddSnapshot(
                    new Vector3(info.X, info.Y, info.Z),
                    Quaternion.Euler(info.RotX, info.RotY, info.RotZ)
                );

                var animator = instance.GetComponent<Animator>();
                PlayerAnimator.ApplyAnimatorState(
                    (PlayerAnimationState)info.AnimationState,
                    animator
                );
            }
            else
            {
                Vector3 vector = new(info.X, info.Y, info.Z);
                Quaternion quat = Quaternion.Euler(info.RotX, info.RotY, info.RotZ);
                var obj = new GameObject($"Player-{info.PlayerId}");
                obj.transform.position = vector;
                obj.transform.rotation = quat;
                var newInstance = InstantiateCharacter(info.ModelName, obj.transform);
                newInstance.AddComponent<RemotePlayerInterpolator>();
                var interpolator = newInstance.GetComponent<RemotePlayerInterpolator>();

                interpolator.Initialize(
                    newInstance.transform.position,
                    newInstance.transform.rotation);

                _nearbyPlayers[info.PlayerId] = newInstance;
                _nearbyPlayersLastUpdate[info.PlayerId] = DateTime.UtcNow;
            }
        }

        #endregion

        #region Helpers

        private static GameObject? InstantiateCharacter(string prefabName, Transform parentTransform)
        {
            GameObject prefab = Resources.Load<GameObject>($"Characters/{prefabName}") ?? Resources.Load<GameObject>($"Characters/DefaultMaleHumanoid");
            GameObject instance = Instantiate(prefab);
            instance.transform.SetParent(parentTransform, false);
            instance.transform.localPosition = Vector3.zero;
            instance.transform.localRotation = Quaternion.identity;

            return instance;
        }

        #endregion

        private async void OnDestroy()
        {
            if (_socket != null)
                await _socket.CloseAsync();
        }
    }
}
