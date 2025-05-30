using Assets.Scripts.Connection.ApiConfiguration;
using Assets.Scripts.Models.Character;
using Assets.Scripts.Models.File;
using Assets.Scripts.Models.Response;

using Google.Protobuf.WellKnownTypes;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using UnityEngine;

namespace Assets.Scripts.Connection.Api
{
    public class ApiCharacterService : MonoBehaviour
    {
        public ApiClient _apiClient;

        public Task<List<CharacterModel>> GetAccountCharactersAsync(Guid guid, CancellationToken cancellationToken = default)
        {
            TaskCompletionSource<List<CharacterModel>> tcs = new();

            if (cancellationToken.IsCancellationRequested)
            {
                tcs.SetCanceled();
                return tcs.Task;
            }

            _apiClient.Get<DefaultResponse>(
                $"api/Player/GetAllCharacters?accountGuid={guid}",
                (response) =>
                {
                    if (response != null)
                    {
                        string json = JsonConvert.SerializeObject(response.ObjectResponse);
                        List<CharacterModel> characterList = JsonConvert.DeserializeObject<List<CharacterModel>>(json);
                        tcs.SetResult(characterList);
                    }
                    else
                    {
                        Debug.LogError("Failed to retrieve response.");
                        tcs.SetResult(null);
                    }
                });

            return tcs.Task;
        }

        public Task<CharacterModel> GetCharacterAsync(Guid guid, CancellationToken cancellationToken = default)
        {
            TaskCompletionSource<CharacterModel> tcs = new();

            if (cancellationToken.IsCancellationRequested)
            {
                tcs.SetCanceled();
                return tcs.Task;
            }

            _apiClient.Get<DefaultResponse>(
                $"api/Player/GetCharacter?guid={guid}",
                (response) =>
                {
                    if (response != null)
                    {
                        string json = JsonConvert.SerializeObject(response.ObjectResponse);
                        CharacterModel characterList = JsonConvert.DeserializeObject<CharacterModel>(json);
                        tcs.SetResult(characterList);
                    }
                    else
                    {
                        Debug.LogError("Failed to retrieve response.");
                        tcs.SetResult(null);
                    }
                });

            return tcs.Task;
        }

        public Task<DefaultResponse> CreateCharacterAsync(CharacterModel characterModel, CancellationToken cancellationToken = default)
        {
            TaskCompletionSource<DefaultResponse> tcs = new();

            if (cancellationToken.IsCancellationRequested)
            {
                tcs.SetCanceled();
                return tcs.Task;
            }

            _apiClient.Post<DefaultResponse>(
                $"api/Player/CreateCharacter",
                characterModel,
                (Action<DefaultResponse>)((response) =>
                {
                    if (response != null)
                    {
                        tcs.SetResult(response);
                    }
                    else
                    {
                        Debug.LogError("Failed to retrieve response.");
                        tcs.SetResult(null);
                    }
                }));

            return tcs.Task;
        }

        public void Start()
        {
            GameObject apiClientObject = GameObject.Find("apiClientObj");

            if (apiClientObject == null)
            {
                apiClientObject = new("apiClientObj");
                apiClientObject.AddComponent<ApiClient>();
            }

            _apiClient = apiClientObject.GetComponent<ApiClient>();
            _apiClient.Start();
        }
    }
}