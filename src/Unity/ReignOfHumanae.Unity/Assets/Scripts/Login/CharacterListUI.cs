using Assets.Scripts.Helpers;
using Assets.Scripts.Models.Character;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Login
{
    public class CharacterListUI : MonoBehaviour
    {
        public Transform contentParent;
        public GameObject characterItemPrefab;

        private Guid selectedCharacterId;

        public void LoadCharacters(List<CharacterModel> characters)
        {
            foreach (Transform child in contentParent)
                Destroy(child.gameObject); // limpa a lista

            foreach (var character in characters)
            {
                GameObject item = Instantiate(characterItemPrefab, contentParent);
                var characterItem = item.GetComponent<CharacterListItem>();

                characterItem.Initialize(character.Name, character.Guid, OnCharacterSelected);
            }
        }

        private void OnCharacterSelected(Guid guid)
        {
            selectedCharacterId = guid;
            GameState.CharacterGuid = guid; // Atualiza o estado do jogo com o personagem selecionado
            LoadGameSceneAsync();
            Debug.Log($"Selecionado: {guid}");
        }

        public async void LoadGameSceneAsync() => await SceneManager.LoadSceneAsync("MainWorld");

        public void LoginSelectedCharacter()
        {
            Debug.Log($"Logando com personagem {selectedCharacterId}");
            // chamar o backend ou carregar cena
        }
    }
}

