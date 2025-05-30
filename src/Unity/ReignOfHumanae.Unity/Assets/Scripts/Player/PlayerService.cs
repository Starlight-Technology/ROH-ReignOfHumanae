using Assets.Scripts.Configurations;
using Assets.Scripts.Connection.Api;
using Assets.Scripts.Helpers;
using Assets.Scripts.Models.Character;
using Assets.Scripts.Models.Configuration;

using Grpc.Core;

using ROH.Protos.Player;

using System;
using System.Collections;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Player
{
    public class PlayerService : MonoBehaviour
    {
        private GameObject apiCharacter;
        private GameObject playerInstance;
        private ROH.Protos.Player.PlayerService.PlayerServiceClient _client;
        private Channel _channel;
        private CharacterModel player;

        public GameObject PlayerObj; // Prefab base do player

        public void Start()
        {
            apiCharacter = new("apiCharacter");
            var apiService = apiCharacter.AddComponent<ApiCharacterService>();
            apiService.Start();

            try
            {
                apiService.GetCharacterAsync(GameState.CharacterGuid).ContinueWith(task =>
                {
                    if (task.IsCompleted)
                    {
                        Debug.Log("Character loaded successfully.");

                        player = task.Result;
                        if (player != null)
                        {
                            UnityMainThreadDispatcher.Instance().Enqueue(() =>
                            {
                                try
                                {
                                    // Instancia o jogador com posição e rotação
                                    Vector3 position = new(
                                        player.PlayerPosition.Position.X,
                                        player.PlayerPosition.Position.Y,
                                        player.PlayerPosition.Position.Z);

                                    Quaternion rotation = Quaternion.Euler(
                                        player.PlayerPosition.Rotation.X,
                                        player.PlayerPosition.Rotation.Y,
                                        player.PlayerPosition.Rotation.Z);

                                    playerInstance = Instantiate(PlayerObj, position, rotation);

                                    // Instancia o modelo visual como filho
                                    InstantiateCharacter("DefaultMaleHumanoid", playerInstance.transform);

                                    // Configura a câmera
                                    var mainCamera = Camera.main;
                                    if (mainCamera != null)
                                    {
                                        mainCamera.GetComponent<CameraMovements>().player = playerInstance.transform;
                                    }

                                    var movements = playerInstance.GetComponent<PlayerMovements>();

                                    if (movements != null && mainCamera != null)
                                    {
                                        movements.cameraTransform = mainCamera.transform;
                                        movements.mainCamera = mainCamera;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Debug.LogError($"Erro ao instanciar personagem: {ex}");
                                }
                            });
                        }
                        else
                        {
                            Debug.LogError("Character data is null.");
                            SceneManager.LoadScene("Login");
                        }
                    }
                    else
                    {
                        Debug.LogError($"Failed to load character: {task.Exception?.Message}");
                        SceneManager.LoadScene("Login");
                    }
                });

                ConfigurationModel config = InitialConfiguration.GetInitialConfiguration();

                string gateway = config.ServerUrlGrpc;
                if (gateway.Contains("http://"))
                    gateway = gateway.Replace("http://", "");
                else if (gateway.Contains("https://"))
                    gateway = gateway.Replace("https://", "");

                _channel = new Channel(gateway, ChannelCredentials.Insecure);
                _client = new ROH.Protos.Player.PlayerService.PlayerServiceClient(_channel);

                StartCoroutine(SendPositionCoroutine());
            }
            catch (Exception ex)
            {
                Debug.LogError($"An error occurred while starting PlayerService: {ex.Message}");
                SceneManager.LoadScene("Login");
            }
        }

        private IEnumerator SendPositionCoroutine()
        {
            while (true)
            {
                SendPlayerPosition();
                yield return new WaitForSeconds(0.2f); // Envia a cada 200ms
            }
        }

        private async Task SendPlayerPosition()
        {
            if (PlayerObj != null)
            {    
                var request = new PlayerRequest
                {
                    PlayerId = player.Guid.ToString(),
                    Position = new Position
                    {
                        X = playerInstance.transform.position.x,
                        Y = playerInstance.transform.position.y,
                        Z = playerInstance.transform.position.z
                    },
                    Rotation = new Rotation
                    {
                        X = playerInstance.transform.rotation.x,
                        Y = playerInstance.transform.rotation.y,
                        Z = playerInstance.transform.rotation.z,
                        W = playerInstance.transform.rotation.w
                    }
                };

                try
                {
                    await _client.SavePlayerDataAsync(request).ConfigureAwait(false);
                }
                catch (RpcException ex)
                {
                    Debug.LogWarning($"Erro ao enviar dados do jogador: {ex.Status.Detail}");
                }
            }
        }

        private void InstantiateCharacter(string prefabName, Transform parentTransform)
        {
            GameObject prefab = Resources.Load<GameObject>($"Characters/{prefabName}");

            if (prefab != null)
            {
                GameObject instance = Instantiate(prefab);
                instance.transform.SetParent(parentTransform, worldPositionStays: false);
                instance.transform.localPosition = Vector3.zero;
                instance.transform.localRotation = Quaternion.identity;
            }
            else
            {
                Debug.LogError($"Prefab '{prefabName}' não encontrado em Resources.");
            }
        }
    }
}
