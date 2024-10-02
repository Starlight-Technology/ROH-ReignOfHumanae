using System;

using UnityEngine;

namespace Assets.Scripts.Models.Login
{
    [Serializable]
    public class LoginModel
    {
        [SerializeField]
        public string Login { get; set; }
        [SerializeField]
        public string Password { get; set; }
    }
}
