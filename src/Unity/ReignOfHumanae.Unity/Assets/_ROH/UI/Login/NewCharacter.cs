using Assets.Scripts.Connection.Api;
using Assets.Scripts.Models.Account;
using Assets.Scripts.Models.Character;

using System;

using UnityEngine;

namespace Assets.Scripts.Login
{
    public class NewCharacter : MonoBehaviour
    {
        public GameObject apiCharacter;
        public GameObject loginServiceObj;
        public GameObject newCharacterObj;
        public GameObject characterListObj;

        UserModel userModel;

        public async void Create()
        {
            var characterModel = new CharacterModel
            {
                GuidAccount = userModel.Guid,
                Name = GameObject.Find("CharacterName").GetComponent<UnityEngine.UI.InputField>().text,
                PlayerPosition = new PlayerPositionModel(),
                Status = new Status(),
                DefenseStatus = new DefenseStatus(),
                AttackStatus = new AttackStatus(),
                Inventory = new System.Collections.Generic.List<CharacterInventory>(),
                Skills = new System.Collections.Generic.List<CharacterSkill>(),
                EquippedItems = new EquippedItems()
            };
            characterModel.PlayerPosition.Position = new PositionModel() { X = -269.73f, Y = 22.069f, Z = 242.6736f };
            characterModel.PlayerPosition.Rotation = new RotationModel() { X = 0f, Y = 0f, Z = 0f };


            try
            {
                apiCharacter.GetComponent<ApiCharacterService>().Start(); // Ensure the service is started before calling CreateCharacterAsync
                await apiCharacter.GetComponent<ApiCharacterService>().CreateCharacterAsync(characterModel).ConfigureAwait(true);

                await loginServiceObj.GetComponent<LoginService>().LoadCharactersAsync(userModel).ConfigureAwait(true);

                newCharacterObj.SetActive(false);
                characterListObj.SetActive(true);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error creating character: {ex.Message}");
            }
        }

        public void Cancel()
        {
            newCharacterObj.SetActive(false);
            characterListObj.SetActive(true);
        }

        void Start()
        {
            apiCharacter = GameObject.Find("apiCharacter");
            if (apiCharacter == null)
            {
                apiCharacter = new("apiCharacter");
                apiCharacter.AddComponent<ApiCharacterService>();
                apiCharacter.GetComponent<ApiCharacterService>().Start();
            }

            userModel = loginServiceObj.GetComponent<LoginService>().user;

        }

    }
}