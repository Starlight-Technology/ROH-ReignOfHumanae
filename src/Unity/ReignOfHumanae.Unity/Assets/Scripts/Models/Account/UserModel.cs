using System;

namespace Assets.Scripts.Models.Account
{
    [Serializable]
    public class UserModel
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
        public Guid Guid { get; set; }
    }
}
