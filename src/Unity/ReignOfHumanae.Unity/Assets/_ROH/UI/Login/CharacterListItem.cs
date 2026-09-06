using System;

using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Login
{
    internal class CharacterListItem : MonoBehaviour
    {
        public Text nameText;
        public Button button;

        private Guid characterGuid;

        public void Initialize(string name, Guid guid, System.Action<Guid> onClick)
        {
            characterGuid = guid;
            nameText.text = name;
            button.onClick.AddListener(() => onClick?.Invoke(characterGuid));
        }
    }
}
