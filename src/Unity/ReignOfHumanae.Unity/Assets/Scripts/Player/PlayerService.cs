using Assets.Scripts.Configurations;
using Assets.Scripts.Connection.Api;
using Assets.Scripts.Helpers;
using Assets.Scripts.Models.Character;
using Assets.Scripts.Models.Configuration;

using ROH.Protos.NearbyPlayer;
using ROH.Protos.Player;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.SceneManagement;
using Grpc.Net.Client;
using Grpc.Net.Client.Web;

namespace Assets.Scripts.Player
{
    public class PlayerService : MonoBehaviour
    {
        private GameObject apiCharacter;
        private GameObject playerInstance;
        private ROH.Protos.Player.PlayerService.PlayerServiceClient _playerPositionClient;
        private ROH.Protos.NearbyPlayer.NearbyPlayerService.NearbyPlayerServiceClient _nearbyPlayerClient;
        private CharacterModel player;

        public GameObject PlayerObj; // Prefab base do player

        // Variáveis para otimização do envio de posição
        private Vector3 _lastSentPosition;
        private Quaternion _lastSentRotation;
        private bool _hasLastPosition = false;
        private const float POSITION_THRESHOLD = 0.01f; // Ajuste conforme necessário
        private const float ROTATION_THRESHOLD = 0.5f;  // Ajuste conforme necessário

        // Dicionário para manter referência aos jogadores próximos instanciados
        private readonly Dictionary<string, GameObject> _nearbyPlayers = new();

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

                                    if (playerInstance == null)
                                    {
                                        Debug.LogError("Falha ao instanciar o jogador (playerInstance é null).");
                                        SceneManager.LoadScene("Login");
                                        return;
                                    }

                                    // Instancia o modelo visual como filho
                                    InstantiateCharacter("DefaultMaleHumanoid", playerInstance.transform);

                                    // Configura a câmera
                                    var mainCamera = Camera.main;
                                    if (mainCamera != null)
                                    {
                                        var cameraMovements = mainCamera.GetComponent<CameraMovements>();
                                        if (cameraMovements != null)
                                            cameraMovements.player = playerInstance.transform;
                                    }

                                    var movements = playerInstance.GetComponent<PlayerMovements>();

                                    if (movements != null && mainCamera != null)
                                    {
                                        movements.cameraTransform = mainCamera.transform;
                                        movements.mainCamera = mainCamera;
                                    }

                                    // Inicializa posição/rotação para otimização
                                    _lastSentPosition = playerInstance.transform.position;
                                    _lastSentRotation = playerInstance.transform.rotation;
                                    _hasLastPosition = true;
                                }
                                catch (Exception ex)
                                {
                                    Debug.LogError($"Erro ao instanciar personagem: {ex}");
                                    SceneManager.LoadScene("Login");
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


                var innerHandler = new HttpClientHandler
                {
                    
                };

                var grpcWebHandler = new GrpcWebHandler(GrpcWebMode.GrpcWeb, innerHandler);

                var httpClient = new HttpClient(grpcWebHandler)
                {
                    BaseAddress = new Uri(gateway)
                };

                var channel = GrpcChannel.ForAddress(gateway, new GrpcChannelOptions
                {
                    HttpClient = httpClient,
                    // unsafe só se precisar mesmo:
                    UnsafeUseInsecureChannelCallCredentials = true
                });


                _playerPositionClient = new ROH.Protos.Player.PlayerService.PlayerServiceClient(channel);
                _nearbyPlayerClient = new ROH.Protos.NearbyPlayer.NearbyPlayerService.NearbyPlayerServiceClient(channel);

                StartCoroutine(SendPositionCoroutine());
                StartCoroutine(GetNearbyPlayersCoroutine());
            }
            catch (System.Exception ex)
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

