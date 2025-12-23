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

using MessagePack;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Player
{
    public class PlayerService : MonoBehaviour
    {
        private GameObject apiCharacter;
        private GameObject playerInstance;
        private CharacterModel player;

        private WebSocketService _socket;

        public GameObject PlayerObj;

        private Vector3 _lastSentPosition;
        private Quaternion _lastSentRotation;
        private bool _hasLastPosition;

        private const float POSITION_THRESHOLD = 0.01f;
        private const float ROTATION_THRESHOLD = 0.5f;

        private readonly Dictionary<string, GameObject> _nearbyPlayers = new();

        #region Unity Lifecycle

        public void Start()
        {
            apiCharacter = new GameObject("apiCharacter");
            var apiService = apiCharacter.AddComponent<ApiCharacterService>();
            apiService.Start();

            try
            {
                apiService.GetCharacterAsync(GameState.CharacterGuid)
                    .ContinueWith(task =>
                    {
                        if (!task.IsCompletedSuccessfully || task.Result == null)
                        {
                            Debug.LogError("Failed to load character.");
                            SceneManager.LoadScene("Login");
                            return;
                        }

                        player = task.Result;

                        UnityMainThreadDispatcher.Instance().Enqueue(() =>
                        {
                            InitializePlayer();
                            InitializeWebSocket();
                            StartCoroutine(SendPositionCoroutine());
                        });
                    });
            }
            catch (Exception ex)
            {
                Debug.LogError($"PlayerService start error: {ex}");
                SceneManager.LoadScene("Login");
            }
        }

        private void Update()
        {
            _socket?.Dispatch();
        }

        #endregion

        #region Initialization

        private void InitializePlayer()
        {
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

            string wsUrl = $"{baseUrl}/ws?access_token={jwt}";

            _socket = new WebSocketService();

            _socket.OnConnected += () =>
            {
                Debug.Log("[WS] Connected");
            };

            _socket.OnDisconnected += () =>
            {
                Debug.LogWarning("[WS] Disconnected");
            };

            _socket.OnError += error =>
            {
                Debug.LogError($"[WS] Error: {error}");
            };         

            _socket.OnMessage += HandleRealtimeMessage;

            _ = ConnectWebSocketAsync(wsUrl);
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

        private async void SendPlayerPositionWs()
        {
            if (!_hasLastPosition || playerInstance == null)
                return;

            Vector3 pos = playerInstance.transform.position;
            Quaternion rot = playerInstance.transform.rotation;

            if (Vector3.Distance(pos, _lastSentPosition) < POSITION_THRESHOLD &&
                Quaternion.Angle(rot, _lastSentRotation) < ROTATION_THRESHOLD)
                return;

            var msg = new PlayerPositionMessage
            {
                PlayerId = player.Guid.ToString(),
                X = pos.x,
                Y = pos.y,
                Z = pos.z,
                RotX = rot.eulerAngles.x,
                RotY = rot.eulerAngles.y,
                RotZ = rot.eulerAngles.z
            };

            var envelope = new RealtimeEnvelope
            {
                Type = "PlayerPosition",
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
            if (data is not byte[] bytes)
            {
                Debug.LogWarning($"[WS] Unexpected message type: {data?.GetType()}");
                return;
            }

            RealtimeEnvelope env;

            try
            {
                env = MessagePackSerializer.Deserialize<RealtimeEnvelope>(bytes);
            }
            catch (Exception ex)
            {
                Debug.LogError($"[WS] Failed to deserialize envelope: {ex}");
                return;
            }

            switch (env.Type)
            {
                case "NearbyPlayers":
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
            }
        }


        private void UpdateNearbyPlayer(NearbyPlayerMessage info)
        {
            if (info.PlayerId == player.Guid.ToString())
                return;

            if (_nearbyPlayers.TryGetValue(info.PlayerId, out var instance))
            {
                var interpolator = instance.GetComponent<RemotePlayerInterpolator>() ?? instance.AddComponent<RemotePlayerInterpolator>();
                interpolator.AddSnapshot(
                    new Vector3(info.X, info.Y, info.Z),
                    Quaternion.Euler(info.RotX, info.RotY, info.RotZ)
                );
                return;
            }

            string prefabName = string.IsNullOrEmpty(info.ModelName)
                ? "DefaultMaleHumanoid"
                : info.ModelName;

            GameObject prefab =
                Resources.Load<GameObject>($"Characters/{prefabName}") ??
                Resources.Load<GameObject>("Characters/DefaultMaleHumanoid");

            var newInstance = Instantiate(
                prefab,
                new Vector3(info.X, info.Y, info.Z),
                Quaternion.Euler(info.RotX, info.RotY, info.RotZ));

            var interpolatorNew = newInstance.AddComponent<RemotePlayerInterpolator>();
            interpolatorNew.AddSnapshot(
                new Vector3(info.X, info.Y, info.Z),
                Quaternion.Euler(info.RotX, info.RotY, info.RotZ)
            );


            newInstance.name = $"NearbyPlayer_{info.PlayerId}";

            var movement = newInstance.GetComponent<PlayerMovements>();
            if (movement != null)
                movement.enabled = false;

            _nearbyPlayers[info.PlayerId] = newInstance;
        }

        #endregion

        #region Helpers

        private void InstantiateCharacter(string prefabName, Transform parentTransform)
        {
            GameObject prefab = Resources.Load<GameObject>($"Characters/{prefabName}");

            if (prefab == null)
            {
                Debug.LogError($"Prefab '{prefabName}' not found.");
                return;
            }

            GameObject instance = Instantiate(prefab);
            instance.transform.SetParent(parentTransform, false);
            instance.transform.localPosition = Vector3.zero;
            instance.transform.localRotation = Quaternion.identity;
        }

        #endregion
    }
}