        private async void SendPlayerPosition()
        {
            if (PlayerObj != null && playerInstance != null && _hasLastPosition)
            {
                Vector3 currentPosition = playerInstance.transform.position;
                Quaternion currentRotation = playerInstance.transform.rotation;

                // Só envia se houve movimento ou rotação significativa
                if (Vector3.Distance(currentPosition, _lastSentPosition) < POSITION_THRESHOLD &&
                    Quaternion.Angle(currentRotation, _lastSentRotation) < ROTATION_THRESHOLD)
                {
                    return;
                }

                var request = new PlayerRequest
                {
                    PlayerId = player.Guid.ToString(),
                    Position = new Position
                    {
                        X = currentPosition.x,
                        Y = currentPosition.y,
                        Z = currentPosition.z
                    },
                    Rotation = new Rotation
                    {
                        X = currentRotation.x,
                        Y = currentRotation.y,
                        Z = currentRotation.z,
                        W = currentRotation.w
                    }
                };

                try
                {
                    await _playerPositionClient.SavePlayerDataAsync(request).ConfigureAwait(false);
                    _lastSentPosition = currentPosition;
                    _lastSentRotation = currentRotation;
                }
                catch (Exception ex)
                {
                    Debug.LogWarning($"Erro ao enviar dados do jogador: {ex.Message}");
                    // Tratamento para perda de conexão
                    if (ex is Grpc.Core.RpcException || ex.InnerException is Grpc.Core.RpcException)
                    {
                        Debug.LogError("Conexão perdida com o servidor. Retornando para a tela de login.");
                        UnityMainThreadDispatcher.Instance().Enqueue(() =>
                        {
                            SceneManager.LoadScene("Login");
                        });
                    }
                }
            }
        }

        private IEnumerator GetNearbyPlayersCoroutine()
        {
            while (true)
            {
                GetNearbyPlayers();
                yield return new WaitForSeconds(0.2f);
            }
        }

        private void GetNearbyPlayers()
        {
            try
            {
                var response = _nearbyPlayerClient.GetNearbyPlayers(new ROH.Protos.NearbyPlayer.NearbyPlayersRequest
                {
                    PlayerId = player.Guid.ToString()
                });

                // Mantém uma lista dos IDs recebidos nesta atualização
                HashSet<string> receivedIds = new();

                foreach (var playerInfo in response.Players)
                {
                    string playerId = playerInfo.PlayerId;
                    receivedIds.Add(playerId);

                    // Não instancia o próprio jogador
                    if (playerId == player.Guid.ToString())
                        continue;

                    // Busca prefabName, se existir, senão usa o padrão
                    string prefabName = !string.IsNullOrEmpty(playerInfo.ModelName)
                        ? playerInfo.ModelName
                        : "DefaultMaleHumanoid";

                    // Tenta encontrar o prefab
                    GameObject prefab = Resources.Load<GameObject>($"Characters/{prefabName}");
                    if (prefab == null)
                    {
                        prefab = Resources.Load<GameObject>("Characters/DefaultMaleHumanoid");
                    }

                    // Se já existe, apenas atualiza posição/rotação/animação
                    if (_nearbyPlayers.TryGetValue(playerId, out var instance) && instance != null)
                    {
                        
                        instance.transform.SetPositionAndRotation(new Vector3(
                            playerInfo.X,
                            playerInfo.Y,
                            playerInfo.Z), Quaternion.Euler(
                            playerInfo.RotX,
                            playerInfo.RotY,
                            playerInfo.RotZ));

                        // Sincroniza animação idle (ou outra, se disponível)
                        var animator = instance.GetComponentInChildren<Animator>();
                        if (animator != null)
                        {
                            animator.Play("Idle");
                        }
                    }
                    else
                    {
                        // Instancia novo jogador próximo
                        var position = new Vector3(
                            playerInfo.X,
                            playerInfo.Y,
                            playerInfo.Z);

                        var rotation = Quaternion.Euler(
                            playerInfo.RotX,
                            playerInfo.RotY,
                            playerInfo.RotZ);

                        var newInstance = Instantiate(prefab, position, rotation);

                        // Marca como jogador remoto (opcional: tag, layer, etc)
                        newInstance.name = $"NearbyPlayer_{playerId}";

                        // Sincroniza animação idle
                        var animator = newInstance.GetComponentInChildren<Animator>();
                        if (animator != null)
                        {
                            animator.Play("Idle");
                        }

                        // Se houver script de movimentação, desabilite para jogadores remotos
                        var movement = newInstance.GetComponent<PlayerMovements>();
                        if (movement != null)
                        {
                            movement.enabled = false;
                        }

                        _nearbyPlayers[playerId] = newInstance;
                    }
                }

                // Remove jogadores que não estão mais próximos
                var idsToRemove = new List<string>();
                foreach (var kvp in _nearbyPlayers)
                {
                    if (!receivedIds.Contains(kvp.Key))
                    {
                        if (kvp.Value != null)
                            Destroy(kvp.Value);
                        idsToRemove.Add(kvp.Key);
                    }
                }
                foreach (var id in idsToRemove)
                {
                    _nearbyPlayers.Remove(id);
                }
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"Erro ao buscar jogadores próximos: {ex.Message}");
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
